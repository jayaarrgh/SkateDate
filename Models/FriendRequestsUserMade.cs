using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SkateDate.Models
{
    public class FriendRequestsUserMade
    {
        public string OwnerID { get; set; }

        public string RequesteeID { get; set; }
    }
}
