using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlNotation : XmlAnnotated
{
	[XmlAttribute("name")]
	public string? Name { get; set; }

	[XmlAttribute("public")]
	public string? Public { get; set; }

	[XmlAttribute("system")]
	public string? System { get; set; }
}