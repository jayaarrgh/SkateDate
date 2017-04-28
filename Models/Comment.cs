using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkateDate.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Author { get; set; }
        public string TheComment { get; set; }
        public DateTime ComCreated { get; set; }

        public int PostID { get; set; }
        public Post Post { get; set; }

        public Comment()
        {
            ComCreated = DateTime.Now;
        }
    }
}