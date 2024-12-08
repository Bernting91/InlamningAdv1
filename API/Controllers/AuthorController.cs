using Application.Authors.Commands.AddAuthor;
using Application.Authors.Commands.RemoveAuthor;
using Application.Authors.Commands.UpdateAuthor;
using Application.Authors.Queries.GetAllAuthors;
using Application.Authors.Queries.GetAuthorByID;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IMediator mediator, ILogger<AuthorController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all authors", Description = "Retrieves a list of all authors.")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Retrieving all authors");
            var result = await _mediator.Send(new GetAllAuthorsQuery());
            if (!result.IsSuccessfull)
            {
                _logger.LogWarning("No authors found");
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get author by ID", Description = "Retrieves an author by their unique ID.")]
        public async Task<ActionResult<Author>> Get(Guid id)
        {
            _logger.LogInformation("Retrieving author with ID: {AuthorId}", id);
            var result = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (!result.IsSuccessfull)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found", id);
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add a new author", Description = "Creates a new author.")]
        public async Task<ActionResult<Author>> Post([FromBody] Author authorToAdd)
        {
            _logger.LogInformation("Adding a new author: {AuthorName}", authorToAdd.Name);
            var result = await _mediator.Send(new AddAuthorCommand(authorToAdd));
            if (!result.IsSuccessfull)
            {
                _logger.LogWarning("Failed to add author: {ErrorMessage}", result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
            return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an author", Description = "Updates an existing author.")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Author authorToUpdate)
        {
            _logger.LogInformation("Updating author with ID: {AuthorId}", id);
            if (id != authorToUpdate.Id)
            {
                _logger.LogWarning("ID mismatch: {AuthorId} != {AuthorToUpdateId}", id, authorToUpdate.Id);
                return BadRequest("ID mismatch.");
            }

            var result = await _mediator.Send(new UpdateAuthorCommand(authorToUpdate.Id, authorToUpdate.Name));
            if (!result.IsSuccessfull)
            {
                _logger.LogWarning("Failed to update author with ID: {AuthorId}", id);
                return NotFound(result.ErrorMessage);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an author", Description = "Deletes an author by their unique ID.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Deleting author with ID: {AuthorId}", id);
            var result = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (!result.IsSuccessfull)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found", id);
                return NotFound(result.ErrorMessage);
            }

            var deleteResult = await _mediator.Send(new RemoveAuthorCommand(id));
            if (!deleteResult.IsSuccessfull)
            {
                _logger.LogWarning("Failed to delete author with ID: {AuthorId}", id);
                return BadRequest(deleteResult.ErrorMessage);
            }

            return NoContent();
        }
    }
}