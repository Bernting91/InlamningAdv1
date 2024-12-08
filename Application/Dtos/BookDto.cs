using System.ComponentModel.DataAnnotations;

public class BookDto
{
    [Required(ErrorMessage = "Id is required")]
    public required Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title can't be longer than 200 characters")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, ErrorMessage = "Description can't be longer than 1000 characters")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "AuthorId is required")]
    public required Guid AuthorId { get; set; }

    [Required(ErrorMessage = "AuthorName is required")]
    public required string AuthorName { get; set; }
}
