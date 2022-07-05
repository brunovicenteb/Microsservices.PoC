using Benefit.Domain.Benefit;

namespace Benefit.Domain.Events;

public class BenefitInsertedEvent
{
    public uint? ParentID { get; set; }
    public OperatorType Operator { get; set; }
    public string Name { get; set; }
    public string CPF { get; set; }
    public DateTime? BirthDate { get; set; }
}