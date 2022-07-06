using Toolkit.MessageBroker;
using Benefit.Domain.Events;
using Benefit.Domain.Interfaces;
using Benefit.Service.Interfaces;
using Benefit.Domain.Benefit;

namespace Benefit.Service.Workers;

public sealed class BenefitLoadMoviesConsumer : BrokerConsumer<BenefitCreatedEvent>
{
    public BenefitLoadMoviesConsumer(IImdbApiClient apiClient, IBenefitRepository benefitRepository)
    {
        _ApiClient = apiClient;
        _BenefitRepository = benefitRepository;
    }

    private readonly IImdbApiClient _ApiClient;
    private readonly IBenefitRepository _BenefitRepository;

    protected override BrokerConsumerResult Consume(BenefitCreatedEvent message)
    {
        var benefit = _BenefitRepository.GetObjectByID(message.ID);
        var imdbPerson = _ApiClient.GetPerson(BenefitConsumerFactory.ImdbKey, benefit.Name).Result;
        if (imdbPerson == null || imdbPerson.results == null)
            return BrokerConsumerResult.Sucess;
        benefit.Works = imdbPerson.results
            .Select(o => new Work(o.title, o.image, o.description)).ToArray();
        _BenefitRepository.Update(benefit);
        return BrokerConsumerResult.Sucess;
    }
}