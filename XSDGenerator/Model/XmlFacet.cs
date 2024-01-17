using System.ComponentModel;
using System.Xml.Serialization;
using XSDGenerator.XSDGenerator.Model;

namespace XSDGenerator.Model;

public abstract class XmlFacet : XmlAnnotated
{
	[XmlAttribute("value")]
	public string? Value { get; set; }
	
	[XmlAttribute("fixed"), DefaultValue(false)]
	public virtual bool IsFixed { get; set; }

	[XmlIgnore]
	public FacetType FacetType { get; set; }
}