using XSDGenerator.Model;

namespace XSDGenerator;

public static class XSDParser
{
	public static string ParseComplexType(XmlComplexType complexType)
	{
		var name = complexType.Name;
		var content = complexType.Particle;

		var items = ParseParticle(content);

		return $$"""
			public class {{Titleize(name)}}
			{
				{{String.Join("\n\n\t", items)}}
			}
			""";
	}

	public static IEnumerable<string> ParseSequence(XmlSequence sequence)
	{
		return sequence.Items
			.SelectMany(ParseParticle);
	}

	public static IEnumerable<string> ParseChoice(XmlChoice choice)
	{
		return choice.Items
			.SelectMany(ParseParticle);
	}

	public static IEnumerable<string> ParseElement(XmlElement element)
	{
		if (element.SchemaType is null)
		{
			yield return $@"[XmlElement(ElementName = ""{element.Name}"")]";

			var ending = " { get; set; }";
			var typeName = GetFriendlyName(element.SchemaTypeName.Name);

			if (String.IsNullOrEmpty(element.FixedValue))
			{
				var fixedValue = element.FixedValue;

				if (typeName == "string")
				{
					fixedValue = $"\"{fixedValue}\"";
				}

				ending = $"{{ get; }} = {fixedValue};";
			}

			if ((element.MinOccurs != null && element.MinOccurs.Value > 1) ||
				(element.MaxOccurs != null && element.MaxOccurs.Value > 1))
			{
				yield return $"public {typeName}[] {Titleize(element.Name)}{ending}";
			}
			else
			{
				yield return $"public {typeName} {Titleize(element.Name)}{ending}";
			}
		}
	}

	private static IEnumerable<string> ParseParticle(XmlParticle? particle)
	{
		return particle switch
		{
			XmlSequence sequence => ParseSequence(sequence),
			XmlElement element => ParseElement(element),
			XmlChoice choice => ParseChoice(choice),
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