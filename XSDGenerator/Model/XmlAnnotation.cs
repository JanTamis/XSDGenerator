using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlAnnotation : IElement
{
	[XmlAttribute("id", DataType = "ID")]
	public string? Id { get; set; }

	[XmlElement("documentation", typeof(XmlDocumentation))]
	[XmlElement("appinfo", typeof(XmlAppInfo))]
	public IElement[] Items { get; set; }
}