using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface ICartRepository: IRepository<Cart>
    {
        void DeleteFromCart(int cartId, int productId);
        Cart GetCartByUserId(string userId);
    }
}