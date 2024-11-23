﻿using deflix.monolithic.api.DTOs;
using deflix.monolithic.api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace deflix.monolithic.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ApiController
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("user/{userId}/list")]
        public IActionResult GetAllMovies(Guid userId)
        {
            var movies = _movieService.GetAllMovies(userId);
            return Ok(movies);
        }

        [HttpGet("user/{userId}/movie/{movieId}")]
        public IActionResult GetMovieById(Guid userId, Guid movieId)
        {
            var movie = _movieService.GetMovieById(userId, movieId);
            if (movie == null)
            {
                return NotFound(new { message = "Movie not found" });
            }

            return Ok(movie);
        }

        [HttpGet("user/{userId}/list/genre/{genreId}")]
        public IActionResult GetMoviesByGenre(Guid userId, Guid genreId)
        {
            var movies = _movieService.GetMoviesByGenre(userId, genreId);
            return Ok(movies);
        }

        [HttpPost]
        public IActionResult AddMovie([FromBody] AddMovieDto movieDto)
        {
            _movieService.AddMovie(movieDto);
            return Ok(new { message = "Movie added successfully" });
        }


    }

}
