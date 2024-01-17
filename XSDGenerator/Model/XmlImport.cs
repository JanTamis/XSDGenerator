using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlImport : XmlExternal
{
	[XmlAttribute("namespace", DataType = "anyURI")]
	public string? Namespace { get; set; }

	[XmlElement("annotation", typeof(XmlAnnotation))]
	public XmlAnnotation? Annotation { get; set; }
}