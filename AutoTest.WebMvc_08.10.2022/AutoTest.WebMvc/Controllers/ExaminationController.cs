using AutoTest.WebMvc.Models;
using AutoTest.WebMvc.Repositories;
using AutoTest.WebMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoTest.WebMvc.Controllers
{
    public class ExaminationController : Controller
    {
        private readonly QuestionRepository _questionRepository;
        private readonly UsersServices _usersServices;
        private readonly TicketsRepository _ticketsRepository;

        private const int ticketQuestionsCount = 20;
        public ExaminationController()
        {
            _questionRepository = new QuestionRepository();
            _usersServices = new UsersServices();
            _ticketsRepository = new TicketsRepository();
        }
        public IActionResult Index()
        {
            var user = _usersServices.GetUserFromCookie(HttpContext);
            if (user == null)
                return RedirectToAction("SignIn", "Users");

            var ticket = CreateRandomTicket(user);

            return View(ticket);
        }

        private Ticket CreateRandomTicket(User user)
        {
            var ticketsCount = _questionRepository.GetQuestionsCount() / ticketQuestionsCount;
            var random = new Random();
            var ticketIndex = random.Next(0, ticketsCount);
            var from = ticketIndex * ticketQuestionsCount;

            var ticket = new Ticket(user.Id, from, ticketQuestionsCount);
            _ticketsRepository.InsertTicket(ticket);

            var id = _ticketsRepository.GetLastRowId();
            ticket.Id = id;

            return ticket;
        }

        public IActionResult Exam(int ticketId, int? questionId = null, int? choiceId = null)
        {
            var user = _usersServices.GetUserFromCookie(HttpContext);
            if (user == null) return RedirectToAction("SignIn", "Users");

            var ticket = _ticketsRepository.GetTicketById(ticketId, user.Id);

            questionId = questionId ?? ticket.FromIndex;

            if (ticket.FromIndex <= questionId && ticket.FromIndex + ticket.QuestionsCount > questionId)
            {
            questionId = questionId ?? 0;
            ViewBag.Ticket = ticket;

            var question = _questionRepository.GetQuestionById(questionId.Value);
                if (choiceId != null)
                {
                    ViewBag.ChoiceId = choiceId;
                    ViewBag.Answer = question.Choices!.First(ch => ch.Id == choiceId).Answer;
                }

            return View(question);
            }

            return NotFound();
        }

        public IActionResult GetQuestionById(int questionId)
        {
            var question = _questionRepository.GetQuestionById(questionId);
            return View(question);
        }
    }
}
