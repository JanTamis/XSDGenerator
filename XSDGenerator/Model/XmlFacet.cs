using System.ComponentModel;
using System.Xml.Serialization;

namespace XSDGenerator.Model;

public abstract class XmlFacet : XmlAnnotated
{
	[XmlAttribute("value")]
	public string? Value { get; set; }
	
	[XmlAttribute("fixed"), DefaultValue(false)]
	public virtual bool IsFixed { get; set; }
}