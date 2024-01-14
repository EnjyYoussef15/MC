using System.ComponentModel.DataAnnotations;

namespace MCSHiPPERS_Task.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

       
        public string? Name { get; set; }
       
        public int? Quantity { get; set; }
      
        public decimal? Price { get; set; }

        //public decimal TotalPrice
        //{
        //    get { return Price * Quantity; }
        //}
        [MaxLength(100)]
        public string? Description { get; set; }

        public string? CoverPhoto { get; set; }
    }
}
