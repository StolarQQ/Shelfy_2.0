﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;
using Shelfy.Core.Exceptions;

namespace Shelfy.Core.Domain
{
    public class Book
    {
        private static readonly Regex CoverRegex = new Regex("(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)");

        [BsonElement]
        private ISet<AuthorShortcut> _authorsDetials = new HashSet<AuthorShortcut>(); 
        [BsonElement]
        private ISet<Review> _reviews = new HashSet<Review>();

        [BsonId]
        public Guid BookId { get; private set; }
        public string Title { get; private set; }
        public string OriginalTitle { get; private set; }
        public string Description { get; private set; }
        public string ISBN { get; private set; }
        public int Pages { get; private set; }
        public string Publisher { get; private set; }
        public DateTime PublishedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        // Path to book cover
        public string Cover { get; private set; }
        public string BookUrl => $"https://localhost:5001/book/{ISBN}";
        //User that added book
        public Guid CreatorId { get; private set; }

        public int ReviewCount => _reviews.Count;
        public double Rating =>
            ReviewCount == 0 ? 0 : Math.Round(_reviews.Average(x => x.Rating), 2);

        public IEnumerable<AuthorShortcut> Authors => _authorsDetials;
        public IEnumerable<Review> Reviews => _reviews;

        // For mongo driver 
        private Book()
        {

        }

        public Book(Guid bookId, string title, string originalTitle,
            string description, string isbn, string cover,
            int pages, string publisher, DateTime publishedAt, Guid creatorId)
        {
            BookId = bookId;
            SetTitle(title);
            OriginalTitle = originalTitle;
            SetDescription(description);
            SetIsbn(isbn);
            SetCover(cover);
            SetPages(pages);
            SetPublisher(publisher);
            PublishedAt = publishedAt;
            UpdatedAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            CreatorId = creatorId;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new DomainException(ErrorCodes.InvalidTitle, $"Book with {BookId} cannot have an empty title.");
            }

            Title = title;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new DomainException(ErrorCodes.InvalidDescription, "Description cannot be empty.");
            }

            if (description.Length < 15)
            {
                throw new DomainException(ErrorCodes.InvalidDescription, "Description must contain at least 15 characters.");
            }

            if (description.Length > 500)
            {
                throw new DomainException(ErrorCodes.InvalidDescription, "Description cannot contain more than 500 characters.");
            }

            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetIsbn(string isbn)
        {
            if (isbn.Length != 13)
            {
                throw new DomainException(ErrorCodes.InvalidIsbn, "ISBN number must contain exactly 13 characters.");
            }

            ISBN = isbn;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPages(int pages)
        {
            if (pages <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidPages, "Number of pages cannot be zero or negative.");
            }

            Pages = pages;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetCover(string cover)
        {
            if (CoverRegex.IsMatch(cover) == false)
            {
                throw new DomainException(ErrorCodes.InvalidCover, $"Cover URL {cover} doesn't meet required criteria.");
            }

            Cover = cover;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPublisher(string publisher)
        {
            if (string.IsNullOrWhiteSpace(publisher))
            {
                throw new DomainException(ErrorCodes.InvalidPublisher, "Publisher cannot be empty.");
            }

            Publisher = publisher;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddAuthor(AuthorShortcut author)
        {
            var authorExist = Authors.SingleOrDefault(x => x.AuthorId == author.AuthorId);
            if (authorExist != null)
            {
                throw new DomainException(ErrorCodes.AuthorAlreadyAdded, $"Author with id: '{author.AuthorId}' already added for Book: '{Title}'.");
            }

            _authorsDetials.Add(author);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAuthor(Guid authorId)
        {
            var authorExist = Authors.SingleOrDefault(x => x.AuthorId == authorId);
            if (authorExist == null)
            {
                throw new DomainException(ErrorCodes.AuthorNotFound, $"Author with id: '{authorId}' was not found for Book: '{Title}'.");
            }

            _authorsDetials.Remove(authorExist);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddReview(Review review)
        {
            var reviewExist = Reviews.SingleOrDefault(x => x.CreatorId == review.CreatorId);
            if (reviewExist != null)
            {
                throw new DomainException(ErrorCodes.ReviewAlreadyAdded, $"User with id '{review.CreatorId}' already added review for book {Title}.");
            }

            _reviews.Add(review);
            UpdatedAt = DateTime.UtcNow;
        }

        public void DeleteReview(Guid userId)
        {
            var reviewExist = Reviews.SingleOrDefault(x => x.CreatorId == userId);
            if (reviewExist == null)
            {
                throw new DomainException(ErrorCodes.ReviewNotFound, $"Review by user with id '{userId}' not exist for book {Title}.");
            }

            _reviews.Remove(reviewExist);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
