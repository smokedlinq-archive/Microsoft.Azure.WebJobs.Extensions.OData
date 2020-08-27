using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Company.Function
{
    public class Department
    {
        private readonly List<Product> _products = new List<Product>();

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products => _products;
    }
}