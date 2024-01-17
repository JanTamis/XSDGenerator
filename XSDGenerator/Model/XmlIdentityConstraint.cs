using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlIdentityConstraint
{
	[XmlAttribute("name")]
	public string? Name { get; set; }

	[XmlElement("selector", typeof(XmlXPath))]
	public XmlXPath? Selector { get; set; }

	[XmlElement("field", typeof(XmlXPath))]
	public IList<XmlXPath> Fields { get; set; }
}