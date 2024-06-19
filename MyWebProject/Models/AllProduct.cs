using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebProject.Models
{
    public class AllProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }




    }
}

