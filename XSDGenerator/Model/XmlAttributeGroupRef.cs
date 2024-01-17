using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlAttributeGroupRef : XmlAnnotated
{
	[XmlAttribute("ref")]
	public XmlQualifiedName RefName { get; set; }
}