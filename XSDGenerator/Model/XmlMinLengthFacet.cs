namespace XSDGenerator.XSDGenerator.Model;

public class XmlMinLengthFacet : XmlNumericFacet
{
	public XmlMinLengthFacet()
	{
		FacetType = FacetType.MinLength;
	}
}