using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlInclude : XmlExternal
{
	[XmlElement("annotation", typeof(XmlAnnotation))]
	public XmlAnnotation? Annotation { get; set; }
}