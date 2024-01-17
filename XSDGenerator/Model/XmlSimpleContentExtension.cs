using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlSimpleContentExtension : XmlContent
{
	[XmlAttribute("base")]
	public XmlQualifiedName BaseTypeName { get; set; }

	[XmlElement("attribute", typeof(XmlAttribute))]
	[XmlElement("attributeGroup", typeof(XmlAttributeGroupRef))]
	public IList<XmlAnnotated> Attributes { get; set; }

	[XmlElement("anyAttribute")]
	public XmlAnyAttribute? AnyAttribute { get; set; }
}