using System.ComponentModel;
using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlType : XmlAnnotated
{
	[XmlAttribute("name")]
	public string? Name { get; set; }

	[XmlAttribute("final"), DefaultValue(XmlDerivationMethod.None)]
	public XmlDerivationMethod Final { get; set; }

	[XmlIgnore]
	public virtual bool IsMixed { get; set; }

	[XmlIgnore]
	public XmlQualifiedName QualifiedName { get; set; }

	[XmlIgnore]
	public XmlDerivationMethod FinalResolved { get; set; }
}