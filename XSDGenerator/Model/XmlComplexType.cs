using System.ComponentModel;
using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlComplexType : XmlType
{
	[XmlAttribute("abstract"), DefaultValue(false)]
	public bool IsAbstract { get; set; }

	[XmlAttribute("block"), DefaultValue(XmlDerivationMethod.None)]
	public XmlDerivationMethod Block { get; set; }

	[XmlAttribute("mixed"), DefaultValue(false)]
	public override bool IsMixed { get; set; }

	[XmlElement("simpleContent", typeof(XmlSimpleContent))]
	[XmlElement("complexContent", typeof(XmlComplexContent))]
	public XmlContentModel? ContentModel { get; set; }

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