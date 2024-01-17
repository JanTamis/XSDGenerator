using System.Globalization;
using System.Text;
using System.Xml;

namespace XSDGenerator;

public static class XmlParser
{
	/// <summary>
	/// Parses the provided XML string into an XmlDocument.
	/// </summary>
	/// <param name="xml">The XML string to parse.</param>
	/// <returns>An XmlDocument that represents the provided XML string.</returns>
	public static XmlDocument Parse(string xml)
	{
		var document = new XmlDocument();

		document.LoadXml(xml);

		return document;
	}

	public static XmlElement? GetSchema(XmlDocument document)
	{
		return document.DocumentElement;
	}
	
	public static IEnumerable<XmlElement> GetComplexTypes(XmlElement? schema)
	{
		if (schema is null)
		{
			return Enumerable.Empty<XmlElement>();
		}
		
		return schema.GetElementsByTagName("xs:complexType").Cast<XmlElement>();
	}

	public static IEnumerable<XmlElement> GetSimpleTypes(XmlElement? element)
	{
		if (element is null)
		{
			return Enumerable.Empty<XmlElement>();
		}

		return element.GetElementsByTagName("xs:simpleType").Cast<XmlElement>();
	}
	
	public static XmlElement GetRestriction(XmlElement element)
	{
		return element.GetElementsByTagName("xs:restriction")
			.Cast<XmlElement>()
			.FirstOrDefault();
	}
	
	public static IEnumerable<XmlElement> GetEnumeration(XmlElement element)
	{
		return element.GetElementsByTagName("xs:enumeration")
			.Cast<XmlElement>();
	}
	
	public static string GetEnumProperty(XmlElement element)
	{
		var value = element.GetAttribute("value");
		
		var enumName = String.Join(String.Empty, value.Split(' ')
			.Select(s =>
			{
				var builder = new StringBuilder();

				for (var i = 0; i < s.Length; i++)
				{
					if (IsValidInIdentifier(s[i], i == 0))
					{
						builder.Append(s[i]);
					}
				}

				return builder.ToString();
			})
			.Select(Titleize));

		return $"""
			[XmlEnum("{value}")]
				{enumName},
			""";
	}

	public static XmlElement GetSequence(XmlElement element)
	{
		return element.GetElementsByTagName("xs:sequence")
			.Cast<XmlElement>()
			.FirstOrDefault();
	}

	public static IEnumerable<XmlElement> GetAttributes(XmlElement element)
	{
		return element.GetElementsByTagName("xs:attribute")
			.Cast<XmlElement>();
	}
	
	public static IEnumerable<XmlElement> GetElements(XmlElement? schema)
	{
		if (schema is null)
		{
			return Enumerable.Empty<XmlElement>();
		}

		return schema.ChildNodes.OfType<XmlElement>()
			.Where(w => w.Name == "xs:element");
	}

	public static IEnumerable<XmlElement> GetChoice(XmlElement? element)
	{
		if (element is null)
		{
			return Enumerable.Empty<XmlElement>();
		}

		return element.ChildNodes.OfType<XmlElement>()
			.Where(w => w.Name == "xs:choice");
	}

	public static IEnumerable<XmlElement> GetSimpleContent(XmlElement? element)
	{
		if (element is null)
		{
			return Enumerable.Empty<XmlElement>();
		}

		return element.GetElementsByTagName("xs:simpleContent")
			.Cast<XmlElement>();
	}

	public static string GetChoiceProperty(XmlElement choice)
	{
		var elements = GetElements(choice).Select(s =>
		{
			var type = GetType(s);
			var name = GetName(s);
			
			return $"[XmlElement(\"{name}\", typeof({type}))]";
		});

		return $$"""
			{{String.Join("\n\t", elements)}}
				public object Item { get; set; }
			""";
	}
	
	public static string GetProperty(XmlElement element)
	{
		var name = GetName(element);
		var type = GetType(element);

		var result = String.Empty;
		var isFixed = TryGetFixedValue(element, type, out var fixedValue);

		if (TryGetMinOccurs(element, out _))
		{
			if (isFixed)
			{
				result = $"public {type}[]? {Titleize(name)} {{ get; }}";
			}
			else
			{
				result = $"public {type}[]? {Titleize(name)} {{ get; set; }}";
			}
		}
		else
		{
			if (isFixed)
			{
				result = $"public {type}? {Titleize(name)} {{ get; }}";
			}
			else
			{
				result = $"public {type}? {Titleize(name)} {{ get; set; }}";
			}
		}

		if (TryGetDefaultValue(element, type, out var defaultValue))
		{
			result = $"{result} = {defaultValue}";
		}
		
		if (isFixed)
		{
			result = $"{result} = {fixedValue}";
		}

		if (TryGetMinOccurs(element, out _))
		{
			return $"""
				[XmlArray("{name}")]
					{result}
				""";
		}

		return $"""
			[XmlElement(ElementName = "{name}")]
				{result}
			""";
	}
	
