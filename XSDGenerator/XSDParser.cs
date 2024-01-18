using System.Linq;
using System.Xml.Schema;

namespace XSDGenerator;

public static class XSDParser
{
	public static IEnumerable<string> ParseComplexType(XmlSchemaComplexType complexType, string optionalName = "")
	{
		var name = complexType.Name ?? optionalName;
		var content = complexType.Particle;

		var items = ParseParticle(content)
			.Concat(ParseContentModel(complexType.ContentModel))
			.Concat(complexType.Attributes
				.OfType<XmlSchemaAttribute>()
				.Where(w => w is not null)
				.SelectMany(ParseAttribute));

		var documentation = complexType.Annotation?.Items is not null
			? String.Join("\n///", complexType.Annotation.Items
					.OfType<XmlSchemaDocumentation>()
					.SelectMany(s => s.Markup.Select(x => x.InnerText)))
			: String.Empty;

		var isAbstract = complexType.IsAbstract;

		var extraData = String.Empty;

		if (complexType.IsAbstract)
		{
			extraData += " abstract";
		}

		if (!String.IsNullOrWhiteSpace(documentation))
		{
			yield return $$"""
				/// <summary>
				/// {{documentation}}
				/// </summary>
				public{{extraData}} class {{name}}
				{
				{{String.Join("\n\n", items)}}
				}
				""";
		}
		else
		{
			yield return $$"""
				public{{extraData}} class {{name}}
				{
				{{String.Join("\n\n", items)}}
				}
				""";
		}
		
	}

	public static IEnumerable<string> ParseSimpleType(XmlSchemaSimpleType simpleType, string optionalName = "")
	{
		if (simpleType.Content is XmlSchemaSimpleTypeUnion union)
		{
			return union.BaseTypes
				.Cast<XmlSchemaSimpleType>()
				.SelectMany(s => ParseSimpleType(s, optionalName));
		}

		if (simpleType.Content is XmlSchemaSimpleTypeList list)
		{
			return ParseSimpleType(list.ItemType);
		}

		if (simpleType.Content is XmlSchemaSimpleTypeRestriction restriction)
		{
			return ParseSimpleTypeRestriction(restriction, simpleType);
		}

		return Enumerable.Empty<string>();
	}

	public static IEnumerable<string> ParseContentModel(XmlSchemaContentModel content)
	{
		return content switch
		{
			XmlSchemaSimpleContent simpleContent => ParseSimpleContent(simpleContent),
			// XmlSchemaComplexContent complexContent
			_ => Enumerable.Empty<string>(),
		};
	}

	public static IEnumerable<string> ParseSimpleContent(XmlSchemaSimpleContent simpleContent)
	{
		switch (simpleContent.Content)
		{
			case XmlSchemaSimpleContentExtension extension:

				return extension.Attributes
					.Cast<XmlSchemaObject>()
					.SelectMany(s =>
					{
						return s switch
						{
							XmlSchemaAttribute attribute => ParseAttribute(attribute),
							// XmlSchemaAttributeGroupRef groupRef
							_ => Enumerable.Empty<string>()
						};
					});
		}

		return Enumerable.Empty<string>();
	}

	public static IEnumerable<string> ParseSimpleTypeRestriction(XmlSchemaSimpleTypeRestriction restriction, XmlSchemaSimpleType simpleType)
	{
		var facets = restriction.Facets.Cast<XmlSchemaFacet>();
		var isEnum = facets.All(a => a is XmlSchemaEnumerationFacet);

		if (isEnum)
		{
			var items = facets.Select(s =>
			{
				return $"""
						[XmlEnum("{s.Value}")]
						{Titleize(s.Value)},
					""";
			});

			yield return $$"""
				public enum {{Titleize(simpleType.Name)}}
				{
				{{String.Join("\n\n", items)}}
				}
				""";
		}
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

			if (!String.IsNullOrEmpty(element.FixedValue))
			{
				var fixedValue = element.FixedValue;

				if (typeName == "string")
				{
					fixedValue = $"\"{fixedValue}\"";
				}

				ending = $"{{ get; }} = {fixedValue};";
			}

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
		else
		{
			if (element.SchemaType is XmlSchemaSimpleType simpleType)
			{
				foreach (var item in ParseSimpleType(simpleType, element.Name))
				{
					yield return item;
				}
			}
			else if (element.SchemaType is XmlSchemaComplexType complexType) 
			{
				foreach (var item in ParseComplexType(complexType, element.Name))
				{
					yield return item;
				}
			}
		}
	}

	public static IEnumerable<string> ParseAttribute(XmlSchemaAttribute attribute)
	{
		if (attribute.SchemaType is null)
		{
			var ending = " { get; set; }";
			var typeName = GetFriendlyName(attribute.SchemaTypeName.Name);

			if (!String.IsNullOrEmpty(attribute.FixedValue))
			{
				var fixedValue = attribute.FixedValue;

				if (typeName == "string")
				{
					fixedValue = $"\"{fixedValue}\"";
				}

				ending = $" {{ get; }} = {fixedValue};";
			}
			else if (!String.IsNullOrEmpty(attribute.DefaultValue))
			{
				var defaultValue = attribute.DefaultValue;

				if (typeName == "string")
				{
					defaultValue = $"\"{defaultValue}\"";
				}

				ending = $" {{ get; set; }} = {defaultValue};";
			}

			yield return $"""
					[XmlAttribute("{attribute.Name}")]
					public {typeName} {Titleize(attribute.Name)}{ending}
				""";
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
		if (String.IsNullOrEmpty(source))
		{
			return source;
		}

		source = String.Join(String.Empty, source
			.Split(' ', '-')
			.Select(i => Char.ToUpper(i[0]) + i.Substring(1)));

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

		if (type == "Boolean")
		{
			type = "bool";
		}

		return type;
	}
}