using MyWebProject.Models;
using MyWebProject.ViewModel;

namespace MyWebProject.Intrerface
{
    public interface IProductService
    {
        // Retrieves all products based on search criteria, category, subcategory, price range, and pagination
        IEnumerable<AllProduct> GetAllProducts(string searchstring, string category, string subcategory, decimal? min, decimal? max, int page, int pageSize);

        // Retrieves details of a specific product by its ID
        AllProduct GetProduct(int id);

        // Creates a new product
        void CreateProduct(ProductCreateModel model);

        // Updates an existing product
        void UpdateProduct(ProductEditModel model);

        // Deletes a product by its ID
        void DeleteProduct(int id);

        // Retrieves products belonging to a specific category
        IEnumerable<AllProduct> GetProductsByCategory(int categoryId);
    }


}
