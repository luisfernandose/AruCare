using AruCareApi.Data;
using AruCareApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AruCareApi.Controllers
{
    public class ChatController : Controller
    {

        private readonly ApplicationDbContext db;
        public ChatController(ApplicationDbContext context)
        {
            db = context;
        }

        public bool Create(ChatObject co)
        {
            try
            {
                Chat chat = new Chat();
                Guid id = Guid.Parse(co.idapointmment);
                
                chat.Appointment = db.Appointment.Where(t => t.IdAppointment == id).SingleOrDefault();
                chat.Name = co.username;
                chat.Message = co.message;
                chat.Date = DateTime.Now;
                db.Chat.Add(chat);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
