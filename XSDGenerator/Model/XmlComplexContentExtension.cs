using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlComplexContentExtension : XmlContent
{
	[XmlAttribute("base")]
	public XmlQualifiedName BaseTypeName { get; set; }

	[XmlElement("group", typeof(XmlGroupRef))]
	[XmlElement("choice", typeof(XmlChoice))]
	[XmlElement("all", typeof(XmlAll))]
	[XmlElement("sequence", typeof(XmlSequence))]
	public XmlParticle? Particle { get; set; }

	[XmlElement("attribute", typeof(XmlAttribute))]
	[XmlElement("attributeGroup", typeof(XmlAttributeGroupRef))]
	public IList<XmlAnnotated> Attributes { get; set; }

	[XmlElement("anyAttribute")]
	public XmlAnyAttribute? AnyAttribute { get; set; }
}