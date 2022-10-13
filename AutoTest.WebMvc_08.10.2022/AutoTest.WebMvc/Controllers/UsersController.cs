
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
        private readonly TicketsRepository _ticketsRepository;
        private readonly QuestionRepository _questionsRepository;

        public UsersController()
        {
            _usersRepository = new UsersRepository();
            _cookiesServices = new CookiesServices();
            _usersServices = new UsersServices();
            _ticketsRepository = new TicketsRepository();
            _questionsRepository = new QuestionRepository();
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
        [HttpPost]
        public IActionResult SignIn(UserDto user)
        {

            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var _user = _usersRepository.GetUserByPhone(user.Phone!);
            if (_user.Password == user.Password)
            {
                _cookiesServices.SendUserPhoneToCookie(user.Phone!, HttpContext);
                return RedirectToAction("Index");
            }
            return RedirectToAction("SignIn");
        }

        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.Image ??= "dotnet.jpg";

            _usersRepository.InsertUser(user);

            var _user = _usersRepository.GetUserByPhone(user.Phone!);

            var ticketsCount = _questionsRepository.GetQuestionsCount() / 20;
            _ticketsRepository.InsertUserTrainingTickets(_user.Id, ticketsCount, 20);

            _cookiesServices.SendUserPhoneToCookie(user.Phone!, HttpContext);
            return RedirectToAction("Index");
        }

        public IActionResult EditUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditUser([FromForm] User user)
        {
            var _user = _usersServices.GetUserFromCookie(HttpContext);
            if (_user == null)
            {
                return RedirectToAction("SignIn");
            }

            user.Id = _user.Id;
            user.Image = SaveUserImage(user.ImageFile);
            _usersRepository.UpdateUser(user);

            _cookiesServices.UdateUserPhoneToCookie(user.Phone, HttpContext);

            return RedirectToAction("Index");
        }

        private string? SaveUserImage(IFormFile? imageFile)
        {
            if (imageFile ==  null)
            {
                return "dotnet.jpeg";
            }
            var imagePath = Guid.NewGuid().ToString("N") + Path.GetExtension(imageFile.FileName);

            var ms = new MemoryStream();
            imageFile.CopyTo(ms);
            System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "Profile", imagePath), ms.ToArray());

            return imagePath;
        }
    }

    
}
