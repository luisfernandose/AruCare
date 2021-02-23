using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AruCareApi.Models;

namespace AruCareApi.ViewModels
{
    public class AppointmentViewModel
    {
        public Guid IdAppointment { get; set; }
        public DoctorViewModel Doctor = new DoctorViewModel();
    }
}