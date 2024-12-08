using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class AuthorDto
    {
        [Required(ErrorMessage = "Id is required")]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public required string Name { get; set; }
    }
}