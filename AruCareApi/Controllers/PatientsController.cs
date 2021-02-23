using AruCareApi.Data;
using AruCareApi.Models;
using AruCareApi.Utils;
using AruCareApi.ViewModels;
using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;


namespace AruCareApi.Controllers
{
    public class PatientsController : ApiController
    {
        private readonly ApplicationDbContext db;
        public PatientsController(ApplicationDbContext context)
        {
            db = context;
        }

        //[ResponseType(typeof(Patient))]
        [Route("api/Patients/GetPatient")]
        [HttpGet]
        public Response GetPatient(Guid id)
        {
            Response rp = new Response();

            Patient patient = db.Patient.Find(id);
            PatientViewModel pvm = new PatientViewModel();

            Copier.CopyPropertiesTo(patient, pvm);

            if (patient == null)
            {
                rp.RespondeCode = 2;
                rp.Message = "Patient Not Found";
                return rp;
            }
            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = pvm;

            return rp;
        }

        [Route("api/Patients/NewPatient")]
        [HttpPut]
        public Response PutPatient(Patient patient)
        {
            Response rp = new Response();
            try
            {
                if (!ModelState.IsValid)
                {
                    rp.RespondeCode = 2;
                    rp.ModelState = ModelState;
                    rp.Message = "Error In Model";
                    return rp;
                }

                Patient pt = db.Patient.Where(f => f.Email == patient.Email).FirstOrDefault();

                if (pt == null)
                {
                    pt = db.Patient.Where(f => f.IdPassport == patient.IdPassport).FirstOrDefault();
                    if (pt == null)
                    {
                        PatientViewModel pvm = new PatientViewModel();

                        patient.IdPatient = Guid.NewGuid();
                        var today = DateTime.Today;
                        var age = today.Year - patient.BirthdDay.Year;
                        if (patient.BirthdDay > today.AddYears(-age)) age--;
                        patient.Age = age;

                        patient.Created = DateTime.Now;
                        patient.Updated = DateTime.Now;
                        patient.Password = Encryptor.MD5Hash(patient.Password);
                        db.Patient.Add(patient);
                        db.SaveChanges();

                        rp.RespondeCode = 1;
                        rp.Message = ResponseMessages.Response.Successful.ToString();

                        Copier.CopyPropertiesTo(patient, pvm);
                        rp.Object = pvm;
                    }
                    else
                    {
                        rp.RespondeCode = 2;
                        rp.Message = "Id or Passport Already Exists";
                    }
                }
                else
                {
                    rp.RespondeCode = 2;
                    rp.Message = "Email Already Exists";
                }
            }
            catch (Exception ex)
            {
                rp.RespondeCode = 3;
                rp.Message = ex.Message;
            }
            return rp;
        }

        [Route("api/Patients/EditPatient")]
        [HttpPut]
        public Response EditPatient(Patient patient)
        {
            Response rp = new Response();
            try
            {
                if (!ModelState.IsValid)
                {
                    rp.RespondeCode = 2;
                    rp.ModelState = ModelState;
                    rp.Message = "Error In Model";
                    return rp;
                }

                Patient pt = db.Patient.Where(f => f.Email == patient.Email && f.IdPatient != patient.IdPatient).FirstOrDefault();

                if (pt == null)
                {
                    pt = db.Patient.Where(f => f.IdPassport == patient.IdPassport && f.IdPatient != patient.IdPatient).FirstOrDefault();
                    if (pt == null)
                    {
                        PatientViewModel pvm = new PatientViewModel();

                        pt = db.Patient.Where(f => f.IdPatient == patient.IdPatient).FirstOrDefault();

                        var today = DateTime.Today;
                        var age = today.Year - patient.BirthdDay.Year;
                        if (patient.BirthdDay > today.AddYears(-age)) age--;

                        patient.Age = age;
                        patient.Created = pt.Created;
                        patient.Updated = DateTime.Now;

                        Copier.CopyPropertiesTo(patient, pt);
                        db.SaveChanges();

                        rp.RespondeCode = 1;
                        rp.Message = ResponseMessages.Response.Successful.ToString();

                        Copier.CopyPropertiesTo(patient, pvm);
                        rp.Object = pvm;
                    }
                    else
                    {
                        rp.RespondeCode = 2;
                        rp.Message = "Id or Passport Already Exists";
                    }
                }
                else
                {
                    rp.RespondeCode = 2;
                    rp.Message = "Email Already Exists";
                }
            }
            catch (Exception ex)
            {
                rp.RespondeCode = 3;
                rp.Message = ex.Message;
            }
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