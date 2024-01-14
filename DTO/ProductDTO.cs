namespace MCSHiPPERS_Task.DTO
{
    public class ProductDTO
    {
       
        public string? Name { get; set; }

        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

      
        public string? Description { get; set; }

       

        public IFormFile? CoverPhoto { get; set; }

    }
     public class ProductDTODetails : ProductDTO

    {
        public int ID { get; set; }
        public string? CoverPhotoString { get; set; }

        public decimal? TotalPrice
        {
            get { return Price * Quantity; } 
        }
    }
    public class ProductDTOUppdate : ProductDTO
    {
        public int ID { get; set; }
    }
}
