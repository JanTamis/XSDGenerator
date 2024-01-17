using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlAnnotated : IElement
{
	[XmlAttribute("id", DataType = "ID")]
	public string Id { get; set; }

	[XmlElement("annotation", typeof(XmlAnnotation))]
	public XmlAnnotation? Annotation { get; set; }
}