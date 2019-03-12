using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntervieweeService.Models
{
        public class Interviewee
        {
            public int id { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string email { get; set; }
            public string university { get; set; }
            public string githublink { get; set; }
            public string bamboolink { get; set; }
            public int? backendnote { get; set; }
            public int? frontend { get; set; }
            public int? algorithms { get; set; }
            public string specialnote { get; set; }
           
            public List<ExtraNote> extranotes { get; set; }
        }
}