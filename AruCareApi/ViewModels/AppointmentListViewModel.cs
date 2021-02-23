using System;
using AruCareApi.Models;

namespace AruCareApi.ViewModels
{
    public class AppointmentListViewModel
    {
        public Guid IdAppointment { get; set; }
        public Guid IdDoctor { get; set; }
        public Guid Idpatient { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public string AppointmentAddress { get; set; }
        public string AppointmentGps { get; set; }
        public string DoctorComment { get; set; }
        public string DoctorRecommendations { get; set; }
        public string PatientComment { get; set; }
        public decimal AppointmentRate { get; set; }
        public bool Status { get; set; } = true;
        public decimal Cost { get; set; }
        public bool Paid { get; set; }
        public string PaymentReference { get; set; }
    }
}