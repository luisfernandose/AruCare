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
    public class DoctorController : ApiController
    {
        //TokenValidationHandler tvh = new TokenValidationHandler();

        private readonly ApplicationDbContext db;
        public DoctorController(ApplicationDbContext context)
        {
            db = context;
        }

        [ResponseType(typeof(Response))]
        public Response NextDoc(Doctors dc_)
        {
            Response rp = new Response();
            try
            {
                Doctors dc = db.Doctors.Where(l => l.LastAttention == null && l.Status == true).FirstOrDefault();

                if (dc == null)
                    dc = db.Doctors.Where(d => d.Status == true).OrderBy(o => o.LastAttention).FirstOrDefault();

                DoctorViewModel dvm = new DoctorViewModel();

                Copier.CopyPropertiesTo(dc, dvm);

                dc.LastAttention = DateTime.Now;
                db.SaveChanges();

                rp.RespondeCode = 1;
                rp.Message = ResponseMessages.Response.Successful.ToString();
                rp.Object = dvm;
            }
            catch (Exception ex)
            {
                rp.RespondeCode = 3;
                rp.Message = ex.Message;
            }

            return rp;
        }

        [Route("api/Doctor/NewDoctor")]
        [HttpPut]
        public Response NewDoctor(Doctors doctor)
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

                Doctors pt = db.Doctors.Where(f => f.Email == doctor.Email).FirstOrDefault();

                if (pt == null)
                {
                    pt = db.Doctors.Where(f => f.IdCard == doctor.IdCard).FirstOrDefault();
                    if (pt == null)
                    {
                        DoctorViewModel pvm = new DoctorViewModel();

                        doctor.IdDoctor = Guid.NewGuid();

                        doctor.Created = DateTime.Now;
                        doctor.Updated = DateTime.Now;
                        doctor.Password = Encryptor.MD5Hash(doctor.Password);
                        db.Doctors.Add(doctor);
                        db.SaveChanges();

                        rp.RespondeCode = 1;
                        rp.Message = ResponseMessages.Response.Successful.ToString();

                        Copier.CopyPropertiesTo(doctor, pvm);
                        rp.Object = pvm;
                    }
                    else
                    {
                        rp.RespondeCode = 2;
                        rp.Message = "Medical Identification Number Already Exists";
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

        [Route("api/Doctor/EditDoctor")]
        [HttpPut]
        public Response EditDoctor(Doctors doctor)
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

                Doctors pt = db.Doctors.Where(f => f.Email == doctor.Email && f.IdDoctor != doctor.IdDoctor).FirstOrDefault();

                if (pt == null)
                {
                    pt = db.Doctors.Where(f => f.IdCard == doctor.IdCard && f.IdDoctor != doctor.IdDoctor).FirstOrDefault();
                    if (pt == null)
                    {
                        DoctorViewModel pvm = new DoctorViewModel();

                        pt = db.Doctors.Where(f => f.IdDoctor == doctor.IdDoctor).FirstOrDefault();

                        doctor.Created = pt.Created;
                        doctor.Updated = DateTime.Now;

                        Copier.CopyPropertiesTo(doctor, pt);
                        db.SaveChanges();

                        rp.RespondeCode = 1;
                        rp.Message = ResponseMessages.Response.Successful.ToString();

                        Copier.CopyPropertiesTo(doctor, pvm);
                        rp.Object = pvm;
                    }
                    else
                    {
                        rp.RespondeCode = 2;
                        rp.Message = "Medical Identification Number Already Exists";
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

        [Route("api/Doctor/GetDoctor")]
        [HttpGet]
        public Response GetDoctor(Guid id)
        {
            Response rp = new Response();

            Doctors doctor = db.Doctors.Find(id);
            DoctorViewModel pvm = new DoctorViewModel();

            Copier.CopyPropertiesTo(doctor, pvm);

            if (doctor == null)
            {
                rp.RespondeCode = 2;
                rp.Message = "Doctor Not Found";
                return rp;
            }
            rp.RespondeCode = 1;
            rp.Message = ResponseMessages.Response.Successful.ToString();
            rp.Object = pvm;

            return rp;
        }
    }
}
