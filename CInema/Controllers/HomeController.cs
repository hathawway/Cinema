using Cinema.Domain.Db;
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CinemaDbContext _context;
        public HomeController(ILogger<HomeController> logger, CinemaDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var boxOffice = _context.BoxOffices.Select(x => x).ToArray();
            var countries = _context.Countries.Select(x => x).ToArray();
            var employees = _context.Employees.Select(x => x).ToArray();
            var films = _context.Films.Select(x => x).ToArray();
            
            var semps = _context.FilmSemps.Select(x => x).ToArray();
            var filmStudios = _context.FilmStudios.Select(x => x).ToArray();
            var genres = _context.Genres.Select(x => x).ToArray();
            var ratings = _context.Ratings.Select(x => x).ToArray();
            var roleUsers = _context.RoleUser.Select(x => x).ToArray();
            var sessionFilms = _context.SessionFilms.Select(x => x).ToArray();
            var users = _context.Users.Select(x => x).ToArray();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
