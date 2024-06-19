using MyWebProject.Date;
using MyWebProject.Intrerface;
using MyWebProject.Models;
using MyWebProject.ViewModel;

namespace MyWebProject.services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductService(AppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // Retrieves a list of products based on search criteria, category, subcategory, price range, and pagination
        public IEnumerable<AllProduct> GetAllProducts(string searchstring, string category, string subcategory, decimal? min, decimal? max, int page, int pageSize)
        {
            var products = _context.AllProducts.AsQueryable();

            // Apply filters based on search string, category, subcategory, min and max price
            if (!string.IsNullOrEmpty(searchstring))
            {
                products = products.Where(p => p.ProductName.Contains(searchstring));
            }

            if (!string.IsNullOrEmpty(category) && int.TryParse(category, out int categoryId) && categoryId > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(subcategory) && int.TryParse(subcategory, out int subcategoryId) && subcategoryId > 0)
            {
                products = products.Where(p => p.SubCategoryId == subcategoryId);
            }

            if (min != null)
            {
                products = products.Where(p => p.Price >= min);
            }

            if (max != null)
            {
                products = products.Where(p => p.Price <= max);
            }

            // Calculate total number of items after filtering
            int totalItems = products.Count();

            // Apply pagination
            products = products.Skip((page - 1) * pageSize).Take(pageSize);

            return products.ToList();
        }

        // Retrieves a product by its ID
        public AllProduct GetProduct(int id)
        {
            return _context.AllProducts.FirstOrDefault(p => p.ProductId == id);
        }

        // Creates a new product based on the provided model
        public void CreateProduct(ProductCreateModel model)
        {
            if (model != null)
            {
                // Process uploaded file and get unique file name
                string uniqueFileName = ProcessUploadedFile(model);

                // Create new product entity
                AllProduct newProduct = new AllProduct
                {
                    ProductName = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    PhotoPath = uniqueFileName,
                    CategoryId = model.SelectedCategoryId,
                    SubCategoryId = model.SubCategoryId,
                };

                // Add new product to the database
                _context.AllProducts.Add(newProduct);
                _context.SaveChanges();
            }
        }

        // Updates an existing product based on the provided model
        public void UpdateProduct(ProductEditModel model)
        {
            if (model != null)
            {
                // Find the existing product in the database
                AllProduct product = _context.AllProducts.FirstOrDefault(p => p.ProductId == model.Id);

                if (product != null)
                {
                    // Update product properties
                    product.ProductName = model.Name;
                    product.Price = model.Price;
                    product.Description = model.Description;

                    // Process uploaded file if a new photo is provided
                    if (model.Photo != null)
                    {
                        // Delete existing photo if it exists
                        if (model.ExistingPhotoPath != null)
                        {
                            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                            System.IO.File.Delete(filePath);
                        }

                        // Save new photo and update product's PhotoPath
                        product.PhotoPath = ProcessUploadedFile(model);
                    }

                    // Update product in the database
                    _context.AllProducts.Update(product);
                    _context.SaveChanges();
                }
            }
        }

        // Deletes a product from the database by its ID
        public void DeleteProduct(int id)
        {
            var product = _context.AllProducts.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                _context.AllProducts.Remove(product);
                _context.SaveChanges();
            }
        }

        // Retrieves a list of products based on the provided category ID
        public IEnumerable<AllProduct> GetProductsByCategory(int categoryId)
        {
            return _context.AllProducts.Where(p => p.CategoryId == categoryId).ToList();
        }

        // Processes uploaded file for product creation, returns unique file name
        private string ProcessUploadedFile(ProductCreateModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        // Processes uploaded file for product update, returns unique file name
        private string ProcessUploadedFile(ProductEditModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }


}
