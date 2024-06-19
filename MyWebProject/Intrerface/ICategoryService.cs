using MyWebProject.Models;
using MyWebProject.ViewModel;

namespace MyWebProject.Intrerface
{
    public interface ICategoryService
    {
        // Retrieves all categories from the data source
        IEnumerable<Category> GetAllCategories();

        // Retrieves subcategories for a given category ID from the data source
        IEnumerable<SubCategory> GetSubCategories(int categoryId);

        // Creates a new category asynchronously
        Task CreateCategory(CategoryCreate category);

        // Creates a new subcategory asynchronously
        Task CreateSubCategory(SubCreate subCategory);
    }


}
