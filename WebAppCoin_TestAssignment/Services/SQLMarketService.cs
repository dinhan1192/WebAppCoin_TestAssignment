using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppCoin_TestAssignment.Models;
using static WebAppCoin_TestAssignment.Models.Market;

namespace WebAppCoin_TestAssignment.Services
{
    public class SQLMarketService : ICRUDService<Market>
    {
        private MyDbContext db = new MyDbContext();
        //public MyDbContext DbContext
        //{
        //    get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
        //    set { _db = value; }
        //}
        public bool Create(Market item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = null;
                item.Status = MarketStatus.NotDeleted;
                db.Markets.Add(item);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Delete(Market item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                item.Status = MarketStatus.Deleted;
                item.UpdatedAt = DateTime.Now;
                db.Markets.AddOrUpdate(item);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public Market Detail(Market item)
        {
            throw new NotImplementedException();
        }

        public bool Update(Market existItem, Market item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                existItem.Name = item.Name;
                existItem.Description = item.Description;
                existItem.UpdatedAt = DateTime.Now;
                db.Markets.AddOrUpdate(existItem);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public void ValidateCategory(Market item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCode(Market item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }
    }
}