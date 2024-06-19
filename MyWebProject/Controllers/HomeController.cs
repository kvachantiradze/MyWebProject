using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyWebProject.Date;
using MyWebProject.Intrerface;
using MyWebProject.Models;
using MyWebProject.ViewModel;
using MyWebProject.services;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace MyWebProject.Controllers
{
    public class HomeController : Controller
    {
        // Dependency Injection of various services
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        // Constructor to inject the services
        public HomeController(IShoppingCartService shoppingCartService, IOrderService orderService, IProductService productService, ICategoryService categoryService)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
            _productService = productService;
            _categoryService = categoryService;
        }

        // Action to display shopping cart
        [Authorize]
        [HttpGet]
        public ViewResult ShoppingCart()
        {
            var cart = _shoppingCartService.GetCart();
            return View(cart);
        }

        // Action to get cart item count as JSON
        [HttpGet]
        public JsonResult GetCartItemCount()
        {
            var cartItemCount = _shoppingCartService.GetCartItemCount();
            return Json(cartItemCount);
        }

        // Action to add a product to the shopping cart
        [Authorize]
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            _shoppingCartService.AddToCart(productId, quantity);
            return RedirectToAction("Index");
        }

        // Action to remove a product from the shopping cart
        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            _shoppingCartService.RemoveFromCart(productId);
            return RedirectToAction("ShoppingCart");
        }

        // Action to update the quantity of a product in the shopping cart
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            _shoppingCartService.UpdateQuantity(productId, quantity);
            return RedirectToAction("ShoppingCart");
        }

        // Action to display orders within a date range
        [HttpGet]
        public IActionResult Orders(DateTime? startDate, DateTime? endDate)
        {
            var orders = _orderService.GetOrders(startDate, endDate);
            return View(orders);
        }

        // Action to place an order
        [HttpPost]
        public IActionResult PlaceOrder()
        {
            _orderService.PlaceOrder();
            return RedirectToAction("Index");
        }

        // Action to display details of a specific order
        public IActionResult OrderDetails(int id)
        {
            var order = _orderService.GetOrderDetails(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // Action to delete an order
        [HttpPost]
        public IActionResult DeleteOrder(int orderId)
        {
            _orderService.DeleteOrder(orderId);
            return RedirectToAction("Orders");
        }

        // Action to display products with pagination and search/filtering
        [HttpGet]
        public ViewResult Index(string searchstring, string category, string subcategory, decimal? min, decimal? max, int page = 1, int pageSize = 10)
        {
            // Retrieve filtered and paginated list of products
            var products = _productService.GetAllProducts(searchstring, category, subcategory, min, max, page, pageSize);

            // Get the total number of filtered products
            int totalItems = _productService.GetAllProducts(searchstring, category, subcategory, min, max, page: 1, pageSize: int.MaxValue).Count();

            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Pass categories and subcategories to the view
            ViewBag.Categories = _categoryService.GetAllCategories();
            ViewBag.SubCategories = _categoryService.GetSubCategories(0);

            // Pass pagination info to the view
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(products);
        }

        // Action to search products by category
        public IActionResult SearchByCategory(int categoryId)
        {
            var products = _productService.GetProductsByCategory(categoryId);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // Action to get subcategories as JSON based on a category
        public IActionResult GetSubcategories(int categoryId)
        {
            var subcategories = _categoryService.GetSubCategories(categoryId);
            return Json(subcategories);
        }

        // Action to display form for creating a new product
        [HttpGet]
        public ViewResult CreateProduct()
        {
            var categories = _categoryService.GetAllCategories();
            ViewBag.Categories = categories;
            var subcategories = _categoryService.GetSubCategories(0);
            ViewBag.SubCategories = subcategories;

            return View();
        }

        // Action to handle submission of new product creation form
        [HttpPost]
        public IActionResult CreateProduct(ProductCreateModel model)
        {
            if (ModelState.IsValid)
            {
                _productService.CreateProduct(model);
                return RedirectToAction("Index", "Home");
            }

            var subcategories = _categoryService.GetSubCategories(0);
            ViewBag.SubCategories = subcategories;
            var categories = _categoryService.GetAllCategories();
            ViewBag.Categories = categories;
            return View(model);
        }

        // Action to display form for editing a product
        [HttpGet]
        public ViewResult Edit(int id)
        {
            AllProduct product = _productService.GetProduct(id);
            ProductEditModel editProduct = new ProductEditModel
            {
                Id = product.ProductId,
                Name = product.ProductName,
                Price = product.Price,
                Description = product.Description,
                ExistingPhotoPath = product.PhotoPath,
            };
            return View(editProduct);
        }

        // Action to handle submission of product edit form
        [HttpPost]
        public IActionResult Edit(ProductEditModel model)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(model);
                return RedirectToAction("index");
            }
            return View(model);
        }

        // Action to display details of a product
        [HttpGet]
        public ViewResult Details(int id)
        {
            DetailViewModel detailsViewModel = new DetailViewModel()
            {
                allProduct = _productService.GetProduct(id),
                PageTitle = "Product Details"
            };

            return View(detailsViewModel);
        }

        // Action to delete a product
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        // Action to display form for creating a new category
        public IActionResult CreateCategory()
        {
            return View();
        }

        // Action to handle submission of new category creation form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryCreate category)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategory(category);
                return RedirectToAction("Index", new { id = category.CategoryName });
            }
            return View(category);
        }

        // Action to display form for creating a new subcategory
        public IActionResult CreateSub()
        {
            var categories = _categoryService.GetAllCategories();
            if (categories != null && categories.Any())
            {
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            }
            else
            {
                ViewBag.ErrorMessage = "No categories available. Please create categories first.";
            }

            return View();
        }

        // Action to handle submission of new subcategory creation form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSub(SubCreate subCategory)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.CreateSubCategory(subCategory);
                return RedirectToAction("Index", "Home");
            }

            var categories = _categoryService.GetAllCategories();
            ViewBag.Categories = categories;
            return View(subCategory);
        }
    }


}























