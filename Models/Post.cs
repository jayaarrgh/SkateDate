using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkateDate.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public string Author { get; set; }  // Username of author -- then use this to see who friends are
        //public string Location { get; set; }
        public string Message { get; set; }
        //public string TimeFrame { get; set; } // View Model could validate this as date time range feature indtead       
        public DateTime CreatedDate { get; set; }
        //public bool IsPrivate { get; set; } // - Can checkfor friendship if this is true, or skip if false
        public double Lat { get; set; }
        public double Lng { get; set; }

        public IList<Comment> Comments { get; set; } // Will I now need Include statements???

        public Post() // Constructor sets CreatedDate to NOW
        {
            CreatedDate = DateTime.Now;
        }
    }
}