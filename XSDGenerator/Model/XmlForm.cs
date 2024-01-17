using System.Xml.Serialization;

namespace XSDGenerator.Model;

public enum XmlForm
{
	[XmlIgnore]
	None,
	
	[XmlEnum("qualified")]
	Qualified,
	
	[XmlEnum("unqualified")]
	Unqualified,
}