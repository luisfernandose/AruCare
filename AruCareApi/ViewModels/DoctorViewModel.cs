using System;
using System.ComponentModel;

namespace AruCareApi.ViewModels
{
    public class DoctorViewModel
    {
        public Guid IdDoctor { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        [DisplayName("Medical Identification Number")]
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public DateTime? LastAttention { get; set; }
        public bool Status { get; set; } = true;
    }
}