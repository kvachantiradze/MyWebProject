using MyWebProject.Date;
using MyWebProject.Intrerface;
using MyWebProject.Models;
using MyWebProject.ViewModel;

namespace MyWebProject.services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        // Retrieves all categories from the database
        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        // Retrieves subcategories of a specified category from the database
        public IEnumerable<SubCategory> GetSubCategories(int categoryId)
        {
            return _context.SubCategories.Where(s => s.CategoryId == categoryId).ToList();
        }

        // Creates a new category asynchronously
        public async Task CreateCategory(CategoryCreate category)
        {
            if (category != null)
            {
                Category newCategory = new Category
                {
                    CategoryName = category.CategoryName,
                };

                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
            }
        }

        // Creates a new subcategory asynchronously
        public async Task CreateSubCategory(SubCreate subCategory)
        {
            if (subCategory != null)
            {
                SubCategory newSubCategory = new SubCategory
                {
                    SubCategoryName = subCategory.SubCategoryname,
                    CategoryId = subCategory.Categoryid,
                };

                _context.SubCategories.Add(newSubCategory);
                await _context.SaveChangesAsync();
            }
        }
    }




}