	public static string GetName(XmlElement element)
	{
		var name = String.Empty;
		
		do
		{
			name = element.GetAttribute("name");
			element = element.ParentNode as XmlElement;
		} while (String.IsNullOrEmpty(name));

		return name;
	}

	public static bool TryGetMinOccurs(XmlElement element, out int result)
	{
		return Int32.TryParse(element.GetAttribute("minOccurs"), out result);
	}
	
	public static bool TryGetDefaultValue(XmlElement element, string type, out string result)
	{
		var defaultText = element.GetAttribute("default");

		result = type switch
		{
			"string" => $"\"{defaultText}\"",
			"DateOnly" => $"DateOnly.Parse(\"{defaultText}\")",
			"DateTime" => $"DateTime.Parse(\"{defaultText}\")",
			"TimeOnly" => $"TimeOnly.Parse(\"{defaultText}\")",
			"TimeSpan" => $"TimeSpan.Parse(\"{defaultText}\")",
			_ => defaultText,
		};

		return !String.IsNullOrEmpty(defaultText);
	}
	
	public static bool TryGetFixedValue(XmlElement element, string type, out string result)
	{
		var fixedText = element.GetAttribute("fixed");

		result = type switch
		{
			"string" => $"\"{fixedText}\"",
			"DateOnly" => $"DateOnly.Parse(\"{fixedText}\")",
			"DateTime" => $"DateTime.Parse(\"{fixedText}\")",
			"TimeOnly" => $"TimeOnly.Parse(\"{fixedText}\")",
			"TimeSpan" => $"TimeSpan.Parse(\"{fixedText}\")",
			_ => fixedText,
		};

		return !String.IsNullOrEmpty(fixedText);
	}
	
	public static string GetType(XmlElement element)
	{
		var type = element.GetAttribute("type");
		var name = element.GetAttribute("name");

		if (String.IsNullOrEmpty(type))
		{
			return Titleize(name);
		}

		switch (type)
		{
			case "xs:string":
				return "string";
			case "xs:integer":
				return "int";
			case "xs:decimal":
				return "decimal";
			case "xs:boolean":
				return "bool";
			case "xs:date":
				return "DateOnly";
			case "xs:dateTime":
				return "DateTime";
			case "xs:time":
				return "TimeOnly";
			case "xs:duration":
				return "TimeSpan";
			case "xs:float":
				return "float";
			case "xs:double":
				return "double";
			case "xs:byte":
				return "byte";
			case "xs:unsignedByte":
				return "ubyte";
			case "xs:short":
				return "short";
			case "xs:unsignedShort":
				return "ushort";
			case "xs:int":
				return "int";
			case "xs:unsignedInt":
				return "uint";
			case "xs:long":
				return "long";
			case "xs:unsignedLong":
				return "ulong";
			case "xs:base64Binary":
			case "xs:hexBinary":
				return "byte[]";
			case "xs:anyURI":
				return "Uri";
			case "xs:QName":
				return "QName";
			case "xs:NOTATION":
				return "NOTATION";
			default:
			{
				var index = type.IndexOf(':');

				if (index > -1)
				{
					type = type.Substring(index + 1);
				}

				return Titleize(type);
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

	private static bool IsValidInIdentifier(this char c, bool firstChar = true)
	{
		switch (char.GetUnicodeCategory(c))
		{
			case UnicodeCategory.UppercaseLetter:
			case UnicodeCategory.LowercaseLetter:
			case UnicodeCategory.TitlecaseLetter:
			case UnicodeCategory.ModifierLetter:
			case UnicodeCategory.OtherLetter:
				// Always allowed in C# identifiers
				return true;

			case UnicodeCategory.LetterNumber:
			case UnicodeCategory.NonSpacingMark:
			case UnicodeCategory.SpacingCombiningMark:
			case UnicodeCategory.DecimalDigitNumber:
			case UnicodeCategory.ConnectorPunctuation:
			case UnicodeCategory.Format:
				// Only allowed after first char
				return !firstChar;
			default:
				return false;
		}
	}
}