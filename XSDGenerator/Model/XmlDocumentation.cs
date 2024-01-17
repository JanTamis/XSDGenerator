using System.Xml;
using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlDocumentation : IElement
{
	[XmlAttribute("source", DataType = "anyURI")]
	public string? Source { get; set; }

	[XmlAttribute("xml:lang")]
	public string? Language { get; set; }

	[XmlText, XmlAnyElement]
	public XmlNode?[]? Markup { get; set; }
}