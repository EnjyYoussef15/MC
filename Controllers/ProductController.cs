using Azure;
using MCSHiPPERS_Task.DTO;
using MCSHiPPERS_Task.Models;
using MCSHiPPERS_Task.Repository.IProduct;
using MCSHiPPERS_Task.Utilites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IO;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace MCSHiPPERS_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository <Product> _product;
        private readonly IHostingEnvironment _host;
        
        public ProductController(IProductRepository<Product> product, IHostingEnvironment host)
        {
            _product = product;
            _host = host;
           
        }
        //[HttpGet]
        //[Route("getProuctsPagination")]
        //public async Task<APIResponse> GetAllProductsPagination(int PageIndex =1, int PageSize = 10)

        //{

        //    List<Product> products = (List<Product>)await _product.GetAll();
        //    var totalCount = products.Count();
        //    if (totalCount > 0 )
        //    {
        //        List<ProductDTODetails> ProductSDTO = new List<ProductDTODetails>();
        //        foreach (var product in products)
        //        {
        //            ProductDTODetails ProductDTODetails = new()
        //            {
        //                ID = product.ID,
        //                Price = product.Price,
        //                Name = product.Name,
        //                Quantity = product.Quantity,
        //                CoverPhotoString = product.CoverPhoto,
        //                Description = product.Description,


        //            };
        //            ProductSDTO.Add(ProductDTODetails);
        //        }


        //        var page = ProductSDTO.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        //        var response = new
        //        {
        //            TotalCount = totalCount,
        //            pageIndex = PageIndex,
        //            pageSize = PageSize,
        //            Data = page,
        //        };

        //        return new APIResponse
        //        { Data = response };

        //    }

        //    else
        //    {
        //        return new APIResponse
        //        {
        //            Messages = new List<string>()
        //           {
        //               "No Data"
        //           }
        //        };
        //    }
        //}


        


        [HttpPost]
        [Route("CreateProduct")]
        public  APIResponse CreateProduct([FromForm] ProductDTO ProductDTO)
        {

            if (ModelState.IsValid)
            {
                string coverImage = SaveCoverPhoto(ProductDTO.CoverPhoto);
               
                Product product = new()
                {
                   
                    Price = ProductDTO.Price,
                    Name = ProductDTO.Name,
                    Quantity = ProductDTO.Quantity,
                    CoverPhoto = coverImage,
                    Description = ProductDTO.Description,

                   
                   
                };
                _product.Add(product);

                return new APIResponse
                {
                    Data = product,
                    Messages = new List<string>()
                    {
                       "Add Succfully"
                    }
                };
            }
            else
            {
                return new APIResponse
                {
                    Messages = new List<string>()
                   {
                       "Not Found Such Prouct"
                   }
                };
            }
           
        }

        [HttpPut]
        [Route("UpdateProduct/{productID}")]
        public async Task<APIResponse> UpdateProduct(int productID, [FromForm] ProductDTO productDTO)
        {
            var Product = await _product.GetOne(productID);
            string coverImage = SaveCoverPhoto(productDTO.CoverPhoto);
            if (Product != null)
            {

                Product.Name = productDTO.Name;

                Product.Description = productDTO.Description;
                Product.Price = productDTO.Price;
                Product.Quantity = productDTO.Quantity;

                Product.CoverPhoto = coverImage;
            }


            if (ModelState.IsValid)
            {
                _product.Update(Product);
                return new APIResponse
                {
                    Data = Product,
                    Messages = new List<string>()
                    {
                       "Updated Succfully"
                    }
                };
            }
            else
            {
                return new APIResponse
                {

                    Messages = new List<string>()
                    {
                       "Updated Failed"
                    }
                };
            }

        }

        [HttpGet]
        [Route("GetOneProduct")]
        public async Task<APIResponse> GetOneProduct(int id)
        {
            var product = await _product.GetOne(id);

            if (product != null)
            {
                ProductDTODetails productDTO = new()
                {
                  
                    Price = product.Price,
                    Name = product.Name,
                    Quantity = product.Quantity,
                    CoverPhotoString = product.CoverPhoto,
                    Description = product.Description,

                };
                productDTO.ID = id;
                return new APIResponse
                {
                    Data = productDTO
                };
            }
            else
            {
                return new APIResponse
                {
                    Messages = new List<string>()
                   {
                       "Not Found Such Prouct"
                   }
                };
            }
        }



        [HttpGet]
        [Route("getProuctsPagination")]
        public async Task<APIResponse> GetAllProductsPagination(int PageIndex = 1, int PageSize = 10)

        {

            List<Product> Allproducts = (List<Product>)await _product.GetAll();
            List<Product> products = (List<Product>)await _product.GetAll(PageIndex, PageSize);
            var totalCount = Allproducts.Count();
            if (totalCount > 0)
            {
                List<ProductDTODetails> ProductSDTO = new List<ProductDTODetails>();
                foreach (var product in products)
                {
                    ProductDTODetails ProductDTODetails = new()
                    {
                        ID = product.ID,
                        Price = product.Price,
                        Name = product.Name,
                        Quantity = product.Quantity,
                        CoverPhotoString = product.CoverPhoto,
                        Description = product.Description,


                    };
                    ProductSDTO.Add(ProductDTODetails);
                }
                var response = new
                {
                    TotalCount = totalCount,
                    pageIndex = PageIndex,
                    pageSize = PageSize,
                    Data = ProductSDTO,
                };

                return new APIResponse
                { Data = response };

            }

            else
            {
                return new APIResponse
                {
                    Messages = new List<string>()
                   {
                       "No Data"
                   }
                };
            }
        }

        [HttpGet]
        [Route("GetAllProductss")]
        public async Task<APIResponse> GetAllProductss()

        {

            List<Product> products = (List<Product>)await _product.GetAll();
            var totalCount = products.Count();
            if (totalCount > 0)
            {
                List<ProductDTODetails> ProductSDTO = new List<ProductDTODetails>();
                foreach (var product in products)
                {
                    ProductDTODetails ProductDTODetails = new()
                    {
                        ID = product.ID,
                        Price = product.Price,
                        Name = product.Name,
                        Quantity = product.Quantity,
                        CoverPhotoString = product.CoverPhoto,
                        Description = product.Description,


                    };
                    ProductSDTO.Add(ProductDTODetails);
                }

                var response = new
                {
                    TotalCount = totalCount,

                    Data = ProductSDTO,
                };
                return new APIResponse
                { Data = response };
            }
            else
            {
                return new APIResponse
                {
                    Messages = new List<string>()
                   {
                       "No Data"
                   }
                };
            }

        }


        [HttpGet]
        [Route("Search")]
        public async Task<APIResponse> Search(string? name, decimal? price)
        {
            List<Product> products = new List<Product>();

            if (name != null && price == null)
            {
                products = (List<Product>)await _product.Search(m => m.Name == name );
            }
            if (name == null && price != null)
            {
                products = (List<Product>)await _product.Search(m => m.Price == price);
            }
            if (name != null && price != null)
            {
                products = (List<Product>)await _product.Search(m => m.Name == name && m.Price == price);
            }
            if (name == null && price == null)
            {
                return new APIResponse
                {
                    Messages = new List<string>()
                   {
                       "Search by at least one value to get data"
                   }
                };
            }

            if (products.Count >0)
            {
                var result = new List<ProductDTODetails>();
                result = products.Select(u => new ProductDTODetails
                {
                    ID = u.ID,
                    Name = u.Name,
                    Price = u.Price,
                    Description = u.Description,
                    Quantity = u.Quantity,
                    CoverPhotoString = u.CoverPhoto.IsNullOrEmpty() ? null : "https://localhost:7118/ProductPhoto/" + u.CoverPhoto,
                }).ToList();

                var response = new
                {
                    TotalCount = products.Count,

                    Data = result,
                };
                return new APIResponse
                { Data = response };

            }
            else
            {
                return new APIResponse
                {
                    Messages = new List<string>()
                       {
                           "No Data"
                        }
                };
            }







        }


        [HttpDelete]
        [Route("deleteProduct/{productID}")]
        public async Task<APIResponse> DeleteProduct( int productID)
        {
            var deleteProduct = await _product.GetOne(productID); 
            if (deleteProduct != null)
            {
                _product.Delete(productID);
                  return new APIResponse
                {
                    Data = deleteProduct,
                    Messages = new List<string>()
                    {
                       "deleted Succfully"
                    }
                };
            }
            else
            {
                return new APIResponse
                {
                    Messages = new List<string>()
                   {
                       "already deleted"
                   }
                };
            }
          
        }


        

       


        [NonAction]
        public string SaveCoverPhoto(IFormFile coverImage)
        {
            if (coverImage != null)
            {
                string _wwwRootPath = Path.Combine(_host.WebRootPath, "ProductPhoto");

                string NewFileName = Guid.NewGuid().ToString() + coverImage.FileName;
                var targetFilePath = Path.Combine(_wwwRootPath, NewFileName);
                using (var stream = new FileStream(targetFilePath, FileMode.Create))
                {
                    coverImage.CopyTo(stream);

                }
                return NewFileName;
            }
            return null;
        }

    }
}

