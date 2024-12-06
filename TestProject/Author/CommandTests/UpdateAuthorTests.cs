﻿using Application.Authors.Commands.UpdateAuthor;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Authors.Commands
{
    public class UpdateAuthorCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateAuthorCommandHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_UpdateAuthor_isCalled_Then_AuthorUpdated()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            Author authorToUpdate = new Author(authorId, "Original Name");
            _fakeDatabase.Authors.Add(authorToUpdate);

            // Act
            var updatedName = "Updated Name";
            OperationResult<Author> result = await _mediator.Send(new UpdateAuthorCommand(authorId, updatedName));

            // Assert
            Assert.That(result.IsSuccessfull, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Name, Is.EqualTo(updatedName));
        }

        [Test]
        public async Task When_Method_UpdateAuthor_isCalled_With_InvalidAuthor_Then_AuthorNotUpdated()
        {
            // Arrange
            var invalidAuthorId = Guid.NewGuid();
            var invalidAuthorName = "Invalid Author";

            // Act
            OperationResult<Author> result = await _mediator.Send(new UpdateAuthorCommand(invalidAuthorId, invalidAuthorName));

            // Assert
            Assert.That(result.IsSuccessfull, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo("Author not found"));
        }
    }
}