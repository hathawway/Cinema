using Cinema.Domain.Db;
using Cinema.Models;
using Cinema.Models.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class QueryController : Controller
    {
        private readonly CinemaDbContext _context;
        private readonly string ConnectionString;
        public QueryController(CinemaDbContext context, IConfiguration Configuration)
        {
            _context = context;
            ConnectionString = Configuration.GetConnectionString("OracleDBConnection");
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
            var films = _context.Films.Where(x => x.StartDate.Year == 2021 && x.CountryKod == russiaCode).Select(x => new FilmViewModel
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
            OracleConnection con = new();
            con.ConnectionString = ConnectionString;
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select kino.PACKAGE__P.spisokemp from dual";
            List<List<string>> response = new();
            var lenth = 1;
            if (lenth > 0)
            {
                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    response.Add(new List<string>());
                    for (int i = 0; i < lenth; i++)
                    {
                        response[^1].Add(reader.GetString(i));
                    }
                }
            }
            else
            {
                cmd.ExecuteNonQuery();
            }

            con.Close();

            var result = response[0][0].Split("haha").ToArray();
            result = result.Take(result.Length - 1).ToArray();
            ViewData["TableData"] = result;
            ViewData["Headers"] = new string[] { "Фильмы, в которых режиссер является одновременно " +
                "исполнителем главной роли"};
            return View();
        }

        [HttpGet]
        public IActionResult StatisticsGenre()
        {
            var russiaCode = _context.Countries.First(x => x.Name == "Россия").Kod;
            var films = _context.Films.Select(x => x).ToArray();

            var genres = films.GroupBy(x => x.GenreKod).Select(x => new 
            {
                Genre = x.Key,
                RuFilm = films.Where(f => f.GenreKod == x.Key && f.CountryKod == russiaCode).Count(),
                Count = x.Count()
            }).Select(x => new StatOnGenre
            {
                Genre = _context.Genres.First(g => g.Kod == x.Genre).Name,
                RusAmount = x.RuFilm,
                ForreignAmount = x.Count - x.RuFilm
            }).AsEnumerable();
            ViewData["TableName"] = "Статистика по жанрам";
            ViewData["Headers"] = new string[] { "Жанр", "Кол-во фильмов снятых в России", "Кол-во фильмов снятых в др. странах" };
            ViewData["TableData"] = genres;
        
            return View();
        }
    }
}
