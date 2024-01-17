using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlAll : XmlParticle
{
	[XmlElement("element", typeof(XmlElement))]
	public IList<XmlElement> Items { get; set; }
}