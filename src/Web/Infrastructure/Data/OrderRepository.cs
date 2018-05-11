using System;
using System.Collections.Generic;
using ApplicationCore.Entities.OrderAggregate;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Data.Starcounter;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Infrastructure.Data
{
    public class OrderRepository : StarcounterRepository<Order>, IOrderRepository
    {
        public Order GetByIdWithItems(ulong id)
        {
            var pocoOrder = DbObjectToPoco(Db.FromId<Order>(id));

            pocoOrder.OrderItems = DbLinq.Objects<OrderItem>()
                .Where(item => item.OrderId == id)
                .ToList()
                .Select(DbOrderItemToPoco)
                .ToList();

            return pocoOrder;
        }

        private OrderItem DbOrderItemToPoco(OrderItem dbOrderItem)
        {
            var orderItemId = dbOrderItem.GetObjectNo();
            var pocoOrderItem = Mapper.Map<OrderItem>(dbOrderItem);
            pocoOrderItem.IntId = orderItemId;
            pocoOrderItem.ItemOrdered = DbItemOrderedToPoco(Db.FromId<CatalogItemOrdered>(pocoOrderItem.ItemOrderedId));
            return pocoOrderItem;
        }

        private CatalogItemOrdered DbItemOrderedToPoco(CatalogItemOrdered dbItemOrdered)
        {
            return Mapper.Map<CatalogItemOrdered>(dbItemOrdered);
        }
    }
}
