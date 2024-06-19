using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebProject.Models;

namespace MyWebProject.ViewModel
{
    public class ProductCreateModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
        public string Description { get; set; }

        public IFormFile Photo { get; set; }

        public int SubCategoryId { get; set; }
        public int SelectedCategoryId { get; set; }
      


    }

}
