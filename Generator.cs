using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Xml.Schema;

namespace XSDGenerator;

[Generator]
public class Generator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var files = context.AdditionalTextsProvider
			.Where(x => String.Equals(Path.GetExtension(x.Path), ".xsd"))
			.Select((s, token) => s.GetText(token).ToString());

		var compilationAndFiles = context.CompilationProvider.Combine(files.Collect());

		context.RegisterSourceOutput(compilationAndFiles, Generate);
	}

	void Generate(SourceProductionContext context, (Compilation compilation, ImmutableArray<string> files) compilationAndFiles)
	{
		var set = new XmlSchemaSet();

		foreach (var file in compilationAndFiles.files)
		{
			set.Add(XmlSchema.Read(new StringReader(file), null));
		}

		set.Compile();

		foreach (XmlSchema schema in set.Schemas())
		{
			foreach (XmlSchemaObject element in schema.Elements.Values)
			{
				var list = new List<(string, string)>();
				var data = ParseElement(element, list);

				foreach (var (name, source) in list)
				{
					context.AddSource(name, source);
				}				
			}
		}
	}

	private string ParseElement(XmlSchemaObject item, List<(string, string)> classes)
	{
		if (item is XmlSchemaElement element)
		{
			if (element.ElementSchemaType is XmlSchemaComplexType { Particle: XmlSchemaSequence particle } complexType)
			{
				var builder = new StringBuilder();
				builder.AppendLine($"public class {Titleize(element.Name)}");
				builder.AppendLine("{");

				foreach (var child in particle.Items)
				{
					if (child is not null)
					{
						var classcount = classes.Count;
						var text = ParseElement(child, classes);

						if (classcount == classes.Count)
						{
							builder.AppendLine(text);
						}						
					}
				}

				builder.AppendLine("}");

				classes.Add((element.Name, builder.ToString()));

				//return builder.ToString();
			}
			else if (element.ElementSchemaType is XmlSchemaSimpleType simple)
			{
				return $"\tpublic {GetFriendlyName(simple.Datatype.ValueType)} {Titleize(element.Name)} {{ get; set; }}";
			}
		}

		return String.Empty;
	}

	private string Titleize(string source)
	{
		return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(source);
	}

	public static string GetFriendlyName(Type type)
	{
		if (type == typeof(int))
			return "int";
		else if (type == typeof(short))
			return "short";
		else if (type == typeof(byte))
			return "byte";
		else if (type == typeof(bool))
			return "bool";
		else if (type == typeof(long))
			return "long";
		else if (type == typeof(float))
			return "float";
		else if (type == typeof(double))
			return "double";
		else if (type == typeof(decimal))
			return "decimal";
		else if (type == typeof(string))
			return "string";
		else if (type.IsGenericType)
			return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(x => GetFriendlyName(x)).ToArray()) + ">";
		else
			return type.Name;
	}

	private string GetIndent(int indent)
	{
		return new string('\t', indent);
	}
}
