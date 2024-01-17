using System.ComponentModel;
using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlAttribute : XmlAnnotated
{
	[XmlAttribute("default")]
	[DefaultValue(null)]
	public string? DefaultValue { get; set; }

	[XmlAttribute("fixed")]
	[DefaultValue(null)]
	public string? FixedValue { get; set; }

	[XmlAttribute("form"), DefaultValue(XmlForm.None)]
	public XmlForm Form { get; set; }

	[XmlAttribute("name")]
	public string? Name { get; set; }

	[XmlAttribute("ref")]
	public XmlQualifiedName RefName { get; set; }

	[XmlAttribute("type")]
	public XmlQualifiedName SchemaTypeName { get; set; }

	[XmlElement("simpleType")]
	public XmlSimpleType? SchemaType { get; set; }

	[XmlAttribute("use"), DefaultValue(XmlUse.None)]
	public XmlUse Use { get; set; }
}