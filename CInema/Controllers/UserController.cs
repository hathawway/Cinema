using Cinema.Domain.Db;
using Cinema.Domain.Models.Users;
using Cinema.Models;
using Cinema.Models.User;
using Cinema.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cinema.Controllers
{
    public class UserController : Controller
    {
        private readonly CinemaDbContext _context;
        private readonly ISignIn _signInManager;
        public UserController(
            CinemaDbContext context,
            ISignIn SignInManager)
        {
            _context = context;
            _signInManager = SignInManager;
        }
        
        /// <summary>
        /// Форма входа в систему
        /// </summary>
        /// <param name="returnUrl">Путь перехода после авторизации</param>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(x => x.Login == model.Login);
                if (user == null)
                {
                    ViewData["ERROR"] = "Ошибка при попытке войти! Проверьте логин/пароль";
                    return View(model);
                }
                _signInManager.SignIn(user);
            }

            return RedirectToAction("Index", "Cinema");
        }
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="model">Данные о новом пользователе</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrationNewUser(NewUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Users.Any(x => x.Login.ToLower() == model.Login.ToLower()))
            {
                return View(model);
            }

            var user = new User
            {
                Login = model.Login,
                Password = model.Password,
                RoleId = 1,
            };

            var userInserted =_context.Users.Add(user).Entity;
            _context.SaveChanges();

            var profile = new UserInfo
            {
                FirstName = model.FirstName,
                SecondName = model.LastName,
                KodUser = userInserted.Kod,
            };

            _context.UserInfo.Add(profile);
            _context.SaveChanges();
            user = _context.Users.Where(x => x.Login == model.Login).Select(x => x).ToArray()[0];
            _signInManager.SignIn(user);

            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Выход из системы
        /// </summary>
        /// <param name="signInManager">Менеджер авторизации</param>
        [HttpGet]
        public IActionResult Logout()
        {
            _signInManager.SignOut();
            return RedirectToAction("Login", "User");
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        [HttpGet]
        public IActionResult RegistrationNewUser()
        {
            return View();
        }

        public IActionResult AllUsers(long id)
        {
            UserViewModel defaultMode = new();
            if (id != 0)
            {
                defaultMode = _context.Users.Where(x => x.Kod == id).
                    Select(x => new UserViewModel() 
                    {
                        Kod = x.Kod,
                        Login = x.Login,
                        Password = x.Password,
                        Role = _context.Roles.Where(u => u.Kod == x.RoleId).
                                              Select(u => new IdName()
                                              {
                                                Id = u.Kod,
                                                Name = u.Name,
                                              }).First()
                    }).First();
            }

            var employees = _context.Users.Select(e => new UserViewModel
            {
                Kod = e.Kod,
                Login = e.Login,
                Password = e.Password,
                Role = _context.Roles.Where(u => u.Kod == e.RoleId)
                                     .Select(u => new IdName()
                                     {
                                         Id = u.Kod,
                                         Name = u.Name,
                                     }).First()
            });

            ViewData["TableName"] = "Сотрудники";
            ViewData["Headers"] = new string[] {"","Логин", "Пароль", "Роль"};
            ViewData["TableData"] = employees.ToArray();
            ViewData["Roles"] = _context.Roles.Select(x => new IdName() 
            {
                Id = x.Kod,
                Name = x.Name,
            }).AsEnumerable();
            return View(defaultMode);
        }

        public IActionResult EditUser(UserViewModel model)
        {
            if (model.Kod != 0)
            {
                var toEdit = _context.Users.First(x => x.Kod == model.Kod);
                toEdit.Login = model.Login;
                toEdit.Password = model.Password;
                toEdit.RoleId = model.Role.Id;
            } else
            {
                _context.Users.Add(new User()
                {
                    Login = model.Login,
                    Password = model.Password,
                    RoleId = model.Role.Id,
                });
            }

            _context.SaveChanges();
            return RedirectToAction("AllUsers", "User");
        }
    }
}
