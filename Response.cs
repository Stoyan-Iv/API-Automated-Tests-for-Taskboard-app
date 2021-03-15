using System;
using System.Collections.Generic;
using System.Text;

namespace TaskBoard_System_Automated_API_Tests
{
    public class Response
    {
        public int id { get; set; }
        public string title { get; set; }
     
        public string description { get; set; }
        public List<Board> board { get; set; }
        public string dateCreated { get; set; }
        public string dateModified { get; set; }

    }
}
