using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AruCareApi.Models
{
    [Table("Doctors")]
    public class Doctors
    {
        [Key]
        public Guid IdDoctor { get; set; }
        public string Name { get; set; }
        //[JsonProperty("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Medical Identification Number")]
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }
        public DateTime? LastAttention { get; set; }
        public bool Status { get; set; } = true;

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}