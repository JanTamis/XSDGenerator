using System.ComponentModel;
using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlAnyAttribute : XmlAnnotated
{
	[XmlAttribute("namespace")]
	public string? Namespace { get; set; }

	[XmlAttribute("processContents"), DefaultValue(XmlContentProcessing.None)]
	public XmlContentProcessing ProcessContents { get; set; }
}