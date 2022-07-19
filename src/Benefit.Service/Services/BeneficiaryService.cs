using MassTransit;
using Toolkit.Mapper;
using Toolkit.Exceptions;
using Toolkit.Interfaces;
using Benefit.Domain.Benefit;
using Benefit.Domain.Operator;
using Benefit.Domain.Interfaces;
using Benefit.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Benefit.Service.Sagas.Beneficiary.Contract;

namespace Benefit.Service.Services;

public class BeneficiaryService : IBeneficiaryService
{
    public BeneficiaryService(IPublishEndpoint publiser, IBenefitRepository benefitRepository)
    {
        _Publisher = publiser;
        _BenefitRepository = benefitRepository;
        _Mapper = MapperFactory.Map<Beneficiary, BeneficiarySubmitted>();
    }

    private readonly IPublishEndpoint _Publisher;
    private readonly IBenefitRepository _BenefitRepository;
    private readonly IGenericMapper _Mapper;

    public async Task<Beneficiary> SubmitBeneficiary(OperatorType operatorType, string name, string cpf, DateTime? birthDate)
    {
        try
        {
            var op = Operator.CreateOperator(operatorType);
            var entity = op.CreateBeneficiary(name, cpf, birthDate);
            await _BenefitRepository.AddAsync(entity, false);

            var evt = _Mapper.Map<Beneficiary, BeneficiarySubmitted>(entity);
            evt.CorrelationId = NewId.NextGuid();
            await _Publisher.Publish(evt);
            await _BenefitRepository.SaveChangesAsync();

            return entity;
        }
        catch (DbUpdateException exception)
        {
            throw new DuplicateRegistrationException("Duplicate registration", exception);
        }
    }
}