using System.Xml.Serialization;

namespace XSDGenerator.Model;

[Flags]
public enum XmlDerivationMethod
{
	[XmlEnum("")]
	Empty = 0,

	[XmlEnum("substitution")]
	Substitution = 0x0001,

	[XmlEnum("extension")]
	Extension = 0x0002,

	[XmlEnum("restriction")]
	Restriction = 0x0004,

	[XmlEnum("list")]
	List = 0x0008,

	[XmlEnum("union")]
	Union = 0x0010,

	[XmlEnum("#all")]
	All = 0x00FF,

	[XmlIgnore]
	None = 0x0100
}