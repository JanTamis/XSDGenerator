using XSDGenerator.Model;

namespace XSDGenerator.XSDGenerator.Model;

public class XmlMaxInclusiveFacet : XmlFacet
{
    public XmlMaxInclusiveFacet()
    {
        FacetType = FacetType.MaxInclusive;
    }
}