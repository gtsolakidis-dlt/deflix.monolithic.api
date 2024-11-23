using deflix.monolithic.api.DTOs;

namespace deflix.monolithic.api.Interfaces
{
    public interface ISubscriptionsService
    {
        IEnumerable<SubscriptionDto> GetAllSubscriptions();
        UserSubscriptionDto GetUserSubscription(Guid userId);
        bool SubscribeUser(Guid userId, int subscriptionId);
    }

}
