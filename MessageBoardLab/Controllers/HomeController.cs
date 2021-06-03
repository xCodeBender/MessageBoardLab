using MessageBoardLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoardLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Message()
        {
            List<Messages> messageList = new List<Messages>();
            using(MessageDBContext context = new MessageDBContext())
            {
                messageList = context.Messages.ToList();
            }
            return View(messageList);
        }

        [Authorize, HttpPost]
        public IActionResult Message(string message)
        {
            using (MessageDBContext context = new MessageDBContext())
            {
                Messages newMessage = new Messages();
                newMessage.UserId = User.Identity.Name;
                newMessage.PostedTime = DateTime.Now;
                newMessage.Updated = false;
                newMessage.Message = message;
                context.Add(newMessage);
                context.SaveChanges();

            }
            return Redirect("Message");
        }

        

        [Authorize]
        public IActionResult Edit(int id)
        {

            using (MessageDBContext context = new MessageDBContext())
            {
                Messages newM = new Messages();
                newM = context.Messages.ToList().Find(o => o.Id == id);

                newM.Updated = false;
                return View(newM);

            }
        }

        [Authorize, HttpPost]
        public IActionResult Edits(string messages, int id)
        {
            Messages newM= new Messages();
            using (MessageDBContext context = new MessageDBContext())
            {
                newM = context.Messages.ToList().Find(o => o.Id == id);
               
                newM.Updated = true;
                newM.Message = messages;
                
                context.SaveChanges();

            }
            return RedirectToAction("Message");
        }

        [Authorize, HttpPost, HttpDelete]
        public IActionResult Deleted(int id, string message)
        {
            Messages newM = new Messages();
            using (MessageDBContext context = new MessageDBContext())
            {
                newM = context.Messages.ToList().Find(o => o.Id == id);

                newM.Message = message;
                context.Messages.Remove(newM);
                context.SaveChanges();

            }
            return RedirectToAction("Message");
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
