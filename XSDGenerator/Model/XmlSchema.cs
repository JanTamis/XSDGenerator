using System.ComponentModel;
using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlSchema : IElement
{
	[XmlAttribute("attributeFormDefault"), DefaultValue(XmlForm.None)]
	public XmlForm AttributeFormDefault { get; set; }

	[XmlAttribute("blockDefault"), DefaultValue(XmlDerivationMethod.None)]
	public XmlDerivationMethod BlockDefault { get; set; }

	[XmlAttribute("finalDefault"), DefaultValue(XmlDerivationMethod.None)]
	public XmlDerivationMethod FinalDefault { get; set; }

	[XmlAttribute("elementFormDefault"), DefaultValue(XmlForm.None)]
	public XmlForm ElementFormDefault { get; set; }

	[XmlAttribute("targetNamespace", DataType = "anyURI")]
	public string? TargetNamespace { get; set; }

	[XmlAttribute("version", DataType = "token")]
	public string? Version { get; set; }

	[XmlElement("include", typeof(XmlInclude))]
	[XmlElement("import", typeof(XmlImport))]
	[XmlElement("redefine", typeof(XmlRedefine))]
	public XmlExternal[] Includes { get; set; }

	[XmlElement("annotation", typeof(XmlAnnotation))]
	[XmlElement("attribute", typeof(XmlAttribute))]
	[XmlElement("attributeGroup", typeof(XmlAttributeGroup))]
	[XmlElement("complexType", typeof(XmlComplexType))]
	[XmlElement("simpleType", typeof(XmlSimpleType))]
	[XmlElement("element", typeof(XmlElement))]
	[XmlElement("group", typeof(XmlGroup))]
	[XmlElement("notation", typeof(XmlNotation))]
	public IElement[] Items { get; set; }

	[XmlAttribute("id", DataType = "ID")]
	public string? Id { get; set; }

	[XmlAnyAttribute]
	public XmlAttribute[]? UnhandledAttributes { get; set; }
}