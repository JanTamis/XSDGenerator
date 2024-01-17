using System.Xml.Schema;
using System.Xml.Serialization;
using XSDGenerator.XSDGenerator.Model;

namespace XSDGenerator.Model;

public class XmlSimpleType : XmlType
{
	[XmlElement("restriction", typeof(XmlSimpleTypeRestriction))]
	[XmlElement("list", typeof(XmlSimpleTypeList))]
	[XmlElement("union", typeof(XmlSchemaSimpleTypeUnion))]
	public XmlSchemaSimpleTypeContent? Content { get; set; }
}
