using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlParticle : XmlAnnotated
{
	[XmlAttribute("minOccurs")]
	public uint? MinOccurs { get; set; }

	[XmlAttribute("maxOccurs")]
	public uint? MaxOccurs { get; set; }
}