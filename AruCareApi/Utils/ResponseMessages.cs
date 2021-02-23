using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AruCareApi.Utils
{
    public static class ResponseMessages
    {
        public enum Response
        {
            Successful = 1,
            Warning = 2,
            Error = 3
        }
    }
}