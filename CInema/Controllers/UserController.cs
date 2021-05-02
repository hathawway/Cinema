using Cinema.Domain.Db;
using Cinema.Domain.Models.Users;
using Cinema.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CinemaDbContext _context;
        public UserController(
            ILogger<HomeController> logger,
            CinemaDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        /// <summary>
        /// Страница пользовтеля
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Личный кабинет
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Account()
        {
            // TODO: а надо ли вообще ?
            var user = _context.Employees
                .Select(x => x).OrderByDescending(x => x.SecondName);
            return View(user);
        }

        /// <summary>
        /// Форма входа в систему
        /// </summary>
        /// <param name="returnUrl">Путь перехода после авторизации</param>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var g = HttpContext.Session.GetString("_userIsLogged");
            return View();
        }

        /// <summary>
        /// Авторизация в системе
        /// </summary>
        /// <param name="signInManager">Менеджер авторизации</param>
        /// <param name="model">Входные данные с формы</param>
        /// <param name="returnUrl">Путь перехода после авторизации</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(x => x.Login == model.Login).Select(x => x).ToArray()[0];
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Проверьте имя пользователя и пароль");
                    return View(model);
                }
                HttpContext.Session.SetString("_userIsLogged", user.Login);

                return View(model);
            }

            return View(model);
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
            // TODO: ЛОГИН ==  МЕЙЛ ?
            if (_context.Users.Any(x => x.Login.ToLower() == model.EmailAddress.ToLower()))
                ModelState.AddModelError("Email", "Такой email уже используеся в системе");

            var profile = new Employee
            {
                FirstName = model.FirstName,
                SecondName = model.LastName,
            };

            var user = new User
            {
                Login = model.EmailAddress,
                Password = model.Password,
                RoleId = 1
            };

            _context.Users.Add(user);

            HttpContext.Session.SetString("_userIsLogged", user.Login);
            
            /* var result = await _userManager.CreateAsync(user, model.Password);
              if (!result.Succeeded)
              {
                  AddErrors(result);
                  return View(model);
              }

              //await _userManager.AddToRoleAsync(user, SecurityConstants.СustomerRole);
            */
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Выход из системы
        /// </summary>
        /// <param name="signInManager">Менеджер авторизации</param>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Возвращение страницы в случае блокировки пользователя
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        /// <summary>
        /// Подтверждение сброса пароля
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Страница запрета доступа
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        [HttpGet]
        public IActionResult RegistrationNewUser()
        {
            return View();
        }

        #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}
