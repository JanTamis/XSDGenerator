using XSDGenerator.Model;

namespace XSDGenerator.XSDGenerator.Model;

public class XmlMinExclusiveFacet : XmlFacet
{
    public XmlMinExclusiveFacet()
    {
        FacetType = FacetType.MinExclusive;
    }
}