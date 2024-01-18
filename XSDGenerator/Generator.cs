using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Xml.Schema;
using XSDGenerator.Model;
using System.Linq;

namespace XSDGenerator;

[Generator]
public class Generator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var files = context.AdditionalTextsProvider
			.Where(x => String.Equals(Path.GetExtension(x.Path), ".xsd"))
			.Select((s, token) => (Path.GetFileNameWithoutExtension(s.Path), s.GetText(token).ToString()));

		var compilationAndFiles = context.CompilationProvider.Combine(files.Collect());

		context.RegisterSourceOutput(compilationAndFiles, Generate);
	}

	void Generate(SourceProductionContext context, (Compilation compilation, ImmutableArray<(string, string)> files) compilationAndFiles)
	{
		foreach (var file in compilationAndFiles.files)
		{
			var classes = ParseFile(file.Item2);

			context.AddSource(file.Item1, $"""
				using System;
				using System.Xml.Serialization;
				
				{String.Join("\n\n", classes.Where(w => !String.IsNullOrEmpty(w)))}
				""");
		}
	}

	public IEnumerable<string> ParseFile(string file)
	{
		var schema = XmlParser.Parse(file);

		if (schema == null)
		{
			return Enumerable.Empty<string>();
		}

		return schema.Items
			.Cast<object>()
			.SelectMany(s =>
			{
				return s switch
				{
					XmlSchemaComplexType complexType => XSDParser.ParseComplexType(complexType),
					XmlSchemaSimpleType simpleType => XSDParser.ParseSimpleType(simpleType),
					XmlSchemaElement element => XSDParser.ParseElement(element),
					_ => Enumerable.Empty<string>(),
				};
			});
	}
}
