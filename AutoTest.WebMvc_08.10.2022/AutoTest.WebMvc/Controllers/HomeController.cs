using AutoTest.WebMvc.Models;
using AutoTest.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AutoTest.WebMvc.Controllers;

public class HomeController : Controller
{
    private UsersServices _usersServices;

    public HomeController()
    {
        _usersServices = new UsersServices();
    }
    public IActionResult Index()
    {
        var islogin = true;
        var user = _usersServices.GetUserFromCookie(HttpContext);
        if (user == null)
            islogin = false;

        ViewBag.IsLogin = islogin;
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