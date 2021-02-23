using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AruCareApi.Models
{
    [Table("Chat")]
    public class Chat
    {
        [Key]
        public Guid IdChat { get; set; }
        public virtual Appointment Appointment { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}