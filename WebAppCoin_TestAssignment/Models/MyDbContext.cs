using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebAppCoin_TestAssignment.Models
{
    public class MyDbContext : IdentityDbContext<AppUser>
    {
        public MyDbContext() : base("name=SQLContext")
        {

        }

        public static MyDbContext Create()
        {
            return new MyDbContext();
        }

        public DbSet<Market> Markets { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public System.Data.Entity.DbSet<WebAppCoin_TestAssignment.Models.AppRole> IdentityRoles { get; set; }

    }
}