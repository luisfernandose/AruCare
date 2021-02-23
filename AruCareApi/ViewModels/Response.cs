using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace AruCareApi.ViewModels
{
    public class Response
    {
        public int RespondeCode { get; set; }
        public string Message { get; set; }
        public object Object { get; set; }
        public ModelStateDictionary ModelState { get; set; }
    }
}