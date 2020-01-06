using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebAppCoin_TestAssignment.Services
{
    interface ICRUDService<T>
    {
        bool Create(T item, ModelStateDictionary state);
        bool Update(T existItem, T item, ModelStateDictionary state);
        bool Delete(T item, ModelStateDictionary state);
        T Detail(T item);
        void ValidateCode(T item, ModelStateDictionary state);
        void ValidateCategory(T item, ModelStateDictionary state);
    }
}
