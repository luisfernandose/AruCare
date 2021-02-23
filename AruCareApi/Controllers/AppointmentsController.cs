using AruCareApi.Data;
using AruCareApi.Models;
using AruCareApi.Utils;
using AruCareApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AruCareApi.Controllers
{
    public class AppointmentsController : ApiController
    {
        //TokenValidationHandler tvh = new TokenValidationHandler();
        private readonly ApplicationDbContext db;
        public AppointmentsController(ApplicationDbContext context)
        {
            db = context;
        }

        [Route("api/Appointment/GetAppointment")]
        [HttpGet]
        public Response GetAppointment(Guid idpatient, int type)
        {
            Response rp = new Response();

            List<Appointment> p_appointmet = db.Appointment.Where(h => h.Patient.IdPatient == idpatient && h.Status == true).ToList();

            if (p_appointmet.Count() > 0)
            {
                rp.RespondeCode = 2;
                rp.Message = "Patient Has an Active Appointment";
                return rp;
            }

            Patient patient = db.Patient.Where(y => y.IdPatient == idpatient).SingleOrDefault();
            if (patient == null)
            {
                rp.RespondeCode = 2;
                rp.Message = "Patient Not Found";
                return rp;
            }

            Doctors dc = db.Doctors.Where(l => l.LastAttention == null && l.Status == true).FirstOrDefault();

            if (dc == null)
            {
                List<Guid> _doctors = db.Appointment.Where(f => f.Status == true).Select(d => d.Doctor.IdDoctor).ToList();

                dc = db.Doctors.Where(d => d.Status == true && !_doctors.Contains(d.IdDoctor)).OrderBy(o => o.LastAttention).FirstOrDefault();
            }

            if (dc == null)
            {
                rp.RespondeCode = 2;
                rp.Message = "No Doctors Available";
                return rp;
            }

            dc.LastAttention = DateTime.Now;

            Appointment ap = new Appointment();
            ap.IdAppointment = Guid.NewGuid();
            ap.Doctor = dc;
            ap.Patient = patient;
            ap.Date = DateTime.Now;
            ap.Type = type;
            db.Appointment.Add(ap);
            db.SaveChanges();

            AppointmentViewModel avm = new AppointmentViewModel();
            DoctorViewModel dvm = new DoctorViewModel();

            Copier.CopyPropertiesTo(dc, dvm);
            avm.Doctor = dvm;
            avm.IdAppointment = ap.IdAppointment;

            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = avm;
            return rp;
        }


        [Route("api/Appointment/GetDoctorAppointments")]
        [HttpGet]
        public Response GetDoctorAppointments(Guid iddoctor)
        {
            Response rp = new Response();

            List<Appointment> p_appointmet = db.Appointment.Where(h => h.Doctor.IdDoctor == iddoctor).OrderBy(o => o.Date).ToList();

            if (p_appointmet.Count == 0)
            {
                rp.RespondeCode = 2;
                rp.Message = "Does not have appointments";
                return rp;
            }
            List<AppointmentListViewModel> lavm = new List<AppointmentListViewModel>();

            foreach (var i in p_appointmet)
            {
                AppointmentListViewModel avm = new AppointmentListViewModel();
                Copier.CopyPropertiesTo(i, avm);
                avm.IdDoctor = i.Doctor.IdDoctor;
                avm.Idpatient = i.Patient.IdPatient;
                lavm.Add(avm);
            }

            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = lavm;
            return rp;
        }

        [Route("api/Appointment/GetPatientAppointments")]
        [HttpGet]
        public Response GetPatientAppointments(Guid idpatient)
        {
            Response rp = new Response();

            List<Appointment> p_appointmet = db.Appointment.Where(h => h.Patient.IdPatient == idpatient).OrderBy(o => o.Date).ToList();

            if (p_appointmet.Count == 0)
            {
                rp.RespondeCode = 2;
                rp.Message = "Does not have appointments";
                return rp;
            }
            List<AppointmentListViewModel> lavm = new List<AppointmentListViewModel>();

            foreach (var i in p_appointmet)
            {
                AppointmentListViewModel avm = new AppointmentListViewModel();
                Copier.CopyPropertiesTo(i, avm);
                avm.IdDoctor = i.Doctor.IdDoctor;
                avm.Idpatient = i.Patient.IdPatient;
                lavm.Add(avm);
            }

            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = lavm;
            return rp;
        }

        [Route("api/Appointment/GetAppointmentDetail")]
        [HttpGet]
        public Response GetAppointmentDetail(Guid idappointmet)
        {
            Response rp = new Response();

            Appointment p_appointmet = db.Appointment.Where(h => h.IdAppointment == idappointmet).SingleOrDefault();

            if (p_appointmet == null)
            {
                rp.RespondeCode = 2;
                rp.Message = "Appointments Not Exists";
                return rp;
            }
            AppointmentListViewModel avm = new AppointmentListViewModel();
            Copier.CopyPropertiesTo(p_appointmet, avm);
            avm.IdDoctor = p_appointmet.Doctor.IdDoctor;
            avm.Idpatient = p_appointmet.Patient.IdPatient;

            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = avm;
            return rp;
        }

        [Route("api/Appointment/EndAppointment")]
        [HttpGet]
        public Response EndAppointment(Appointment Appointment)
        {
            Response rp = new Response();

            Appointment appointmet = db.Appointment.Where(h => h.IdAppointment == Appointment.IdAppointment).SingleOrDefault();

            if (appointmet == null)
            {
                rp.RespondeCode = 2;
                rp.Message = "Appointment Does not Exists";
                return rp;
            }
            appointmet.Status = false;
            appointmet.EndDate = DateTime.Now;

            if (!string.IsNullOrEmpty(Appointment.DoctorComment))
                appointmet.DoctorComment = Appointment.DoctorComment;

            if (!string.IsNullOrEmpty(Appointment.DoctorRecommendations))
                appointmet.DoctorRecommendations = Appointment.DoctorRecommendations;

            if (!string.IsNullOrEmpty(Appointment.PatientComment))
                appointmet.PatientComment = Appointment.PatientComment;

            if (Appointment.AppointmentRate != 0)
                appointmet.AppointmentRate = Appointment.AppointmentRate;

            db.SaveChanges();
            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = "";
            return rp;
        }

        [Route("api/Appointment/UpdateCost")]
        [HttpPost]
        public Response UpdateCost(Appointment Appointment)
        {
            Response rp = new Response();

            Appointment appointmet = db.Appointment.Where(h => h.IdAppointment == Appointment.IdAppointment).SingleOrDefault();

            if (appointmet == null)
            {
                rp.RespondeCode = 2;
                rp.Message = "Appointment Does not Exists";
                return rp;
            }

            if (Appointment.Cost > 0)
                appointmet.Cost = Appointment.Cost;



            db.SaveChanges();
            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = "";
            return rp;
        }

        [Route("api/Appointment/UpdatePayment")]
        [HttpPost]
        public Response UpdatePayment(Appointment Appointment)
        {
            Response rp = new Response();

            Appointment appointmet = db.Appointment.Where(h => h.IdAppointment == Appointment.IdAppointment).SingleOrDefault();

            if (appointmet == null)
            {
                rp.RespondeCode = 2;
                rp.Message = "Appointment Does not Exists";
                return rp;
            }

            if (!string.IsNullOrEmpty(Appointment.PaymentReference))
            {
                appointmet.PaymentReference = Appointment.PaymentReference;
                appointmet.Paid = true;
            }

            db.SaveChanges();
            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = "";
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