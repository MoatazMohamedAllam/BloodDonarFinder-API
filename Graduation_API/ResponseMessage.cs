using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Graduation_API
{
    public class ResponseMessage
    {
        public int ErrorID { get; set; }

        public string ErrorMessage { get; set; }

        public long UserID { get; set; }
    }
}