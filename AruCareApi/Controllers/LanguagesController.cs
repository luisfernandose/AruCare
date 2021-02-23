using AruCareApi.Data;
using AruCareApi.Models;
using AruCareApi.Utils;
using AruCareApi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace AruCareApi.Controllers
{
    public class LanguagesController : ApiController
    {
        private readonly ApplicationDbContext db;        
        public LanguagesController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: api/Languages/5
        [ResponseType(typeof(Languages))]
        public Response GetLanguages()
        {
            Response rp = new Response();

            List<Languages> languages = db.Languages.ToList();
            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = languages;

            return rp;
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}