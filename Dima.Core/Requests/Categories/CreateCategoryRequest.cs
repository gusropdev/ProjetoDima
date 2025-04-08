using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Dima.Core.Requests.Categories;

public class CreateCategoryRequest : BaseRequest
{
    [Required(ErrorMessage = "The field Title is required")]
    [MaxLength(60, ErrorMessage = "The field Title must have less than 60 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "The field Description is required")]
    public string Description { get; set; } = string.Empty;
}