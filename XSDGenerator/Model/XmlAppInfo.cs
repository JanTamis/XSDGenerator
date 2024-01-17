using System.Xml;
using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlAppInfo : IElement
{
	[XmlAttribute("source", DataType = "anyURI")]
	public string? Source { get; set; }

	[XmlText, XmlAnyElement]
	public XmlNode?[]? Markup { get; set; }
}