using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.Brand;
using WebShopApp.Models.Category;
using WebShopApp.Core.Contracts;
using WebShopApp.Models.Product;
using WebShopApp.Infrastructure.Data.Entities;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Authorization;

namespace WebShopApp.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class ProductController : Controller
    {
       private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        public ProductController(IProductService productService, ICategoryService categoryService, IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
        }
        [AllowAnonymous]
        public ActionResult Index( string searchStringCategoryName, string searchStringBrandName)
        {
       List<ProductIndexVM> products = _productService.GetProducts(searchStringCategoryName,searchStringBrandName).Select(product=>new ProductIndexVM
       {
           Id= product.Id,
           ProductName=product.ProductName,
           BrandId= product.BrandId,
           BrandName=product.Brand.BrandName,
           CategoryId= product.CategoryId,
           CategoryName=product.Category.CategoryName,
           Picture=product.Picture,
           Quantity=product.Quantity,
           Price=product.Price,
           Discount=product.Discount,

       }).ToList();
            return View(products);  
        }
        [AllowAnonymous]
        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
           Product item=_productService.GetProductById(id);
            if(item== null)
            {
                return NotFound();
            }
            ProductDetailsVM product = new ProductDetailsVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                BrandId = item.BrandId,
                BrandName = item.Brand.BrandName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount,
            };
            return View(product);
        }
        [AllowAnonymous]
        // GET: ProductController/Create
        public ActionResult Create()
        {
            var product = new ProductCreateVM();
            product.Brands=_brandService.GetBrands()
            .Select(x=>new BrandPairVM()
            {
                Id= x.Id,   
                Name=x.BrandName
            }).ToList();
            product.Categories=_categoryService.GetCategories().Select(x=>new CategoryPairVM()
            {
                Id=x.Id,
                Name =x.CategoryName
            }).ToList() ;
            return View(product);
        }
        [AllowAnonymous]
        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] ProductCreateVM product)
        {
            if (ModelState.IsValid)
            {
                var createId = _productService.Create(product.ProductName, product.BrandId, product.CategoryId, product.Picture,
                    product.Quantity, product.Price, product.Discount);
                if (createId)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        [AllowAnonymous]
        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
           Product product=_productService.GetProductById(id);
            if(product == null)
            {
                return NotFound();
            }
            ProductEditVM updateProduct = new ProductEditVM()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                Picture = product.Picture,
                Quantity = product.Quantity,
                Price = product.Price,
                Discount = product.Discount
            };
            updateProduct.Brands = _brandService.GetBrands().Select(b => new BrandPairVM()
            {
                Id = b.Id,
                Name = b.BrandName
            }).ToList();
            updateProduct.Categories = _categoryService.GetCategories().Select(b => new CategoryPairVM()
            {
                Id = b.Id,
                Name = b.CategoryName
            }).ToList();
            return View(updateProduct);
        }
        [AllowAnonymous]
        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEditVM product)
        {
            if (ModelState.IsValid)
            {
                var updated = _productService.Update(id, product.ProductName, product.BrandId, product.CategoryId, product.Picture, product.Quantity, product.Price, product.Discount);
                if (updated)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(product);   
        }
        [AllowAnonymous]
        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
           Product item=_productService.GetProductById(id);
            if (item == null)
            {
                return NotFound();  
            }
            ProductDeleteVM product = new ProductDeleteVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                BrandName = item.Brand.BrandName,
                BrandId = item.BrandId,
                CategoryName = item.Category.CategoryName,
                CategoryId = item.CategoryId,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };
            return View(product);
          
        }
        [AllowAnonymous]
        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _productService.RemoveById(id);
            if (deleted)
            {
                return RedirectToAction("Success");
            }
            else
            {
                return View();
            }
        }
        [AllowAnonymous]
        public IActionResult Success()
        {
            return View();
        }
    }
}
