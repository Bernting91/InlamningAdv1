﻿namespace Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Author Author { get; set; }

        public Book(Guid id, string title, string description, Author author)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
        }
    }
}