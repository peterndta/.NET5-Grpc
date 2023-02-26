using Grpc.Core;
using GrpcServiceDemo.DataAccess;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceDemo
{
    public class EmployeeCRUDService : EmployeeCRUD.EmployeeCRUDBase
    {
        private readonly ILogger<EmployeeCRUDService> _logger;
        private DataAccess.UserManagementContext db = null;

        public EmployeeCRUDService(ILogger<EmployeeCRUDService> logger, DataAccess.UserManagementContext db)
        {
            _logger = logger;
            this.db = db;
        }

        //public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        //{
        //    return Task.FromResult(new HelloReply
        //    {
        //        Message = "Hello " + request.Name
        //    });
        //}

        public override Task<TblUsers> SelectAll(Empty requestData, ServerCallContext context)
        {
            TblUsers responseData = new TblUsers();
            var query = from emp in db.TblUsers
                        select new TblUser()
                        {
                            UserId = emp.UserId,
                            FullName = emp.FullName,
                            Password = emp.Password,
                            RoleId = emp.RoleId,
                        };
            responseData.Items.AddRange(query.ToArray());
            return Task.FromResult(responseData);
        }

        public override Task<TblUser> SelectByID(TblUserFilter requestData, ServerCallContext context)
        {
            var data = db.TblUsers.Find(requestData.UserId);
            TblUser emp = new TblUser()
            {
                UserId = data.UserId,
                FullName = data.FullName,
                Password = data.Password,
                RoleId = data.RoleId,
            };
            return Task.FromResult(emp);
        }

        public override Task<Empty> Insert(TblUser requestData, ServerCallContext context)
        {
            db.TblUsers.Add(new DataAccess.TblUser()
            {
                UserId = requestData.UserId,
                FullName = requestData.FullName,
                Password = requestData.Password,
                RoleId = requestData.RoleId,
            });
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }
         
        public override Task<Empty> Update(TblUser requestData, ServerCallContext context)
        {
            db.TblUsers.Update(new DataAccess.TblUser()
            {
                UserId = requestData.UserId,
                FullName = requestData.FullName,
                Password = requestData.Password,
                RoleId = requestData.RoleId,
            });
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }
        public override Task<Empty> Delete(TblUserFilter requestData, ServerCallContext context)
        {
            var data = db.TblUsers.Find(requestData.UserId);
            db.TblUsers.Remove(new DataAccess.TblUser()
            {
                UserId = data.UserId,
                FullName = data.FullName,
                Password = data.Password,
                RoleId = data.RoleId,
            });
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }

    }
}
