using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace SkateDate.Models.FriendViewModels
{
    public class ViewUsersViewModel
    {
        public List<ApplicationUser> Users { get; set; }

        public ViewUsersViewModel() { }
    }
}