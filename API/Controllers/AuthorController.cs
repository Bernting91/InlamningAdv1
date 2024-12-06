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
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllAuthorsQuery());
            if (!result.IsSuccessfull)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get author by ID", Description = "Retrieves an author by their unique ID.")]
        public async Task<ActionResult<Author>> Get(Guid id)
        {
            var result = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (!result.IsSuccessfull)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add a new author", Description = "Creates a new author.")]
        public async Task<ActionResult<Author>> Post([FromBody] Author authorToAdd)
        {
            var result = await _mediator.Send(new AddAuthorCommand(authorToAdd));
            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessage);
            }
            return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an author", Description = "Updates an existing author.")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Author authorToUpdate)
        {
            if (id != authorToUpdate.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var result = await _mediator.Send(new UpdateAuthorCommand(authorToUpdate.Id, authorToUpdate.Name));
            if (!result.IsSuccessfull)
            {
                return NotFound(result.ErrorMessage);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an author", Description = "Deletes an author by their unique ID.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (!result.IsSuccessfull)
            {
                return NotFound(result.ErrorMessage);
            }

            var deleteResult = await _mediator.Send(new RemoveAuthorCommand(id));
            if (!deleteResult.IsSuccessfull)
            {
                return BadRequest(deleteResult.ErrorMessage);
            }

            return NoContent();
        }
    }
}