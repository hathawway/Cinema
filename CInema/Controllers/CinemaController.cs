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
            ViewData["Cinemas"] = cinemas.ToArray();
            return View();
        }
    }
}
