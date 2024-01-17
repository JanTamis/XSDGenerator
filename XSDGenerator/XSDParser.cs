using System.Xml;
using XSDGenerator.Model;

namespace XSDGenerator;

public static class XSDParser
{
	public static IElement Convert(XmlElement element)
	{
		return element?.Name switch
		{
			"xs:sequence" => GetSequence(element),
			"xs:annotation" => GetAnnotation(element),
			"xs:element" => GetElement(element),
			_ => throw new Exception("Invalid element")
		};
	}
	
	private static Sequence GetSequence(XmlElement element)
	{
		var sequence = new Sequence
		{
			Id = element.GetAttribute("id"),
			MaxOccurs = UInt32.TryParse(element.GetAttribute("maxOccurs"), out var maxOccurs) ? maxOccurs : 1,
			MinOccurs = UInt32.TryParse(element.GetAttribute("minOccurs"), out var minOccurs) ? minOccurs : 1,
			Elements = new List<IElement>(),
		};

		foreach (XmlElement xmlElement in element.ChildNodes)
		{
			if (xmlElement.Name == "xs:annotation")
			{
				sequence.Annotation = GetAnnotation(xmlElement);
			}
			else
			{
				if (xmlElement.Name is not "xs:any" and not "xs:element" and not "xs:group" and not "xs:choice" and not "xs:sequence")
				{
					throw new Exception("Invalid sequence element");
				}
				
				sequence.Elements.Add(Convert(xmlElement));
			}
		}
		
		return sequence;
	}
	
	private static Annotation GetAnnotation(XmlElement element)
	{
		var annotation = new Annotation
		{
			Id = element.GetAttribute("id"),
			Elements = new List<IElement>(),
		};

		foreach (XmlElement xmlElement in element.ChildNodes)
		{
			if (xmlElement.Name is not "xs:appinfo" and not "xs:documentation")
			{
				throw new Exception("Invalid annotation element");
			}
			
			annotation.Elements.Add(Convert(xmlElement));
		}
		

		return annotation;
	}

	private static Element GetElement(XmlElement element)
	{
		var result = new Element
		{
			Abstract = Boolean.TryParse(element.GetAttribute("abstract"), out var @abstract) ? @abstract : null,
			Nullable = Boolean.TryParse(element.GetAttribute("nillable"), out var nillable) ? nillable : null,
			Ref = element.GetAttribute("ref"),
			SubstitutionGroup = element.GetAttribute("substitutionGroup"),
			MaxOccurs = UInt32.TryParse(element.GetAttribute("maxOccurs"), out var maxOccurs) ? maxOccurs : 1,
			MinOccurs = UInt32.TryParse(element.GetAttribute("minOccurs"), out var minOccurs) ? minOccurs : 1,
			Id = element.GetAttribute("id"),
			Name = element.GetAttribute("name"),
			Default = element.GetAttribute("default"),
			Fixed = element.GetAttribute("fixed"),
			Type = element.GetAttribute("type"),
		};
		
		foreach (XmlElement xmlElement in element.ChildNodes)
		{
			switch (xmlElement.Name)
			{
				case "xs:annotation":
					result.Annotation = GetAnnotation(xmlElement);
					break;
				case "xs:complexType":
					result.ComplexOrSimpleType = GetComplexType(xmlElement);
					break;
				case "xs:simpleType":
					result.ComplexOrSimpleType = GetSimpleType(xmlElement);
					break;
				default:
					throw new Exception("Invalid element");
			}
		}

		return result;
	}
	
	private static SimpleType GetSimpleType(XmlElement element)
	{
		var result = new SimpleType
		{
			Id = element.GetAttribute("id"),
			Name = element.GetAttribute("name"),
		};

		foreach (XmlElement xmlElement in element.ChildNodes)
		{
			switch (xmlElement.Name)
			{
				case "xs:annotation":
					result.Annotation = GetAnnotation(xmlElement);
					break;
				// case "xs:restriction":
				// 	result.Content = GetRestriction(xmlElement);
				// 	break;
				default:
					throw new Exception("Invalid element");
			}
		}

		return result;
	}
}