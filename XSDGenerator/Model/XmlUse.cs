using System.Xml.Serialization;

namespace XSDGenerator.Model;

public enum XmlUse
{
	[XmlIgnore]
	None,

	[XmlEnum("optional")]
	Optional,

	[XmlEnum("prohibited")]
	Prohibited,

	[XmlEnum("required")]
	Required,
}