using ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Starcounter.Nova;

namespace ApplicationCore.Entities.BuyerAggregate
{
//    [Database]
    public class Buyer : BaseEntity, IAggregateRoot
    {
        public virtual string IdentityGuid { get; private set; }


        public IReadOnlyCollection<PaymentMethod> PaymentMethods;

        protected Buyer()
        {
        }

        public Buyer(string identity) : this()
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
        }
    }
}
