using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Xml.Schema;
using System.IO;
using System.Linq;

namespace XSDGenerator;

[Generator]
public class Generator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var files = context.AdditionalTextsProvider
			.Where(x => String.Equals(Path.GetExtension(x.Path), ".xsd"))
			.Select((s, token) => s.GetText(token).ToString());

		var compilationAndFiles = context.CompilationProvider.Combine(files.Collect());

		context.RegisterSourceOutput(compilationAndFiles, Generate);
	}

	void Generate(SourceProductionContext context, (Compilation compilation, ImmutableArray<string> files) compilationAndFiles)
	{
		var set = new XmlSchemaSet();

		foreach (var file in compilationAndFiles.files)
		{
			set.Add(XmlSchema.Read(new StringReader(file), null));
		}

		foreach (XmlSchema schema in set.Schemas())
		{

		}

		set.Compile();
	}
}
