using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Books.Queries.GetbookbyID;
using Domain;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Commands.AddBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Commands.RemoveBook;
using Application.Dtos;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookController> _logger;

        public BookController(IMediator mediator, ILogger<BookController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Description = "Retrieves a list of all books.")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Retrieving all books");
            try
            {
                var result = await _mediator.Send(new GetAllBooksQuery());
                if (!result.IsSuccessfull)
                {
                    _logger.LogWarning("No books found");
                    return NotFound(result.ErrorMessage);
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving books");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Description = "Retrieves a book by its unique ID.")]
        public async Task<ActionResult<Book>> Get(Guid id)
        {
            _logger.LogInformation("Retrieving book with ID: {BookId}", id);
            try
            {
                var result = await _mediator.Send(new GetBookByIdQuery(id));
                if (!result.IsSuccessfull)
                {
                    _logger.LogWarning("Book with ID {BookId} not found", id);
                    return NotFound(result.ErrorMessage);
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book with ID: {BookId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(Description = "Creates a new book.")]
        public async Task<ActionResult<Book>> Post([FromBody] BookDto bookToAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Adding a new book: {BookTitle}", bookToAdd.Title);
            try
            {
                var book = new Book(
                    bookToAdd.Id,
                    bookToAdd.Title,
                    bookToAdd.Description,
                    new Author(bookToAdd.AuthorId, string.Empty)
                );

                var result = await _mediator.Send(new AddBookCommand(book));
                if (!result.IsSuccessfull)
                {
                    _logger.LogWarning("Failed to add book: {ErrorMessage}", result.ErrorMessage);
                    return BadRequest(result.ErrorMessage);
                }
                return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book: {BookTitle}", bookToAdd.Title);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Description = "Updates an existing book.")]
        public async Task<IActionResult> Put(Guid id, [FromBody] BookDto bookToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating book with ID: {BookId}", id);
            try
            {
                if (id != bookToUpdate.Id)
                {
                    _logger.LogWarning("ID mismatch: {BookId} != {BookToUpdateId}", id, bookToUpdate.Id);
                    return BadRequest("ID mismatch.");
                }

                var book = new Book(
                    bookToUpdate.Id,
                    bookToUpdate.Title,
                    bookToUpdate.Description,
                    new Author(bookToUpdate.AuthorId, string.Empty)
                );

                var result = await _mediator.Send(new UpdateBookCommand(bookToUpdate.Id, book));
                if (!result.IsSuccessfull)
                {
                    _logger.LogWarning("Failed to update book with ID: {BookId}", id);
                    return NotFound(result.ErrorMessage);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book with ID: {BookId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Description = "Deletes a book by its unique ID.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting book with ID: {BookId}", id);
            try
            {
                var result = await _mediator.Send(new GetBookByIdQuery(id));
                if (!result.IsSuccessfull)
                {
                    _logger.LogWarning("Book with ID {BookId} not found", id);
                    return NotFound(result.ErrorMessage);
                }

                var deleteResult = await _mediator.Send(new RemoveBookCommand(id));
                if (!deleteResult.IsSuccessfull)
                {
                    _logger.LogWarning("Failed to delete book with ID: {BookId}", id);
                    return BadRequest(deleteResult.ErrorMessage);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID: {BookId}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}