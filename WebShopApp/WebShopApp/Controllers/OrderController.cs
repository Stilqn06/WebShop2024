using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using WebShopApp.Core.Contracts;
using WebShopApp.Infrastructure.Data.Entities;
using WebShopApp.Models.Order;

namespace WebShopApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;    
        public OrderController(IProductService productService, IOrderService orderService)
        {
            this._productService = productService;
            this._orderService = orderService;
        }

      
        public IActionResult Create(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            OrderCreateVM order = new OrderCreateVM()
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                QuantityStock = product.Quantity,
                Price = product.Price,
                Discount = product.Discount,
                Picture = product.Picture,
            };
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderCreateVM bindingModel)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = _productService.GetProductById(bindingModel.ProductId);
            if (currentUserId == null || product == null || product.Quantity < bindingModel.Quantity || product.Quantity == 0)
            {
                return RedirectToAction("Denied", "Order");
              

            }
            if (ModelState.IsValid)
            {
                _orderService.Create(bindingModel.ProductId, currentUserId, bindingModel.Quantity);

            }
            return RedirectToAction("Index", "Product");
        }
        public ActionResult Denied()
        {
            return View();
        }

        [Authorize(Roles ="Administrator")]
            public ActionResult Index()
        {
            List<OrderIndexVM>orders= _orderService.GetOrders().Select(x=>new OrderIndexVM
            {
                Id = x.Id,
                OrderDate = x.OrderDate.ToString("dd-MMM-yyyy hh:m", CultureInfo.InvariantCulture),
                UserId = x.UserId,
                User = x.User.UserName,
                ProductId = x.ProductId,
                Product = x.Product.ProductName,
                Picture = x.Product.Picture,
                Quantity = x.Quantity,
                Price = x.Price,
                Discount = x.Discount,
                TotalPrice = x.TotalPrice,
            }).ToList();
            
            return View(orders);
        }
        public ActionResult MyOrders()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<OrderIndexVM> orders = _orderService.GetOrdersByUser(currentUserId)
                .Select(x => new OrderIndexVM
                {
                   Id=x.Id,
                   OrderDate=x.OrderDate.ToString("dd-MMM-yyyy HH:m",CultureInfo.InvariantCulture),
                   UserId = x.UserId,
                   User=x.User.UserName,
                   ProductId=x.ProductId,
                   Product= x.Product.ProductName,  
                   Picture=x.Product.Picture,
                   Quantity = x.Quantity,
                   Price = x.Price,
                   Discount = x.Discount,
                   TotalPrice = x.TotalPrice,
                }).ToList();
            return View(orders);
        }
    }
}
