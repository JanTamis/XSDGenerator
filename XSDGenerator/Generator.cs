using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Xml.Schema;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
				
				{String.Join("\n\n", classes)}
				""");
		}
	}

	public IEnumerable<string> ParseFile(string file)
	{
		var document = XmlParser.Parse(file);
		var root = XmlParser.GetSchema(document);

		foreach (var complexType in XmlParser.GetComplexTypes(root))
		{
			var properties = XmlParser.GetElements(XmlParser.GetSequence(complexType))
				.Select(XmlParser.GetProperty)
				.Concat(XmlParser.GetChoice(XmlParser.GetSequence(complexType)).Select(XmlParser.GetChoiceProperty))
				.Concat(XmlParser.GetSimpleContent(complexType).SelectMany(XmlParser.GetAttributes).Select(XmlParser.GetProperty));

			var name = XmlParser.GetName(complexType);

			yield return $$"""
				public class {{name}}
				{
					{{String.Join("\n\n\t", properties)}}
				}
				""";
		}

		foreach (var simpleType in XmlParser.GetSimpleTypes(root))
		{
			var restriction = XmlParser.GetRestriction(simpleType);
			var enumeration = XmlParser.GetEnumeration(restriction);
			var name = Titleize(XmlParser.GetName(simpleType));

			if (enumeration.Any())
			{
				yield return $$"""
					public enum {{Titleize(name)}}
					{
						{{String.Join("\n\n\t", enumeration.Select(XmlParser.GetEnumProperty))}}
					}
					""";
			}
		}
	}

	private static string Titleize(string source)
	{
		if (String.IsNullOrEmpty(source) || Char.IsUpper(source[0]))
		{
			return source;
		}

		return Char.ToUpper(source[0]) + source.Substring(1);
	}

	public static string GetFriendlyName(Type type)
	{
		if (type == typeof(int))
			return "int";
		if (type == typeof(short))
			return "short";
		if (type == typeof(byte))
			return "byte";
		if (type == typeof(bool))
			return "bool";
		if (type == typeof(long))
			return "long";
		if (type == typeof(float))
			return "float";
		if (type == typeof(double))
			return "double";
		if (type == typeof(decimal))
			return "decimal";
		if (type == typeof(string))
			return "string";
		
		if (type.IsGenericType)
		{
			return $"{type.Name.Split('`')[0]}<{String.Join(", ", type.GetGenericArguments().Select(GetFriendlyName))}>";
		}

		return type.Name;
	}
}
