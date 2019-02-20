using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntervieweeService.Models
{
    public class ExtraNote
    {
        public int id { get; set; }
        public int intervieweeid { get; set; }
        public string columnname { get; set; }
        public string note { get; set; }
    }
}