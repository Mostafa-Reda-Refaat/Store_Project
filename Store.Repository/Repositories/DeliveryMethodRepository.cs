using Store.Data.Context;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
    public class DeliveryMethodRepository : GenericRepository<DeliveryMethod, int>, IDeliveryMethodRepository
    {
        public DeliveryMethodRepository(StoreDbContext context) : base(context)
        {
        }
    }
}
