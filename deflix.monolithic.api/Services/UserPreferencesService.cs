using deflix.monolithic.api.DTOs;
using deflix.monolithic.api.Interfaces;
using deflix.monolithic.api.Models;
using System.Collections.Generic;

namespace deflix.monolithic.api.Services
{
    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMovieService _movieService;

        #region SqlQueries
        private static string InsertQuery => @"INSERT INTO [UserPreferences]
                                                ([UserPreferencesId],[UserId],[MovieId],UserRating,UserComment,[IsFavorite],[IsWatchList])
                                                VALUES
                                                (NEWID(),@UserId,@MovieId,0,'',0,0)
                                                ";

        private static string UpdateFavoriteQuery => @"UPDATE [UserPreferences]
                                                SET [IsFavorite] = @IsFavorite
                                                WHERE [UserId] = @UserId And MovieId=@MovieId
                                                ";

        private static string UpdateWatchlistQuery => @"UPDATE [UserPreferences]
                                                SET [IsWatchList] = @IsWatchList
                                                WHERE [UserId] = @UserId And MovieId=@MovieId
                                                ";

        private static string UpdateCommentQuery => @"UPDATE [UserPreferences]
                                                SET [UserRating] = @UserRating, UserComment = @UserComment
                                                WHERE [UserId] = @UserId And MovieId=@MovieId
                                                ";

        public static string GetUsersPreferencesQuery =>
            @"SELECT [UserPreferencesId],[UserId],[MovieId],UserRating,UserComment,[IsFavorite],[IsWatchList]
            FROM [UserPreferences]
            order by MovieId,UserRating desc
            ";

        public static string GetUserPreferencesQuery =>
            @"SELECT [UserPreferencesId],[UserId],[MovieId],UserRating,UserComment,[IsFavorite],[IsWatchList]
            FROM [UserPreferences]
            WHERE UserId = @UserId
            order by MovieId,UserRating desc
            ";

        #endregion

        public UserPreferencesService(IDatabaseService databaseService, IMovieService movieService)
        {
            _databaseService = databaseService;
            _movieService = movieService;
        }

        public IEnumerable<MovieDto> GetFavoritesForUser(Guid userId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null || userPreferences.Count == 0) return null;

            var movieIds = userPreferences.Where(up => up.IsFavorite).Select(u => u.MovieId).ToList();
            if (movieIds.Count == 0) return null;
            var movies = _movieService.GetMovieByIds(userId, movieIds)?.ToList();
            movies = AddUserCommentRate(movies, userPreferences);
            return movies;
        }

        public void AddFavorite(Guid userId, Guid movieId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null || userPreferences.Count == 0 || userPreferences.Count(c => c.MovieId == movieId) == 0)
            {
                _databaseService.Execute(InsertQuery, new { UserId = userId, MovieId = movieId });
            }
            _databaseService.Execute(UpdateFavoriteQuery, new { UserId = userId, MovieId = movieId, IsFavorite = 1 });
        }

        public void RemoveFavorite(Guid userId, Guid movieId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null || userPreferences.Count == 0 || userPreferences.Count(c => c.MovieId == movieId) == 0) return;
            _databaseService.Execute(UpdateFavoriteQuery, new { UserId = userId, MovieId = movieId, IsFavorite = 0 });
        }

        public IEnumerable<MovieDto> GetWatchlistForUser(Guid userId)
        {

            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null || userPreferences.Count == 0) return null;
            var movieIds = userPreferences.Where(up => up.IsWatchList).Select(u => u.MovieId).ToList();
            if (movieIds.Count == 0) return null;
            var movies = _movieService.GetMovieByIds(userId, movieIds)?.ToList();
            movies = AddUserCommentRate(movies, userPreferences);
            return movies;
        }

        public void AddToWatchlist(Guid userId, Guid movieId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null || userPreferences.Count == 0 || userPreferences.Count(c => c.MovieId == movieId) == 0)
            {
                _databaseService.Execute(InsertQuery, new { UserId = userId, MovieId = movieId });
            }
            _databaseService.Execute(UpdateWatchlistQuery, new { UserId = userId, MovieId = movieId, IsWatchList = 1 });
        }

        public void RemoveFromWatchlist(Guid userId, Guid movieId)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null || userPreferences.Count == 0 || userPreferences.Count(c => c.MovieId == movieId) == 0) return;
            _databaseService.Execute(UpdateWatchlistQuery, new { UserId = userId, MovieId = movieId, IsWatchList = 0 });
        }

        public void AddComment(Guid userId, Guid movieId, UserCommentDTO comment)
        {
            var userPreferences = _databaseService.Query<UserPreferencesModel>(GetUserPreferencesQuery, new { userId });
            if (userPreferences == null || userPreferences.Count == 0 || userPreferences.Count(c => c.MovieId == movieId) == 0)
            {
                _databaseService.Execute(InsertQuery, new { UserId = userId, MovieId = movieId });
            }
            _databaseService.Execute(UpdateCommentQuery, new { UserId = userId, MovieId = movieId, UserRating = comment.Rating, UserComment = comment.Comment });
        }

        private List<MovieDto> AddUserCommentRate(List<MovieDto> movies, List<UserPreferencesModel> userPreferences)
        {
            movies?.ForEach(movie =>
            {
                var userPreference = userPreferences.FirstOrDefault(u => u.MovieId == movie.MovieId);
                if (userPreference != null)
                {
                    movie.UsersRating = userPreference.UserRating;
                    movie.UsersComment = userPreference.UserComment;
                }
            });

            return movies;
        }
    }
}
