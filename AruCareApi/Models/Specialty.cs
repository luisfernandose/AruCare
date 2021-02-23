using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AruCareApi.Models
{
    [Table("Specialty")]
    public class Specialty
    {
        [Key]
        public Guid IdSpeciality { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }       
    }
}