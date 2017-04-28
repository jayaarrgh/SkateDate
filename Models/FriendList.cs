using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SkateDate.Models
{
    public class FriendList
    {
        public string OwnerID { get; set; }
        public string FriendID { get; set; }
    }
}