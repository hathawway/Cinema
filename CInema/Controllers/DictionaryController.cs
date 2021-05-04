using Cinema.Domain.Db;
using Cinema.Domain.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly CinemaDbContext _context;
        public DictionaryController(
            CinemaDbContext context
            )
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Countries()
        {
            var countries = _context.Countries.Select(x => x).ToArray();
            ViewData["TableName"] = "Страны";
            ViewData["Headers"] = new string[] { "Идентификатор", "Название"};
            ViewData["TableData"] = countries;
            return View();
        }
        [HttpPost]
        public IActionResult AddCountries(Country country)
        {
            var countries = _context.Countries.Add(country);
            _context.SaveChanges();
            return View();
        }

        [HttpGet]
        public IActionResult Genre()
        {
            var genres = _context.Genres.Select(x => x).ToArray();
            ViewData["TableName"] = "Жанры";
            ViewData["Headers"] = new string[] { "Идентификатор", "Название" };
            ViewData["TableData"] = genres;
            return View();
        }
        [HttpPost]
        public IActionResult AddGenre(Genre genre)
        {
            var countries = _context.Genres.Add(genre);
            _context.SaveChanges();
            return View();
        }

    }
}
