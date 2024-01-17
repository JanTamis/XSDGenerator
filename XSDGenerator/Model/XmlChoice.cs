using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlChoice : XmlParticle
{
	[XmlElement("element", typeof(XmlElement))]
	[XmlElement("group", typeof(XmlGroupRef))]
	[XmlElement("choice", typeof(XmlChoice))]
	[XmlElement("sequence", typeof(XmlSequence))]
	[XmlElement("any", typeof(XmlAny))]
	public XmlParticle[] Items { get; set; }
}