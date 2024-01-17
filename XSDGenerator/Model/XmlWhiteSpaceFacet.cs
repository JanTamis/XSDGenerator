using XSDGenerator.Model;

namespace XSDGenerator.XSDGenerator.Model;

public class XmlWhiteSpaceFacet : XmlFacet
{
	public XmlWhiteSpaceFacet()
	{
		FacetType = FacetType.Whitespace;
	}
}