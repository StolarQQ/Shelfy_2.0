﻿using System;

namespace Shelfy.Infrastructure.DTO.Author
{
    public class AuthorShortcutDto
    {
        public string ProfileUrl { get; set; }
        public Guid AuthorId { get; set; }
        public string FullName { get; set; }
    }
}