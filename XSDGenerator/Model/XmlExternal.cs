using System.Xml.Serialization;

namespace XSDGenerator.Model;

public class XmlExternal : IElement
{
	[XmlAttribute("schemaLocation", DataType = "anyURI")]
	public string? SchemaLocation { get; set; }

	[XmlAttribute("id", DataType = "ID")]
	public string? Id { get; set; }

	[XmlAnyAttribute]
	public IList<XmlAttribute>? UnhandledAttributes { get; set; }
}