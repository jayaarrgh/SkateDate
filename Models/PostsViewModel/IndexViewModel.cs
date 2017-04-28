using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkateDate.Models.PostsViewModel
{
    public class IndexViewModel
    {
        public IOrderedEnumerable<Post> Posts { set; get; }
    }
}