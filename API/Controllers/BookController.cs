using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Books.Queries.GetbookbyID;
using Domain;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Commands.AddBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Commands.RemoveBook;

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
        public async Task<IEnumerable<Book>> Get()
        {
            return await _mediator.Send(new GetAllBooksQuery());
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        
        [HttpPost]
        public async Task<ActionResult<Book>> Post([FromBody] Book bookToAdd)
        {
            var book = await _mediator.Send(new AddBookCommand(bookToAdd));
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Book bookToUpdate)
        {
            if (id != bookToUpdate.Id)
            {
                return BadRequest();
            }

            var updatedBook = await _mediator.Send(new UpdateBookCommand(bookToUpdate));
            if (updatedBook == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));
            if (book == null)
            {
                return NotFound();
            }

            await _mediator.Send(new RemoveBookCommand(book));
            return NoContent();
        }
    }
}