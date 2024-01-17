using XSDGenerator.Model;

namespace XSDGenerator.XSDGenerator.Model;

public class XmlMaxExclusiveFacet : XmlFacet
{
	public XmlMaxExclusiveFacet()
	{
		FacetType = FacetType.MaxExclusive;
	}
}