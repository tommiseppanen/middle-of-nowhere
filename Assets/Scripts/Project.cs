using System;
using System.Collections.Generic;

namespace MiddleOfNowhere.Scripts
{
    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Technologies { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Published { get; set; }
    }
}


