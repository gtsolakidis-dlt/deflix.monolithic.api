using System.ComponentModel.Design;
using deflix.monolithic.api.DTOs;
using deflix.monolithic.api.Interfaces;
using deflix.monolithic.api.Models;

namespace deflix.monolithic.api.Services
{
    public class MovieService : IMovieService
    {
        private readonly IDatabaseService _databaseService;

        #region SqlQueries

        private static string GetAllMoviesQuery => "SELECT * FROM Movies WHERE PlanType <= @PlanType";
        private static string GetMovieByIdQuery => "SELECT * FROM Movies WHERE MovieId = @MovieId AND PlanType <= @PlanType";

        private static string GetMovieByIdsQuery => "SELECT * FROM Movies WHERE MovieId in @MovieIds AND PlanType <= @PlanType";

        private static string GetMoviesByGenreQuery => "SELECT * FROM Movies WHERE GenreId = @GenreId AND PlanType <= @PlanType";

        private static string GetAllGenreQuery => "SELECT * FROM Genre";
        private static string GetGenreByIdQuery => "SELECT * FROM Genre WHERE GenreId = @GenreId";

        private static string InsertMovieQuery =>
            @"INSERT INTO Movies (MovieId, Title, Description, Poster, Backdrop, Logo, GenreId, YoutubeKey, CriticsRating, PlanType)
              VALUES (@MovieId, @Title, @Description, @Poster, @Backdrop, @Logo, @GenreId, @YoutubeKey, @CriticsRating, @PlanType)";

        #endregion

        public MovieService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<MovieDto> GetAllMovies(Guid userId)
        {

            var userSubscription = _databaseService.QueryFirst<UserSubscriptionModel>(SubscriptionsService.GetUserSubscriptionQuery, new { userId });
            var moviesDbList = _databaseService.Query<MovieModel>(GetAllMoviesQuery, new { PlanType = userSubscription.SubscriptionCode });
            if (moviesDbList == null) return null;
            var movies = new List<MovieDto>();

            var subscriptions = _databaseService.Query<SubscriptionModel>(SubscriptionsService.GetSubscriptionsQuery);
            var genreList = _databaseService.Query<GenreModel>(GetAllGenreQuery);

            moviesDbList?.ForEach(m =>
            {
                var movie = ToMovieDto(m);
                movie.PlanType = GetPlanType(subscriptions, m.PlanType);
                movie.Genre = genreList.FirstOrDefault(g => g.GenreId == m.GenreId)?.Name;
                movies.Add(movie);
            });

            return movies;
        }

        public MovieDto GetMovieById(Guid userId, Guid movieId)
        {
            var userSubscription = _databaseService.QueryFirst<UserSubscriptionModel>(SubscriptionsService.GetUserSubscriptionQuery, new { userId });
            var movieDb = _databaseService.QueryFirst<MovieModel>(GetMovieByIdQuery, new { MovieId = movieId, PlanType = userSubscription.SubscriptionCode });
            if (movieDb == null) return null;

            var subscriptions = _databaseService.Query<SubscriptionModel>(SubscriptionsService.GetSubscriptionsQuery);
            var genre = _databaseService.QueryFirst<GenreModel>(GetGenreByIdQuery, new { movieDb.GenreId });
            var movie = ToMovieDto(movieDb);
            movie.PlanType = GetPlanType(subscriptions, movieDb.PlanType);
            movie.Genre = genre?.Name;

            return movie;
        }

        public IEnumerable<MovieDto> GetMovieByIds(Guid userId, List<Guid> movieIds)
        {
            var userSubscription = _databaseService.QueryFirst<UserSubscriptionModel>(SubscriptionsService.GetUserSubscriptionQuery, new { userId });
            var moviesDbList = _databaseService.Query<MovieModel>(GetMovieByIdsQuery, new { MovieIds = movieIds, PlanType = userSubscription.SubscriptionCode });
            if (moviesDbList == null) return null;
            var movies = new List<MovieDto>();

            var subscriptions = _databaseService.Query<SubscriptionModel>(SubscriptionsService.GetSubscriptionsQuery);
            var genreList = _databaseService.Query<GenreModel>(GetAllGenreQuery);
            moviesDbList?.ForEach(m =>
            {
                var movie = ToMovieDto(m);
                movie.PlanType = GetPlanType(subscriptions, m.PlanType);
                movie.Genre = genreList.FirstOrDefault(g => g.GenreId == m.GenreId)?.Name;
                movies.Add(movie);
            });

            return movies;
        }

        public IEnumerable<MovieDto> GetMoviesByGenre(Guid userId, Guid genreId)
        {
            var userSubscription = _databaseService.QueryFirst<UserSubscriptionModel>(SubscriptionsService.GetUserSubscriptionQuery, new { userId });
            var moviesDbList = _databaseService.Query<MovieModel>(GetMoviesByGenreQuery, new { GenreId = genreId, PlanType = userSubscription.SubscriptionCode });
            if (moviesDbList == null) return null;

            var subscriptions = _databaseService.Query<SubscriptionModel>(SubscriptionsService.GetSubscriptionsQuery);
            var genre = _databaseService.QueryFirst<GenreModel>(GetGenreByIdQuery, new { GenreId = genreId });

            var movies = new List<MovieDto>();

            moviesDbList?.ForEach(m =>
            {
                var movie = ToMovieDto(m);
                movie.PlanType = GetPlanType(subscriptions,m.PlanType);
                movie.Genre = genre?.Name;
                movies.Add(movie);
            });

            return movies;
        }

        public void AddMovie(AddMovieDto movieDto)
        {
            var param = new MovieModel
            {
                MovieId = Guid.NewGuid(),
                Title = movieDto.Title,
                Description = movieDto.Description,
                Poster = movieDto.Poster,
                Backdrop = movieDto.Backdrop,
                Logo = movieDto.Logo,
                GenreId = movieDto.GenreId,
                YoutubeKey = movieDto.YoutubeKey,
                CriticsRating = movieDto.CriticsRating,
                PlanType = movieDto.PlanType
            };

            _databaseService.Execute(InsertMovieQuery, param);
        }

        private MovieDto ToMovieDto(MovieModel model)
        {
            return new MovieDto
            {
                MovieId = model.MovieId,
                Title = model.Title,
                Description = model.Description,
                Poster = model.Poster,
                Backdrop = model.Backdrop,
                Logo = model.Logo,
                Genre = "",
                GenreId = model.GenreId,
                YoutubeKey = model.YoutubeKey,
                UsersRating = 0,
                UsersComment = null,
                CriticsRating = model.CriticsRating,
                PlanType = model.PlanType.ToString()
            };
        }

        private string GetPlanType(IEnumerable<SubscriptionModel> subscriptions, int planType)
        {
            return subscriptions?.FirstOrDefault(s => s.SubscriptionCode == planType)?.Name ?? "Unknown";
        }
    }

}
