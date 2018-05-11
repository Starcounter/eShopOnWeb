using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Interfaces;
using Microsoft.eShopWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Services
{
    public class BasketViewModelService : IBasketViewModelService
    {
        private readonly IRepository<Basket> _basketRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IRepository<CatalogItem> _itemRepository;

        public BasketViewModelService(IRepository<Basket> basketRepository,
            IRepository<CatalogItem> itemRepository,
            IUriComposer uriComposer)
        {
            _basketRepository = basketRepository;
            _uriComposer = uriComposer;
            _itemRepository = itemRepository;
        }

        public async Task<BasketViewModel> GetOrCreateBasketForUser(string userName)
        {
            var basketSpec = new BasketWithItemsSpecification(userName);
            var basket = (_basketRepository.List(basketSpec)).FirstOrDefault();

            if(basket == null)
            {
                return CreateBasketForUser(userName);
            }
            return CreateViewModelFromBasket(basket);
        }

        private BasketViewModel CreateViewModelFromBasket(Basket basket)
        {
            var viewModel = new BasketViewModel();
            viewModel.Id = basket.IntId;
            viewModel.BuyerId = basket.BuyerId;
            viewModel.Items = basket.Items.Select(i =>
            {
                var itemModel = new BasketItemViewModel()
                {
                    Id = i.IntId,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    CatalogItemId = i.CatalogItemId

                };
                var item = _itemRepository.GetById(i.CatalogItemId);
                itemModel.PictureUrl = _uriComposer.ComposePicUri(item.PictureUri);
                itemModel.ProductName = item.Name;
                return itemModel;
            })
                            .ToList();
            return viewModel;
        }

        private BasketViewModel CreateBasketForUser(string userId)
        {
            var basket = new Basket() { BuyerId = userId };

            var basketViewModel = new BasketViewModel()
            {
                BuyerId = basket.BuyerId,
                Id = basket.IntId,
                Items = new List<BasketItemViewModel>()
            };

            _basketRepository.Add(basket);
            return basketViewModel;
        }
    }
}
