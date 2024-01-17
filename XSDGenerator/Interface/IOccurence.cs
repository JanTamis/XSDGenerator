namespace XSDGenerator.Model;

public interface IOccurence
{
	uint MaxOccurs { get; set; }

	uint MinOccurs { get; set; }
}