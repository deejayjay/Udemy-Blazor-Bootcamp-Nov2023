using Blazored.LocalStorage;
using Tangy_Common;
using TangyWeb_Client.Service.IService;
using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Service
{
    public class CartService : ICartService
    {
        public event Action? OnChange;

        private readonly ILocalStorageService _localStorageService;

        public CartService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task DecrementCart(ShoppingCart cartToDecrement)
        {
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(Sd.ShoppingCart);
            
            for (int i = 0; i < cart.Count; i++)
            {
                // If cart count less than count in cartToDecrement, remove the item from the cart
                if (cart[i].ProductId == cartToDecrement.ProductId 
                    && cart[i].ProductPriceId == cartToDecrement.ProductPriceId)
                {
                    if (cart[i].Count == 1 || cartToDecrement.Count == 0)
                    {
                        cart.Remove(cart[i]);
                    }
                    else
                    {
                        cart[i].Count -= cartToDecrement.Count;
                    }                                        
                }
            }           

            await _localStorageService.SetItemAsync(Sd.ShoppingCart, cart);
            OnChange?.Invoke();
        }

        public async Task IncrementCart(ShoppingCart cartToAdd)
        {
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(Sd.ShoppingCart);
            var itemIsInCart = false;

            if (cart is null)
            {
                cart = new List<ShoppingCart>();
            }

            foreach (var item in cart)
            {
                if (item.ProductId == cartToAdd.ProductId && item.ProductPriceId == cartToAdd.ProductPriceId)
                {
                    item.Count += cartToAdd.Count;
                    itemIsInCart = true;
                }
            }

            if (!itemIsInCart) 
            {
                cart.Add(new ShoppingCart 
                { 
                    ProductId = cartToAdd.ProductId,
                    ProductPriceId = cartToAdd.ProductPriceId,
                    Count = cartToAdd.Count
                });
            }

            await _localStorageService.SetItemAsync(Sd.ShoppingCart, cart);
            OnChange?.Invoke();
        }
    }
}
