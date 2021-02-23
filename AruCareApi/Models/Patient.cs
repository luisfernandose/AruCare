using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AruCareApi.Models
{
    [Table("Patient")]
    public class Patient
    {
        [Key]
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
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        [DisplayName("Language")]
        public Guid IdLanguage { get; set; }
        public virtual Languages Language { get; set; }

        [DisplayName("Country")]
        public int IdCountry { get; set; }
        public virtual Country Country { get; set; }

    }
}