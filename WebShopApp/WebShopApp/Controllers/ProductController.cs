using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.Brand;
using WebShopApp.Models.Category;
using WebShopApp.Core.Contracts;
using WebShopApp.Models.Product;
using WebShopApp.Infrastructure.Data.Entities;

namespace WebShopApp.Controllers
{
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

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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
            return View(updateProduct);
        }

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

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
