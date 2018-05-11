using Microsoft.eShopWeb.ApplicationCore.Entities;
using Starcounter.Nova;

namespace ApplicationCore.Entities.OrderAggregate
{

    public class OrderItem : BaseEntity
    {
        public virtual ulong ItemOrderedId { get; private set; }
        public CatalogItemOrdered ItemOrdered;
        public virtual decimal UnitPrice { get; private set; }
        public virtual int Units { get; private set; }

        protected OrderItem()
        {
        }
        public OrderItem(CatalogItemOrdered itemOrdered, decimal unitPrice, int units)
        {
            ItemOrdered = itemOrdered;
            UnitPrice = unitPrice;
            Units = units;
        }

        public virtual ulong OrderId { get; set; }
    }
}
