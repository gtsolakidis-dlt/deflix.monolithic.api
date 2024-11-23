using deflix.monolithic.api.DTOs;
using deflix.monolithic.api.Interfaces;

namespace deflix.monolithic.api.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMovieService _movieService;
        private readonly IUserPreferencesService _userPreferencesService;

        public RecommendationService(IDatabaseService databaseService, IMovieService movieService, IUserPreferencesService userPreferencesService)
        {
            _databaseService = databaseService;
            _movieService = movieService;
            _userPreferencesService = userPreferencesService;
        }

        public IEnumerable<MovieDto> GetRecommendationsForUser(Guid userId)
        {
            var userPreferences = _userPreferencesService.GetFavoritesForUser(userId)?.ToList();
            var movies = _movieService.GetAllMovies(userId)?.ToList();
            
            if (!(userPreferences?.Count > 0)) return movies;
            
            var userMovies = userPreferences.Select(u => u.MovieId).ToList();
            var genre = userPreferences.Select(u => u.GenreId).ToList();
            var selectMovies = movies?.Where(m => !userMovies.Contains(m.MovieId) && genre.Contains(m.GenreId)).ToList();

            return selectMovies;
        }
    }

}
