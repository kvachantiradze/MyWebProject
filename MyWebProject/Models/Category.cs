namespace MyWebProject.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<AllProduct> Products { get; set; }
        public ICollection<SubCategory> Subcategories { get; set; }
    }
}
