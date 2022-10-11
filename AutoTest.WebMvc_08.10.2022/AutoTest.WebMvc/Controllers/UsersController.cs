
using AutoTest.WebMvc.Models;
using AutoTest.WebMvc.Repositories;
using AutoTest.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.WebMvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersRepository _usersRepository;
        private readonly CookiesServices _cookiesServices;
        private readonly UsersServices _usersServices;

        public UsersController()
        {
            _usersRepository = new UsersRepository();
            _cookiesServices = new CookiesServices();
            _usersServices = new UsersServices();
        }

        public IActionResult Index()
        {
            var user = _usersServices.GetUserFromCookie(HttpContext);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("SignIn");
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult SignUpPost(User user)
        {
            _usersRepository.InsertUser(user);
            _cookiesServices.SendUserPhoneToCookie(user.Phone!, HttpContext);
            return RedirectToAction("Index");
        }

        public IActionResult SignInPost(User user)
        {
            var _user = _usersRepository.GetUserByPhone(user.Phone!);
            if (_user.Password == user.Password)
            {
                _cookiesServices.SendUserPhoneToCookie(user.Phone!, HttpContext);
                return RedirectToAction("Index");
            }
            return RedirectToAction("SignIn");
        }

    }
}
