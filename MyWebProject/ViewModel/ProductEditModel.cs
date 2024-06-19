namespace MyWebProject.ViewModel
{
    public class ProductEditModel : ProductCreateModel
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
