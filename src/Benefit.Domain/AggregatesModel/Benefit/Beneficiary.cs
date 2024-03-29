﻿using Toolkit;
using System.Text;
using Toolkit.Data;

namespace Benefit.Domain.Benefit;
public class Beneficiary : TimeCycleEntity
{
    public Beneficiary()
        : base()
    {
    }

    public Beneficiary(int id, OperatorType operatorType, string name, string cpf, DateTime? birthDate,
        DateTime createAt, DateTime? updateAt, DateTime? deletedAt)
        : base(id, createAt, updateAt, deletedAt)
    {
        Operator = operatorType;
        Name = name;
        CPF = cpf;
        BirthDate = birthDate;
    }

    public OperatorType Operator { get; set; }
    public string Name { get; set; }
    public string CPF { get; set; }
    public DateTime? BirthDate { get; set; }
    public ICollection<ImdbWork> ImdbWorks { get; set; }
    public ICollection<TheAudioDbWork> TheAudioDbWorks { get; set; }

    public bool IsUnderAge
    {
        get
        {
            if (BirthDate == null)
                return false;
            return BirthDate.Value.AddYears(18) > DateTime.Now;
        }
    }

    public void Validate(StringBuilder errors)
    {
        if (Name.IsEmpty())
            errors.AppendLine("The field \"Name\" cannot be empty.");
        if (CPF.IsFilled() && !CPF.IsValidCPF())
            errors.AppendLine("The field \"CPF\" is not valid.");
        if (BirthDate.HasValue && BirthDate.Value > DateTime.Now)
            errors.AppendLine("The field \"BirthDate\" cannot be in the future.");
    }
}