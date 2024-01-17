using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlKeyref : XmlIdentityConstraint
{
	[XmlAttribute("refer")]
	public XmlQualifiedName Refer { get; set; }
}