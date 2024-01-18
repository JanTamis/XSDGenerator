using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Xml.Schema;

namespace XSDGenerator;

[Generator]
public class Generator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var files = context.AdditionalTextsProvider
			.Where(x => String.Equals(Path.GetExtension(x.Path), ".xsd"))
			.Select((s, token) => (s.Path, s.GetText(token).ToString()));

		var compilationAndFiles = context.CompilationProvider.Combine(files.Collect());
		
		context.RegisterSourceOutput(compilationAndFiles, Generate);
	}

	void Generate(SourceProductionContext context, (Compilation compilation, ImmutableArray<(string, string)> files) compilationAndFiles)
	{
		var path = compilationAndFiles.compilation.SyntaxTrees
			.Select(s => Path.GetDirectoryName(s.FilePath))
			.FirstOrDefault() ?? String.Empty;

		var rootNamespace = compilationAndFiles.compilation.AssemblyName ?? path.Split(Path.DirectorySeparatorChar).Last();
		
		foreach (var file in compilationAndFiles.files)
		{
			var classes = ParseFile(file.Item2);

			var filePath = file.Item1.Substring(path.Length);

			if (filePath.LastIndexOf(Path.DirectorySeparatorChar) is var index and not -1)
			{
				filePath = filePath.Substring(0, index);
			}
			
			var space = rootNamespace + filePath.Replace(Path.DirectorySeparatorChar, '.');

			context.AddSource(Path.GetFileNameWithoutExtension(file.Item1), $"""
				using System;
				using System.Xml.Serialization;

				namespace {space};

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
			.Cast<XmlSchemaObject>()
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
