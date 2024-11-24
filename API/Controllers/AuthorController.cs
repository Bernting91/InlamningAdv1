using Application.Authors.Commands.AddAuthor;
using Application.Authors.Commands.RemoveAuthor;
using Application.Authors.Commands.UpdateAuthor;
using Application.Authors.Queries.GetAllAuthors;
using Application.Authors.Queries.GetAuthorByID;
using Application.Books.Commands.AddBook;
using Application.Books.Commands.RemoveBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Queries.GetbookbyID;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<AuthorController>
        [HttpGet]
        public async Task<IEnumerable<Author>> Get()
        {
            return await _mediator.Send(new GetAllAuthorsQuery());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult<Author>> Post([FromBody] Author authorToAdd)
        {
            var author = await _mediator.Send(new AddAuthorCommand(authorToAdd));
            return CreatedAtAction(nameof(Get), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Author authorToUpdate)
        {
            if (id != authorToUpdate.Id)
            {
                return BadRequest();
            }

            var updatedAuthor = await _mediator.Send(new UpdateAuthorCommand(authorToUpdate));
            if (updatedAuthor == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (author == null)
            {
                return NotFound();
            }

            await _mediator.Send(new RemoveAuthorCommand(author));
            return NoContent();
        }
    }
}
