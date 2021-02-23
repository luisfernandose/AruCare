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
    public class CountriesController : ApiController
    {
        private readonly ApplicationDbContext db;
        public CountriesController(ApplicationDbContext context)
        {
            db = context;
        }

        [ResponseType(typeof(Country))]
        public Response GetCountries()
        {
            Response rp = new Response();

            List<Country> countries = db.Country.ToList();
            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = countries;

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