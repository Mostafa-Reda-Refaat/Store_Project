using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public IProductRepository ProductRepository { get; set; }
        public IProductBrandRepository ProductBrandRepository { get; set; }
        public IProductTypeRepository ProductTypeRepository { get; set; }
        public IDeliveryMethodRepository DeliveryMethodRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IOrderItemRepository OrderItemRepository { get; set; }

        public int Complete();
    }
}
