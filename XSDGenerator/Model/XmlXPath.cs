using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using XSDGenerator.Model;

namespace XSDGenerator.XSDGenerator.Model;

public class XmlXPath : XmlAnnotated
{
    [XmlAttribute("xpath"), DefaultValue("")]
    public string? XPath { get; set; }
}