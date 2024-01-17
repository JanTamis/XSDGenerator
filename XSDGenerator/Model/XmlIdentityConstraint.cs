using System.Xml.Serialization;
using XSDGenerator.XSDGenerator.Model;

namespace XSDGenerator.Model;

public class XmlIdentityConstraint : XmlAnnotated
{
	[XmlAttribute("name")]
	public string? Name { get; set; }

	[XmlElement("selector", typeof(XmlXPath))]
	public XmlXPath? Selector { get; set; }

	[XmlElement("field", typeof(XmlXPath))]
	public XmlXPath[] Fields { get; set; }
}