using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories;

public class UpdateCategoryRequest : BaseRequest
{
    public long Id { get; set; }
    
    [Required(ErrorMessage = "The field Title is required")]
    [MaxLength(60, ErrorMessage = "The field Title must have less than 60 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "The field Description is required")]
    public string Description { get; set; } = string.Empty;
}