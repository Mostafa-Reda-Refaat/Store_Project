using Store.Data.Context;
using Store.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext context;

        public IProductRepository ProductRepository { get; set; }
        public IProductBrandRepository ProductBrandRepository { get; set; }
        public IProductTypeRepository ProductTypeRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IOrderItemRepository OrderItemRepository { get; set; }
        public IDeliveryMethodRepository DeliveryMethodRepository { get; set; }

        public UnitOfWork(StoreDbContext context)
        {
            this.context = context;
            ProductRepository = new ProductRepository(context);
            ProductBrandRepository = new ProductBrandRepository(context);
            ProductTypeRepository = new ProductTypeRepository(context);
            DeliveryMethodRepository = new DeliveryMethodRepository(context);
            OrderRepository = new OrderRepository(context);
            OrderItemRepository = new OrderItemRepository(context);
        }

        public int Complete()
        {
            return context.SaveChanges();
        }
    }
}
