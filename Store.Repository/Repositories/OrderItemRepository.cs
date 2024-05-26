using Store.Data.Context;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem, Guid>, IOrderItemRepository
    {
        public OrderItemRepository(StoreDbContext context) : base(context)
        {
        }
    }
}
