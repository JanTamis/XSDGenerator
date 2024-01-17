using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlGroupRef : XmlParticle
{
	[XmlAttribute("ref")]
	public XmlQualifiedName RefName { get; set; }
}