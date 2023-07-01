using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.entity;

namespace shopapp.data.Concrete
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, ShopContext>, IOrderRepository
    {
    }
}