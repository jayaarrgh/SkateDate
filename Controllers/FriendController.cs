﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkateDate.Models.FriendViewModels;
using SkateDate.Data;
using SkateDate.Models;
using Microsoft.AspNetCore.Authorization;


namespace SkateDate.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        private ApplicationDbContext context;

        public FriendController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            // TODO: Add a search to the VIEW which POSTs to SearchUsers method
            List<ApplicationUser> users = context.ApplicationUsers
                .Where(u => u.UserName != User.Identity.Name).ToList();

            List<ApplicationUser> usersnotfriends = new List<ApplicationUser>();
            foreach (var user in users)
            {
                //If not friends, and neither user has requested the others friendship, display that user as a possible requestee
                if (context.FriendLists.SingleOrDefault(f => f.OwnerID
                == User.Identity.Name && f.FriendID == user.UserName) == null
                    && (context.FriendRequestLists.SingleOrDefault(l => l.OwnerID
                    == user.UserName && l.RequesterID == User.Identity.Name) == null)
                    && (context.FriendRequestLists.SingleOrDefault(l => l.OwnerID
                    == User.Identity.Name && l.RequesterID == user.UserName) == null))
                {
                    usersnotfriends.Add(user);
                }
            }
            // I could alter this model to have 4 Lists: Requesters, Requestees, Friends, and Users not yet interacted with?
            ViewUsersViewModel model = new ViewUsersViewModel()
            {
                Users = usersnotfriends
            };
            return View(model);
        }

        // Post action for "searchusers" & Index -- makes "FriendRequests"
        [HttpPost]
        public IActionResult FriendRequester(string hidden)
        {
            var user = hidden;
            var me = User.Identity.Name;
            FriendRequestList newRequester = new FriendRequestList()
            {
                OwnerID = user,
                RequesterID = me
            };
            FriendRequestsUserMade newRequestee = new FriendRequestsUserMade()
            {
                OwnerID = me,
                RequesteeID = user
            };
            context.FriendRequestLists.Add(newRequester);
            context.FriendRequestsUserMades.Add(newRequestee);
            context.SaveChanges();

            return Redirect("/Friend/RequestsIHaveMade");
        }

        public IActionResult FriendRequests()
        {
            var requests = context.FriendRequestLists.Where(l => l.OwnerID == User.Identity.Name).ToList();

            // TODO: Should Redirect to an Error page
            if (requests == null)
            {
                return Redirect("/Friend");
            }
            FriendRequestsViewModel model = new FriendRequestsViewModel() { Requests = requests };
            return View(model);
        }

        public IActionResult RequestsIHaveMade()
        {
            var requests = context.FriendRequestsUserMades.Where(l => l.OwnerID == User.Identity.Name).ToList();
            // TODO: Should redirect to an error page
            if (requests == null)
            {
                return Redirect("/Friend");
            }
            RequestsUserMadeViewModel model = new RequestsUserMadeViewModel() { Requests = requests };
            return View(model);
        }

        [HttpPost]
        public IActionResult DenyRequest(string hidden)
        {
            var them = context.FriendRequestLists.Single(f => f.OwnerID == User.Identity.Name && f.RequesterID == hidden);
            context.FriendRequestLists.Remove(them);
            var me = context.FriendRequestsUserMades.Single(f => f.OwnerID == hidden && f.RequesteeID == User.Identity.Name);
            context.FriendRequestsUserMades.Remove(me);

            // I'd like to inform this other user thier request was denied.. 
            // THINK HOW??? PREFER: -- Send a message? || Alter the FRUM action method???
            context.SaveChanges();
            return Redirect("/Friend/FriendRequests");
        }

        [HttpPost]
        public IActionResult AddFriend(string hidden)
        {
            //Should i make sure these people have even requested one or the other's friendship?
            //Else wise i feel this could be abused, but im not sure how.
            var me = User.Identity.Name;
            var them = hidden;

            FriendList MyList = new FriendList() { OwnerID = me, FriendID = them };
            FriendList TheirList = new FriendList() { OwnerID = them, FriendID = me };
            context.FriendLists.Add(MyList);
            context.FriendLists.Add(TheirList);

            var a = context.FriendRequestLists.Single(f =>
            f.OwnerID == me && f.RequesterID == them);
            var b = context.FriendRequestsUserMades.Single(f =>
            f.OwnerID == them && f.RequesteeID == me);

            context.FriendRequestLists.Remove(a);
            context.FriendRequestsUserMades.Remove(b);
            context.SaveChanges();

            return Redirect("/Friend/FriendList");
        }

        public IActionResult FriendList()
        {
            var friends = context.FriendLists.Where(u => u.OwnerID == User.Identity.Name).ToList();
            FriendsViewModel model = new FriendsViewModel()
            {
                Friends = friends
            };
            return View(model);
        }

        // POST action for FriendList to remove Friends
        [HttpPost]
        public IActionResult RemoveFriend(string hidden)
        {
            var me =  User.Identity.Name;
            var them =  hidden;

            var myflist = context.FriendLists.Single(l =>
            l.OwnerID == me && l.FriendID == them);

            var theirflist = context.FriendLists.Single(l =>
            l.OwnerID == them && l.FriendID == me);

            context.FriendLists.Remove(myflist);
            context.FriendLists.Remove(theirflist);
            context.SaveChanges();

            return Redirect("/Friend/FriendList");
        }

    }
}
