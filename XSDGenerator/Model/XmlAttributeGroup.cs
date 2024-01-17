using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlAttributeGroup : XmlAnnotated
{
	[XmlAttribute("name")]
	public string? Name { get; set; }

	[XmlElement("attribute", typeof(XmlAttribute))]
	[XmlElement("attributeGroup", typeof(XmlAttributeGroupRef))]
	public IList<XmlAnnotated> Attributes { get; set; } 

	[XmlElement("anyAttribute")]
	public XmlAnyAttribute? AnyAttribute { get; set; }
}