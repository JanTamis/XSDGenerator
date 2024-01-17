using System.Xml.Serialization;

namespace XSDGenerator.Model;

public enum XmlContentProcessing
{
	[XmlIgnore]
	None,

	[XmlEnum("skip")]
	Skip,

	[XmlEnum("lax")]
	Lax,

	[XmlEnum("strict")]
	Strict
}