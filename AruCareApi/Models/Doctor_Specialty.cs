using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AruCareApi.Models
{
    [Table("Doctor_Specialty")]
    public class Doctor_Specialty
    {
        [Key, Column(Order = 0)]
        public Guid IdSpeciality { get; set; }
        [Key, Column(Order = 1)]
        public Guid IdDoctor { get; set; }

        public virtual Specialty Specialty { get; set; }
        public virtual Doctors Doctors { get; set; }
    }
}