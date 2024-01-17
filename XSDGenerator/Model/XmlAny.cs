using System.ComponentModel;
using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlAny : XmlParticle
{
	[XmlAttribute("namespace")]
	public string? Namespace { get; set; }

	[XmlAttribute("processContents"), DefaultValue(XmlContentProcessing.None)]
	public XmlContentProcessing ProcessContents { get; set; }
}