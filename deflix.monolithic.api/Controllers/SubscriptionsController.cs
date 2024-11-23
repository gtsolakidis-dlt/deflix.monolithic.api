using deflix.monolithic.api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace deflix.monolithic.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ApiController
    {
        private readonly ISubscriptionsService _subscriptionsService;

        public SubscriptionsController(ISubscriptionsService subscriptionsService)
        {
            _subscriptionsService = subscriptionsService;
        }

        [HttpGet("list")]
        public IActionResult GetAllSubscriptions()
        {
            var subscriptions = _subscriptionsService.GetAllSubscriptions();
            return Ok(subscriptions);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetUserSubscription(Guid userId)
        {
            var subscription = _subscriptionsService.GetUserSubscription(userId);
            if (subscription == null)
            {
                return NotFound(new { message = "No subscription found for this user." });
            }

            return Ok(subscription);
        }

        [HttpPut("user/{userId}/subscribe/{subscriptionCode}")]
        public IActionResult SubscribeUser(Guid userId, int subscriptionCode)
        {
            _subscriptionsService.SubscribeUser(userId, subscriptionCode);
            return Ok(new { message = "User subscribed successfully." });
        }
    }
}
