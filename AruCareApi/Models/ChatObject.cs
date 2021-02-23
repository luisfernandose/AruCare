using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AruCareApi.Models
{
    public class ChatObject
    {
        public string idapointmment { get; set; }
        public string username { get; set; }
        public int usertype { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
        public string token { get; set; }
        public bool validtoken { get; set; }
    }
}
