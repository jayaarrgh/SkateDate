using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkateDate.Models.PostsViewModel
{
    public class NewPostViewModel
    {
        public string Message { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}