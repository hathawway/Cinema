using Cinema.Domain.Db;
using Cinema.Models;
using Cinema.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class CinemaController : Controller
    {
        private readonly CinemaDbContext _context;
        private readonly ISignIn _signInManager;
        public CinemaController(
            CinemaDbContext context,
            ISignIn signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cinemas = _context.Films.Select(x => new FilmViewModel
            {
                Kod = x.Kod,
                Country = _context.Countries.Where(c => c.Kod == x.CountryKod).Select(c => c.Name).ToArray()[0],
                Genre = _context.Genres.Where(g => g.Kod == x.GenreKod).Select(c => c.Name).ToArray()[0],
                Name = x.Name,
                Rating = _context.Ratings.Where(r => r.Kod == x.RatingKod).Select(c => c.Name).ToArray()[0],
                StartDate = x.StartDate,
                Studio = _context.FilmStudios.Where(s => s.Kod == x.StudioKod).Select(c => c.Name).ToArray()[0],
                TimeDuration = x.TimeDuration
            });
            ViewData["TableData"] = cinemas.ToArray();
            ViewData["Headers"] = new string[]{ 
                "Название фильма",
                "Киностудия",
                "Дата премьеры",
                "Страна",
                "Продолжительность",
                "Жанр",
                "Рейтинг"};
            ViewData["TableName"] = "Фильмы";
            return View();
        }

        [HttpGet]
        public IActionResult BoxOffice()
        {
            var rawBoxes = _context.BoxOffices.Select(x => x).ToArray();

            var boxOffices = rawBoxes.Select(x => new BoxOfficeViewModel
            {
                Film = _context.Films.Where(g => g.Kod == x.KodFilm).Select(g => g.Name).ToArray()[0],
                TotalSum = x.TotalSum,
                Date = x.DateBoxOffice,
            }).ToArray();
            ViewData["TableName"] = "Бокс Оффисы";
            ViewData["Headers"] = new string[] { "Фильм", "Сумма", "Дата" };
            ViewData["TableData"] = boxOffices;
            return View();
        }


        [HttpGet]
        public IActionResult FilmSemp()
        {
            var filmSemps = _context.FilmSemps.Select(x => x).ToArray();
            var employees = _context.Employees.Select(x => x).ToArray();
            //var employees = _context.Employees.Select(x => x).ToArray();

            var boxOffices = filmSemps.Select(x => new FilmSempViewModel
            {
                EmployeeName = employees.First(h => h.Kod == x.EmployeeKod).FIO()
                
            }).ToArray();
            ViewData["TableName"] = "Бокс Оффисы";
            ViewData["Headers"] = new string[] { "Фильм", "Сумма", "Дата" };
            ViewData["TableData"] = boxOffices;
            return View();
        }
    }
}
