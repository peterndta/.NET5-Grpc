using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GrpcClient.Models;

namespace GrpcClient.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult UserDetails()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new EmployeeCRUD.EmployeeCRUDClient(channel);

            //Empty response1 = client.Insert(new TblUser()
            //{
            //    FullName = "Tome",
            //    Password = "Herry",
            //    UserId = "1233333333",
            //    RoleId = "Admin"
            //});

            //TblUser tblUser = client.SelectByID(new TblUserFilter() { UserId = "1233333333" });

            //tblUser.FullName = "Tome123";
            //tblUser.Password = "Herry123";
            //tblUser.UserId = "1233333333";
            //tblUser.RoleId = "Admin";

            //Empty response2 = client.Update(tblUser);

            TblUsers tblUsers = client.SelectAll(new Empty());

            List<UserDetails> userDetails = new List<UserDetails>();

            foreach (var user in tblUsers.Items)
            {
                UserDetails newUser = new UserDetails
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Password = user.Password,
                    RoleId = user.RoleId
                };

                userDetails.Add(newUser);
            }

            ViewBag.List = userDetails;
            ViewData["userDetails"] = userDetails;
            return View(userDetails);

        }
    }
}
