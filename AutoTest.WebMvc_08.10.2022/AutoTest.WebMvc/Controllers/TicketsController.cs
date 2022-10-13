using AutoTest.WebMvc.Models;
using AutoTest.WebMvc.Repositories;
using AutoTest.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.WebMvc.Controllers;

public class TicketsController : Controller
{
    private UsersServices _usersServices;
    private TicketsRepository _ticketsRepository;

    public TicketsController()
    {
        _ticketsRepository = new TicketsRepository();
        _usersServices = new UsersServices();
    }

    public IActionResult Index()
    {
        var user = _usersServices.GetUserFromCookie(HttpContext);
        if (user == null)
            return RedirectToAction("SignIn", "Users");

        var ticket = _ticketsRepository.GetTicketsByUserId(user.Id);
        return View(ticket);
    }
}
