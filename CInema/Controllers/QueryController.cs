using Cinema.Domain.Db;
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class QueryController : Controller
    {
        private readonly CinemaDbContext _context;
        public QueryController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult BoxOffice()
        {
            var boxOffices = _context.BoxOffices.Select(x => new BoxOfficeViewModel
            {
                Film = _context.Films.First(g => g.Kod == x.KodFilm).Name,
                TotalSum = x.TotalSum,
                Date = x.DateBoxOffice,
            }).ToArray();
            ViewData["TableName"] = "Кассовые сборы";
            ViewData["Headers"] = new string[] { "Фильм", "Сумма", "Дата" };
            ViewData["TableData"] = boxOffices;
            return View();
        }

        [HttpGet]
        public IActionResult ListFilms()
        {
            var russiaCode = _context.Countries.First(x => x.Name == "Россия").Kod;
            var films = _context.Films.Where(x => x.StartDate.Year > 2021 && x.CountryKod == russiaCode).Select(x => new FilmViewModel
            {
                Id = x.Kod,
                Country = _context.Countries.First(c => c.Kod == x.CountryKod).Name,
                Genre = _context.Genres.First(g => g.Kod == x.GenreKod).Name,
                Name = x.Name,
                Rating = _context.Ratings.First(r => r.Kod == x.RatingKod).Name,
                StartDate = x.StartDate,
                Studio = _context.FilmStudios.First(s => s.Kod == x.StudioKod).Name,
                TimeDuration = x.TimeDuration
            }).AsEnumerable();
            ViewData["TableName"] = "Список фильмов, вышедших в текущем году в России";
            ViewData["Headers"] = new string[] { "Фильм", "Дата", "Страна" };
            ViewData["TableData"] = films;
            return View();
        }

        [HttpGet]
        public IActionResult ListRole()
        {
            //TODO call procedure
            return View();
        }

        [HttpGet]
        public IActionResult StatisticGenre()
        {
            var russiaCode = _context.Countries.First(x => x.Name == "Россия").Kod;
            var films = _context.Films.Select(x => x).ToArray();

            var genres = films.GroupBy(x => x.GenreKod).Select(x => new 
            {
                Genre = x.Key,
                RuFilm = films.Where(f => f.GenreKod == x.Key && f.CountryKod == russiaCode).Count(),
                Count = x.Count()
            }).Select(x => new 
            {
                Genre = _context.Genres.First(g => g.Kod == x.Genre).Name,
                RuFilm = x.RuFilm,
                Count = x.Count - x.RuFilm
            }).AsEnumerable();
            ViewData["TableName"] = "Статистика по жанрам";
            ViewData["Headers"] = new string[] { "Жанр", "Кол-во фильмов снятых в России", "Кол-во фильмов снятых в др. странах" };
            ViewData["TableData"] = genres;
        
            return View();
        }
    }
}
