// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

var xml = Assembly.GetExecutingAssembly().GetManifestResourceStream("XSDGenerator.Test.verzoekbericht_4_0_0.xml");

var result = await Parse<verzoekXML>(xml);

Console.WriteLine();


async Task<T> Parse<T>(Stream xml)
{
	using var reader = new StreamReader(xml);
	
	var serializer = new XmlSerializer(typeof(T));
	var data = await reader.ReadToEndAsync();
	
	var element = RemoveAllNamespaces(XElement.Parse(data));

	return (T)serializer.Deserialize(element.CreateReader());

	XElement RemoveAllNamespaces(XElement e)
	{
		return new XElement(e.Name.LocalName, e.Nodes()
				.Select(n => n is XElement xElement 
					? RemoveAllNamespaces(xElement) 
					: n),
				e.HasAttributes 
					? e.Attributes()
						.Where(a => !a.IsNamespaceDeclaration)
						.Select(a => new XAttribute(a.Name.LocalName, a.Value)) 
					: null);
	}         
}