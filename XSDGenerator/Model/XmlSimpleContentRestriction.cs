using System.Xml.Serialization;
using XSDGenerator.XSDGenerator.Model;

namespace XSDGenerator.Model;

public class XmlSimpleContentRestriction : XmlContent
{
	[XmlAttribute("base")]
	public XmlQualifiedName BaseTypeName { get; set; }

	[XmlElement("simpleType", typeof(XmlSimpleType))]
	public XmlSimpleType? BaseType { get; set; }

	[XmlElement("length", typeof(XmlLengthFacet))]
	[XmlElement("minLength", typeof(XmlMinLengthFacet))]
	[XmlElement("maxLength", typeof(XmlMaxLengthFacet))]
	[XmlElement("pattern", typeof(XmlPatternFacet))]
	[XmlElement("enumeration", typeof(XmlEnumerationFacet))]
	[XmlElement("maxInclusive", typeof(XmlMaxInclusiveFacet))]
	[XmlElement("maxExclusive", typeof(XmlMaxExclusiveFacet))]
	[XmlElement("minInclusive", typeof(XmlMinInclusiveFacet))]
	[XmlElement("minExclusive", typeof(XmlMinExclusiveFacet))]
	[XmlElement("totalDigits", typeof(XmlTotalDigitsFacet))]
	[XmlElement("fractionDigits", typeof(XmlFractionDigitsFacet))]
	[XmlElement("whiteSpace", typeof(XmlWhiteSpaceFacet))]
	public IList<XmlFacet> Facets { get; set; }

	[XmlElement("attribute", typeof(XmlAttribute))]
	[XmlElement("attributeGroup", typeof(XmlAttributeGroupRef))]
	public IList<XmlAnnotated> Attributes { get; }

	[XmlElement("anyAttribute")]
	public XmlAnyAttribute? AnyAttribute { get; set; }
}