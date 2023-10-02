using ArikteeFoods.API.Entities;
using ArikteeFoods.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Extensions
{
    public static class DtoConversion
    {
        public static ProductDto ConvertToDto(this Product product)
        {
            var productDto = new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductImageUrl = product.ProductImageUrl,
                ProductUnits = (from productUnit in product.ProductUnits
                                select new ProductUnitDto
                                {
                                    Id = productUnit.Id,
                                    Unit = productUnit.Unit,
                                    Price = productUnit.UnitPrice
                                }).ToList(),
            };
            return productDto;
        }

        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            var productDtos = from product in products
                              select new ProductDto
                              {
                                  Id = product.Id,
                                  ProductName = product.ProductName,
                                  ProductDescription = product.ProductDescription,
                                  ProductImageUrl = product.ProductImageUrl,
                                  ProductUnits = (from productUnit in product.ProductUnits
                                                  select new ProductUnitDto
                                                  {
                                                      Id = productUnit.Id,
                                                      Unit = productUnit.Unit,
                                                      Price = productUnit.UnitPrice
                                                  }).ToList(),
                              };
            return productDtos;
        }

        public static UserDto ConvertToDto(this User user)
        {
            return new UserDto
            {
                UserId = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                PhoneNo = user.PhoneNo,
                PasswordHash = user.Passwordhash,
                DeliveryAddresses = (from address in user.DeliveryAddresses
                                    select new AddressDto
                                    {
                                        City = address.City,
                                        HouseAddress = address.HouseAddress
                                    }).ToList(),
            };
        }

        public static UserDto ConvertToDto(this User user, String token, String refreshToken)
        {
            return new UserDto
            {
                RefreshToken = refreshToken,
                Token = token,
                UserId = user.Id,
                Fullname = user.Fullname,
                Email = user.Email,
                PhoneNo = user.PhoneNo,
                PasswordHash = user.Passwordhash
            };
        }

        public static ShoppingCartItemDto ConvertToDto(this VwCartItem vwCartItem)
        {
            return new ShoppingCartItemDto
            {
                Id = vwCartItem.Id,
                CartId = vwCartItem.CartId,
                ProductId = vwCartItem.ProductId,
                Qty = vwCartItem.Qty,
                ProductName = vwCartItem.ProductName,
                ProductImageURL = vwCartItem.ProductImageUrl,
                ProductUnitPrice = vwCartItem.UnitPrice,
                ProductUnit = vwCartItem.Unit,
                TotalPrice = vwCartItem.UnitPrice * vwCartItem.Qty,
                UserEmail = vwCartItem.Email,
                UserFullname = vwCartItem.Fullname
            };
        }

        public static async Task<IEnumerable<ShoppingCartItemDto>> ConvertToDto(this IEnumerable<VwCartItem> vwCartItems)
        {
            return (from vwCartItem in vwCartItems
                    select new ShoppingCartItemDto
                    {
                        Id = vwCartItem.Id,
                        CartId = vwCartItem.CartId,
                        ProductId = vwCartItem.ProductId,
                        Qty = vwCartItem.Qty,
                        ProductName = vwCartItem.ProductName,
                        ProductImageURL = vwCartItem.ProductImageUrl,
                        ProductUnitPrice = vwCartItem.UnitPrice,
                        ProductUnit = vwCartItem.Unit,
                        TotalPrice = vwCartItem.UnitPrice * vwCartItem.Qty,
                        UserEmail = vwCartItem.Email,
                        UserFullname = vwCartItem.Fullname
                    });
        }

        public static ShoppingCartDto ConvertToDto(this Cart cart)
        {
            return new ShoppingCartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TransDate = cart.TransDate,
                TransStatus = cart.TransStatus,
                AuthorizationUrl = cart.AuthorizationUrl,
                TransReference = cart.TransReference,
                PaymentDate = cart.PaymentDate,
                UserEmail = cart.User.Email,
                UserFullname = cart.User.Fullname
            };
        }

        public static IEnumerable<ShoppingCartDto> ConvertToDto(this IEnumerable<Cart> carts)
        {
            return (from cart in carts
                    select new ShoppingCartDto
                    {
                        Id = cart.Id,
                        UserId = cart.UserId,
                        TransDate = cart.TransDate,
                        TransStatus = cart.TransStatus,
                        AuthorizationUrl = cart.AuthorizationUrl,
                        TransReference = cart.TransReference,
                        PaymentDate = cart.PaymentDate,
                        UserEmail = cart.User.Email,
                        UserFullname = cart.User.Fullname
                    });
        }
    }
}
