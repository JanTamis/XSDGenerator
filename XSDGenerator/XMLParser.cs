using System.Xml.Schema;
using System.Xml.Serialization;

namespace XSDGenerator;

public static class XmlParser
{
	/// <summary>
	/// Parses the provided XML string into an XmlDocument.
	/// </summary>
	/// <param name="xml">The XML string to parse.</param>
	/// <returns>An XmlDocument that represents the provided XML string.</returns>
	public static XmlSchema? Parse(string xml)
	{
		var serializer = new XmlSerializer(typeof(System.Xml.Schema.XmlSchema));

		return serializer.Deserialize(new StringReader(xml)) as XmlSchema;
	}
}