using System.ComponentModel.DataAnnotations;

namespace ApiRest.DTO
{
    /// <summary>
    /// Class <c>Point</c> models a point in a two-dimensional plane.
    /// </summary>
    public class DtoProduct
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Range(0, 100,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double Price { get; set; }

        [Required]
        public string? SKU { get; set; }
    }
}
