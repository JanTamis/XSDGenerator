namespace XSDGenerator.Model;

public interface ITypeable
{
	string Default { get; set; }
	string Fixed { get; set; }
	string Name { get; set; }
	string Type { get; set; }
	string Ref { get; set; }
}