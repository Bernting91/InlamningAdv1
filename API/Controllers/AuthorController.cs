using Application.Authors.Commands.AddAuthor;
using Application.Authors.Commands.RemoveAuthor;
using Application.Authors.Commands.UpdateAuthor;
using Application.Authors.Queries.GetAllAuthors;
using Application.Authors.Queries.GetAuthorByID;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all authors", Description = "Retrieves a list of all authors.")]
        public async Task<IEnumerable<Author>> Get()
        {
            return await _mediator.Send(new GetAllAuthorsQuery());
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get author by ID", Description = "Retrieves an author by their unique ID.")]
        public async Task<ActionResult<Author>> Get(Guid id)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add a new author", Description = "Creates a new author.")]
        public async Task<ActionResult<Author>> Post([FromBody] Author authorToAdd)
        {
            var author = await _mediator.Send(new AddAuthorCommand(authorToAdd));
            return CreatedAtAction(nameof(Get), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an author", Description = "Updates an existing author.")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Author authorToUpdate)
        {
            if (id != authorToUpdate.Id)
            {
                return BadRequest();
            }

            var updatedAuthor = await _mediator.Send(new UpdateAuthorCommand(authorToUpdate.Id, authorToUpdate.Name));
            if (updatedAuthor == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an author", Description = "Deletes an author by their unique ID.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (author == null)
            {
                return NotFound();
            }

            await _mediator.Send(new RemoveAuthorCommand(id));
            return NoContent();
        }
    }
}
