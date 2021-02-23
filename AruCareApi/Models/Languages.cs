using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AruCareApi.Models
{
    [Table("Languages")]
    public class Languages
    {
        [Key]
        public Guid IdLanguage { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}