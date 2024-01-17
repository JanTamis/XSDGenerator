namespace XSDGenerator.XSDGenerator.Model;

public class XmlMaxLengthFacet : XmlNumericFacet
{
	public XmlMaxLengthFacet()
	{
		FacetType = FacetType.MaxLength;
	}
}