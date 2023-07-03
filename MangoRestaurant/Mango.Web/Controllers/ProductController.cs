using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO> list = new List<ProductDTO>();
            var responce = await _productService.GetAllProductsAsync<RespoceDTO>();
            if(responce != null && responce.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(responce.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var responce = await _productService.CreateProductAsync<RespoceDTO>(model);
                if (responce != null && responce.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            var responce = await _productService.GetProductByIdAsync<RespoceDTO>(productId);
            if (responce != null && responce.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responce.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var responce = await _productService.UpdateProductAsync<RespoceDTO>(model);
                if (responce != null && responce.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            var responce = await _productService.GetProductByIdAsync<RespoceDTO>(productId);
            if (responce != null && responce.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(responce.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var responce = await _productService.DeleteProductAsync<RespoceDTO>(model.ProductId);
                if (responce.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }

            return View(model);
        }
    }
}
