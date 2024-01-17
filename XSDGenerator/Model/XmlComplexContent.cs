using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlComplexContent : XmlContentModel
{
	[XmlAttribute("mixed")]
	public bool IsMixed { get; set; }

	[XmlElement("restriction", typeof(XmlComplexContentRestriction))]
	[XmlElement("extension", typeof(XmlComplexContentExtension))]
	public override XmlContent? Content { get; set; }
}