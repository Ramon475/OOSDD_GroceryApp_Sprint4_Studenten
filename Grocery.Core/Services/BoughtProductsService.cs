using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            var result = new List<BoughtProducts>();
            if (productId is null) return result;

            var product = _productRepository.Get(productId.Value);
            if (product is null) return result;

            foreach (var item in _groceryListItemsRepository.GetAll().Where(i => i.ProductId == productId.Value))
            {
                if (_groceryListRepository.Get(item.GroceryListId) is not GroceryList list ||
                    _clientRepository.Get(list.ClientId)            is not Client client)
                {
                    continue;
                }

                result.Add(new BoughtProducts(client, list, product));
            }

            return result;

            return result;
        }

    }
}