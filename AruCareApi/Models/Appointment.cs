using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AruCareApi.Models
{
    [Table("Appointment")]
    public class Appointment
    {
        [Key]
        public Guid IdAppointment { get; set; }
        public virtual Doctors Doctor { get; set; }
        public virtual Patient Patient { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }        
        public string AppointmentAddress { get; set; }
        public string AppointmentGps { get; set; }
        public string DoctorComment { get; set; }
        public string DoctorRecommendations { get; set; }
        public string PatientComment { get; set; }
        public decimal AppointmentRate { get; set; }
        public bool Status { get; set; } = true;
        public DateTime? EndDate { get; set; }
        public decimal Cost { get; set; }
        public bool Paid { get; set; }
        public string PaymentReference { get; set; }
    }
}