using System.Xml.Serialization;

namespace XSDGenerator.Model;

public abstract class XmlContentModel : XmlAnnotated
{
	[XmlIgnore]
	public abstract XmlContent? Content { get; set; }
}