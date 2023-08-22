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
                ProductPrice = product.ProductPrice,
                ProductImageUrl = product.ProductImageUrl
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
                                  ProductPrice = product.ProductPrice,
                                  ProductImageUrl = product.ProductImageUrl
                              };
            return productDtos;
        }

        public static UserDto ConvertToDto(this User user)
        {
            return new UserDto
            {
                UserId = user.Id,
                Surname = user.Surname,
                Firstname = user.Firstname,
                Email = user.Email,
                PhoneNo = user.PhoneNo,
                DeliveryAddress = user.DeliveryAddress,
                PasswordHash = user.Passwordhash
            };
        }

        public static UserDto ConvertToDto(this User user, String token)
        {
            return new UserDto
            {
                Token = token,
                UserId = user.Id,
                Surname = user.Surname,
                Firstname = user.Firstname,
                Email = user.Email,
                PhoneNo = user.PhoneNo,
                DeliveryAddress = user.DeliveryAddress,
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
                ProductPrice = vwCartItem.ProductPrice,
                TotalPrice = vwCartItem.ProductPrice * vwCartItem.Qty,
                UserEmail = vwCartItem.Email,
                UserFirstname = vwCartItem.Firstname,
                UserSurname = vwCartItem.Surname
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
                        ProductPrice = vwCartItem.ProductPrice,
                        TotalPrice = vwCartItem.ProductPrice * vwCartItem.Qty,
                        UserEmail = vwCartItem.Email,
                        UserFirstname = vwCartItem.Firstname,
                        UserSurname = vwCartItem.Surname
                    });
        }
    }
}
