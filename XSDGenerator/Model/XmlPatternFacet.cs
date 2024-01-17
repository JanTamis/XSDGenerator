using XSDGenerator.Model;

namespace XSDGenerator.XSDGenerator.Model;

public class XmlPatternFacet : XmlFacet
{
    public XmlPatternFacet()
    {
        FacetType = FacetType.Pattern;
    }
}