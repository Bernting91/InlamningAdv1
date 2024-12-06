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
        public async Task<IEnumerable<Book>> Get()
        {
            return await _mediator.Send(new GetAllBooksQuery());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Description = "Retrieves a book by its unique ID.")]
        public async Task<ActionResult<Book>> Get(Guid id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        [SwaggerOperation(Description = "Creates a new book.")]
        public async Task<ActionResult<Book>> Post([FromBody] Book bookToAdd)
        {
            var book = await _mediator.Send(new AddBookCommand(bookToAdd));
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Description = "Updates an existing book.")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Book bookToUpdate)
        {
            if (id != bookToUpdate.Id)
            {
                return BadRequest();
            }

            var updatedBook = await _mediator.Send(new UpdateBookCommand(bookToUpdate.Id, bookToUpdate));
            if (updatedBook == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Description = "Deletes a book by its unique ID.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));
            if (book == null)
            {
                return NotFound();
            }

            await _mediator.Send(new RemoveBookCommand(id));
            return NoContent();
        }
    }
}