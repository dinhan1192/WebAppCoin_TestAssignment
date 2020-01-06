using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppCoin_TestAssignment.Models;
using static WebAppCoin_TestAssignment.Models.Coin;

namespace WebAppCoin_TestAssignment.Services
{
    public class SQLCoinService : ICRUDService<Coin>
    {
        private MyDbContext db = new MyDbContext();
        public bool Create(Coin item, ModelStateDictionary state)
        {
            //var errors = state.Values.SelectMany(v => v.Errors);
            item.Code = item.BaseAsset + "_" + item.QuoteAsset + "_" + item.MarketId;
            ValidateCode(item, state);
            if (state.IsValid)
            {
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = null;
                item.Status = CoinStatus.NotDeleted;
                db.Coins.Add(item);
                db.SaveChanges();
                return true;
            }

            return false;

            //try
            //{
            //    item.Code = item.BaseAsset + "_" + item.QuoteAsset + "_" + item.MarketId;
            //    item.CreatedAt = DateTime.Now;
            //    item.UpdatedAt = null;
            //    item.Status = CoinStatus.NotDeleted;
            //    db.Coins.Add(item);
            //    db.SaveChanges();
            //    return true;
            //}
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
        }

        public bool Delete(Coin item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                item.Status = CoinStatus.Deleted;
                item.UpdatedAt = DateTime.Now;
                db.Coins.AddOrUpdate(item);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public Coin Detail(Coin item)
        {
            throw new NotImplementedException();
        }

        public bool Update(Coin existItem, Coin item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                existItem.BaseAsset = item.BaseAsset;
                existItem.QuoteAsset = item.QuoteAsset;
                existItem.LastPrice = item.LastPrice;
                existItem.Volumn24h = item.Volumn24h;
                existItem.UpdatedAt = DateTime.Now;
                db.Coins.AddOrUpdate(existItem);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public void ValidateCategory(Coin item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCode(Coin item, ModelStateDictionary state)
        {
            var list = db.Coins.Where(s => s.Code.Contains(item.Code)).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Coin Code already exist.");
            }
        }
    }
}