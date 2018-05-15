﻿using ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using Starcounter.Nova;

namespace ApplicationCore.Entities.OrderAggregate
{
//    [Database]
    public class Order : BaseEntity, IAggregateRoot
    {
        public List<OrderItem> OrderItems;

        protected Order()
        {
        }

        public Order(string buyerId, Address shipToAddress, List<OrderItem> items)
        {
            ShipToAddress = shipToAddress;
            OrderItems = items;
            BuyerId = buyerId;
        }
        public virtual string BuyerId { get; set; }

        public virtual DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;
        public virtual Address ShipToAddress { get; private set; }

        // DDD Patterns comment
        // Using a private collection field, better for DDD Aggregate's encapsulation
        // so OrderItems cannot be added from "outside the AggregateRoot" directly to the collection,
        // but only through the method Order.AddOrderItem() which includes behavior.

        // Using List<>.AsReadOnly() 
        // This will create a read only wrapper around the private list so is protected against "external updates".
        // It's much cheaper than .ToList() because it will not have to copy all items in a new collection. (Just one heap alloc for the wrapper instance)
        //https://msdn.microsoft.com/en-us/library/e78dcd75(v=vs.110).aspx 
        public decimal Total()
        {
            var total = 0m;
            foreach (var item in OrderItems)
            {
                total += item.UnitPrice * item.Units;
            }
            return total;
        }

    }
}
