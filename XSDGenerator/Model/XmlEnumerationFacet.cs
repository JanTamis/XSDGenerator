using XSDGenerator.Model;

namespace XSDGenerator.XSDGenerator.Model;

public class XmlEnumerationFacet : XmlFacet
{
    public XmlEnumerationFacet()
    {
        FacetType = FacetType.Enumeration;
    }
}