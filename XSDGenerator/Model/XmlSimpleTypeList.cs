using System.Xml.Serialization;
using XSDGenerator.Model;

namespace XSDGenerator.XSDGenerator.Model;

public class XmlSimpleTypeList
{
    [XmlAttribute("itemType")]
    public XmlQualifiedName ItemTypeName { get; set; }

    [XmlElement("simpleType", typeof(XmlSimpleType))]
    public XmlSimpleType? ItemType { get; set; }
}