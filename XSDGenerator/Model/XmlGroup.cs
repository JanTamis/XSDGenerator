using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlGroup : XmlAnnotated
{
	[XmlAttribute("name")]
	public string? Name { get; set; }

	[XmlElement("choice", typeof(XmlChoice))]
	[XmlElement("all", typeof(XmlAll))]
	[XmlElement("sequence", typeof(XmlSequence))]
	public IList<XmlParticle>? Particle { get; set; }
}