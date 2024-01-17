using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlRedefine : XmlExternal
{
	[XmlElement("annotation", typeof(XmlAnnotation))]
	[XmlElement("attributeGroup", typeof(XmlAttributeGroup))]
	[XmlElement("complexType", typeof(XmlComplexType))]
	[XmlElement("group", typeof(XmlGroup))]
	[XmlElement("simpleType", typeof(XmlSimpleType))]
	public IElement[] Items { get; set; }
}