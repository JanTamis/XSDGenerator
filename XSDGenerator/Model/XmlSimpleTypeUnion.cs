using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlSimpleTypeUnion : XmlSimpleTypeContent
{
	[XmlElement("simpleType", typeof(XmlSimpleType))]
	public IList<XmlSimpleType> BaseTypes { get; }
}
