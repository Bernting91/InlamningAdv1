﻿using Application.Books.Commands;
using Domain;
using Infrastructure.Database;
using MediatR;

namespace Application.Books.Handlers;
public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book?>
{
    private readonly FakeDatabase _fakeDatabase;

    public UpdateBookCommandHandler(FakeDatabase fakeDatabase)
    {
        _fakeDatabase = fakeDatabase;
    }

    public Task<Book?> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        if (request.Book == null)
        {
            throw new ArgumentNullException(nameof(request.Book), "Book cannot be null.");
        }
        return Task.FromResult(_fakeDatabase.UpdateBook(request.Book));
    }
}