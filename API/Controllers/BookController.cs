using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Books.Queries.GetbookbyID;
using Domain;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Commands.AddBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Commands.RemoveBook;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(Description = "Retrieves a list of all books.")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _mediator.Send(new GetAllBooksQuery());
                if (!result.IsSuccessfull)
                {
                    return NotFound(result.ErrorMessage);
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Description = "Retrieves a book by its unique ID.")]
        public async Task<ActionResult<Book>> Get(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetBookByIdQuery(id));
                if (!result.IsSuccessfull)
                {
                    return NotFound(result.ErrorMessage);
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(Description = "Creates a new book.")]
        public async Task<ActionResult<Book>> Post([FromBody] Book bookToAdd)
        {
            try
            {
                var result = await _mediator.Send(new AddBookCommand(bookToAdd));
                if (!result.IsSuccessfull)
                {
                    return BadRequest(result.ErrorMessage);
                }
                return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Description = "Updates an existing book.")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Book bookToUpdate)
        {
            try
            {
                if (id != bookToUpdate.Id)
                {
                    return BadRequest("ID mismatch.");
                }

                var result = await _mediator.Send(new UpdateBookCommand(bookToUpdate.Id, bookToUpdate));
                if (!result.IsSuccessfull)
                {
                    return NotFound(result.ErrorMessage);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Description = "Deletes a book by its unique ID.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetBookByIdQuery(id));
                if (!result.IsSuccessfull)
                {
                    return NotFound(result.ErrorMessage);
                }

                var deleteResult = await _mediator.Send(new RemoveBookCommand(id));
                if (!deleteResult.IsSuccessfull)
                {
                    return BadRequest(deleteResult.ErrorMessage);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}