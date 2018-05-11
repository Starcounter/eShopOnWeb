using System;
using System.Linq;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data.Starcounter;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Infrastructure.Data
{
    public class BasketRepository: StarcounterRepository<Basket>
    {
        public override void Update(Basket basket)
        {
            Db.Transact(() => {
                var dbBasket = Db.FromId<Basket>(basket.IntId);
                var existingItems = DbLinq.Objects<BasketItem>()
                    .Where(item => item.BasketId == basket.IntId)
                    .ToList()
                    .Select(item => item.GetObjectNo())
                    .ToList();
                foreach (var newBasketItem in basket.Items.Where(item => !existingItems.Contains(item.IntId)))
                {
                    var dbBasketItem = Mapper.Map(newBasketItem, Db.Insert<BasketItem>());
                    dbBasketItem.BasketId = basket.IntId;
                }

                Mapper.Map(basket, dbBasket);
            });
        }

        protected override Basket DbObjectToPoco(Basket dbBasket)
        {
            var pocoBasket = base.DbObjectToPoco(dbBasket);
            pocoBasket.Items = DbLinq.Objects<BasketItem>()
                .Where(item => item.BasketId == pocoBasket.IntId)
                .ToList()
                .Select(item => {
                    var pocoBasketItem = Mapper.Map<BasketItem>(item);
                    pocoBasketItem.IntId = item.GetObjectNo();
                    return pocoBasketItem;
                })
                .ToList();

            return pocoBasket;
        }

        public override void Delete(Basket basket)
        {
            Db.Transact(() => {
                foreach (var basketItem in DbLinq.Objects<BasketItem>().Where(item => item.BasketId == basket.IntId))
                {
                    Db.Delete(basketItem);
                }
                Db.Delete(Db.FromId(basket.IntId));
            });
        }
    }
}