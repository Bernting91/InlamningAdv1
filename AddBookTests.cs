﻿using Application.Books.Commands.AddBook;
using Domain;
using Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace TestProject.Books.Commands
{
    public class AddBookTests
    {
        private FakeDatabase _fakeDatabase;
        private IMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _fakeDatabase = new FakeDatabase();
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddBookCommandHandler).Assembly));
            services.AddSingleton(_fakeDatabase);
            var provider = services.BuildServiceProvider();
            _mediator = provider.GetRequiredService<IMediator>();
        }

        [Test]
        public async Task When_Method_AddNewBook_isCalled_Then_BookAddedToList()
        {
            // Arrange
            Author author = new Author(1, "Dr.Book McBookie");
            Book bookToTest = new Book(1, "RobertBook", "Book of life", author);

            // Act
            Book bookCreated = await _mediator.Send(new AddBookCommand(bookToTest));

            // Assert
            Assert.That(bookToTest, Is.Not.Null);
            Assert.That(bookCreated.Description, Is.EqualTo(bookToTest.Description));
        }

        [Test]
        public void When_Method_AddNewBook_isCalled_With_EmptyTitle_Then_ArgumentExceptionIsThrown()
        {
            // Arrange
            Author author = new Author(1, "Dr.Book McBookie");
            Book bookToTest = new Book(1, "", "Description", author);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _mediator.Send(new AddBookCommand(bookToTest)));
        }
    }
}