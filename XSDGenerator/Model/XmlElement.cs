using System.ComponentModel;
using System.Xml.Serialization;
using XSDGenerator.XSDGenerator.Model;

namespace XSDGenerator.Model;

public class XmlElement : XmlParticle
{
	[XmlAttribute("abstract"), DefaultValue(false)]
	public bool IsAbstract { get; set; }

	[XmlAttribute("block"), DefaultValue(XmlDerivationMethod.None)]
	public XmlDerivationMethod Block { get; set; }

	[XmlAttribute("default")]
	[DefaultValue(null)]
	public string? DefaultValue { get; set; }

	[XmlAttribute("final"), DefaultValue(XmlDerivationMethod.None)]
	public XmlDerivationMethod Final { get; set; }

	[XmlAttribute("fixed")]
	[DefaultValue(null)]
	public string? FixedValue { get; set; }

	[XmlAttribute("form"), DefaultValue(XmlForm.None)]
	public XmlForm Form { get; set; }

	[XmlAttribute("name"), DefaultValue("")]
	public string? Name { get; set; }

	[XmlAttribute("nillable"), DefaultValue(false)]
	public bool IsNillable { get; set; }

	[XmlAttribute("ref")]
	public XmlQualifiedName RefName { get; set; }

	[XmlAttribute("substitutionGroup")]
	public XmlQualifiedName SubstitutionGroup { get; set; }

	[XmlAttribute("type")]
	public XmlQualifiedName SchemaTypeName { get; set; }

	[XmlElement("complexType", typeof(XmlComplexType))]
	[XmlElement("simpleType", typeof(XmlSimpleType))]
	public XmlType? SchemaType { get; set; }

	[XmlElement("key", typeof(XmlKey))]
	[XmlElement("keyref", typeof(XmlKeyref))]
	[XmlElement("unique", typeof(XmlUnique))]
	public IList<XmlIdentityConstraint> Constraints { get; set; }
}