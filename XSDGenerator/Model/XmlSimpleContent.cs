using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlSimpleContent : XmlContentModel
{
	[XmlElement("restriction", typeof(XmlSimpleContentRestriction))]
	[XmlElement("extension", typeof(XmlSimpleContentExtension))]
	public override XmlContent? Content { get; set; }
}