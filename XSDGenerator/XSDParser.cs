using System.Xml.Schema;

namespace XSDGenerator;

public static class XSDParser
{
	public static string ParseComplexType(XmlSchemaComplexType complexType)
	{
		var name = complexType.Name;
		var content = complexType.Particle;

		var items = ParseParticle(content);

		return $$"""
			public class {{Titleize(name)}}
			{
			{{String.Join("\n\n", items)}}
			}
			""";
	}

	public static IEnumerable<string> ParseSequence(XmlSchemaSequence sequence)
	{
		return sequence.Items
			.Cast<XmlSchemaParticle>()
			.SelectMany(ParseParticle);
	}

	public static IEnumerable<string> ParseChoice(XmlSchemaChoice choice)
	{
		return choice.Items
			.Cast<XmlSchemaParticle>()
			.SelectMany(ParseParticle);
	}

	public static IEnumerable<string> ParseElement(XmlSchemaElement element)
	{
		if (element.SchemaType is null)
		{
			var ending = " { get; set; }";
			var typeName = GetFriendlyName(element.SchemaTypeName.Name);

			// if (String.IsNullOrEmpty(element.FixedValue))
			// {
			// 	var fixedValue = element.FixedValue;
			//
			// 	if (typeName == "string")
			// 	{
			// 		fixedValue = $"\"{fixedValue}\"";
			// 	}
			//
			// 	ending = $"{{ get; }} = {fixedValue};";
			// }

			if (element.MinOccurs > 1 || element.MaxOccurs > 1)
			{
				yield return $"""
						[XmlElement(ElementName = "{element.Name}")]
						public {typeName}[] {Titleize(element.Name)}{ending}
					""";
			}
			else
			{
				yield return $"""
						[XmlElement(ElementName = "{element.Name}")]
						public {typeName} {Titleize(element.Name)}{ending}
					""";
			}
		}
	}

	private static IEnumerable<string> ParseParticle(XmlSchemaParticle? particle)
	{
		return particle switch
		{
			XmlSchemaSequence sequence => ParseSequence(sequence),
			XmlSchemaElement element => ParseElement(element),
			XmlSchemaChoice choice => ParseChoice(choice),
			_ => Enumerable.Empty<string>(),
		};
	}

	private static string? Titleize(string? source)
	{
		if (String.IsNullOrEmpty(source) || Char.IsUpper(source![0]))
		{
			return source;
		}

		return Char.ToUpper(source[0]) + source.Substring(1);
	}

	private static string GetFriendlyName(string type)
	{
		if (type.IndexOf(':') is var index and > -1)
		{
			type = type.Substring(index);
		}

		if (type is not "int" 
			and not "short" 
			and not "byte" 
			and not "bool" 
			and not "long" 
			and not "float" 
			and not "double" 
			and not "decimal" 
			and not "string")
		{
			type = Titleize(type) ?? String.Empty;
		}

		return type;
	}
}