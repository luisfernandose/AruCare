using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AruCareApi.Models
{
    [Table("Country")]
    public class Country
    {
        [Key]
        public int IdCountry { get; set; }
        public string Name { get; set; }
        public string Demonym { get; set; }
    }
}