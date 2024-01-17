using System.Xml.Serialization;
using XSDGenerator.XSDGenerator.Model;

namespace XSDGenerator.Model;

public class XmlSimpleTypeRestriction : XmlSimpleTypeContent
{
	[XmlAttribute("base")]
	public XmlQualifiedName BaseTypeName { get; set; }

	[XmlElement("simpleType", typeof(XmlSimpleType))]
	public XmlSimpleType? BaseType { get; set; }

	[XmlElement("length", typeof(XmlLengthFacet)),
	 XmlElement("minLength", typeof(XmlMinLengthFacet)),
	 XmlElement("maxLength", typeof(XmlMaxLengthFacet)),
	 XmlElement("pattern", typeof(XmlPatternFacet)),
	 XmlElement("enumeration", typeof(XmlEnumerationFacet)),
	 XmlElement("maxInclusive", typeof(XmlMaxInclusiveFacet)),
	 XmlElement("maxExclusive", typeof(XmlMaxExclusiveFacet)),
	 XmlElement("minInclusive", typeof(XmlMinInclusiveFacet)),
	 XmlElement("minExclusive", typeof(XmlMinExclusiveFacet)),
	 XmlElement("totalDigits", typeof(XmlTotalDigitsFacet)),
	 XmlElement("fractionDigits", typeof(XmlFractionDigitsFacet)),
	 XmlElement("whiteSpace", typeof(XmlWhiteSpaceFacet))]
	public XmlFacet[] Facets { get; set; }
}