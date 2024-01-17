using XSDGenerator.Model;

namespace XSDGenerator.XSDGenerator.Model;

public class XmlMinInclusiveFacet : XmlFacet
{
	public XmlMinInclusiveFacet()
	{
		FacetType = FacetType.MinInclusive;
	}
}