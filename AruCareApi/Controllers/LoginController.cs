using AruCareApi.Data;
using AruCareApi.Models;
using AruCareApi.Utils;
using AruCareApi.ViewModels;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace AruCareApi.Controllers
{
    public class LoginController : ApiController
    {
        private readonly ApplicationDbContext db;
        public LoginController(ApplicationDbContext context)
        {
            db = context;
        }

        [ResponseType(typeof(void))]
        public Response Login(LoginViewModel login)
        {
            Response rp = new Response();
            try
            {
                string pass = Encryptor.MD5Hash(login.password);

                Patient pt = db.Patient.Where(p => p.Email == login.email && p.Password == pass).FirstOrDefault();
                PatientViewModel pvm = new PatientViewModel();
                if (pt != null)
                {
                    Copier.CopyPropertiesTo(pt, pvm);

                    rp.RespondeCode = 1;
                    rp.Message = ResponseMessages.Response.Successful.ToString();
                    rp.Object = pvm;
                }
                else
                {
                    rp.RespondeCode = 2;
                    rp.Message = "Wrong User or Password";
                }
            }
            catch (Exception ex)
            {
                rp.RespondeCode = 3;
                rp.Message = ex.Message;
            }
            return rp;
        }

       
    }
}
