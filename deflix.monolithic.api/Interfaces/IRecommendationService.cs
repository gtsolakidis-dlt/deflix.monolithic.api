namespace deflix.monolithic.api.Interfaces
{
    using DTOs;
    using System.Collections.Generic;

    public interface IRecommendationService
    {
        IEnumerable<MovieDto> GetRecommendationsForUser(Guid userId);
    }

}
