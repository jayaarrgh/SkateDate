using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SkateDate.Models
{
    public class FriendRequestList  // Join class for FRL owners and FRL Requesters
    {
        public string OwnerID { get; set; }
        //public string Owner { get; set; }

        public string RequesterID { get; set; }
        //public string Requester { get; set; }
    }
}