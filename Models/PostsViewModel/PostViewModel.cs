using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkateDate.Models.PostsViewModel
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public Comment NewComment { get; set; }
        public List<Comment> Comments { get; set; }
        //public List<Comment> EditComments { get; set; }
    }
}