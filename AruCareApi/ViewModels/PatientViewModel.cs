using System;
using System.ComponentModel;

namespace AruCareApi.ViewModels
{
    public class PatientViewModel
    {
        public Guid IdPatient { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        [DisplayName("Id Card or Passport")]
        public string IdPassport { get; set; }
        public DateTime BirthdDay { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string LocalAddress { get; set; }
        public string GPS { get; set; }

        [DisplayName("Language")]
        public Guid IdLanguage { get; set; }

        [DisplayName("Country")]
        public int IdCountry { get; set; }
    }
}