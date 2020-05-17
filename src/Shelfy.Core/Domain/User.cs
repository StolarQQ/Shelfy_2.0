using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;
using Shelfy.Core.Exceptions;

namespace Shelfy.Core.Domain
{
    public class User
    {
        private static readonly Regex UrlRegex = new Regex("(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)");
        private static readonly Regex EmailRegex = new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
        private static readonly Regex TextRegex = new Regex(@"[^A-Za-z0-9]");
        private const string DefaultAvatar = "https://www.stolarstate.pl/avatar/user/default.png";

        private readonly ISet<ReviewShortcut> _reviews = new HashSet<ReviewShortcut>();

        [BsonId]
        public Guid UserId { get; private set; }
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }
        public string AvatarPath { get; private set; }
        public Role Role { get; private set; }
        public State State { get; private set; }
        public string ProfileUrl => $"https://localhost:5001/user/{UserId}";
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public IEnumerable<ReviewShortcut> Reviews => _reviews;
        
        private User()
        {
            
        }

        public User(Guid userid, string email, string username, string password, string salt, Role role, string avatar)
        {
            UserId = userid;
            SetEmail(email);
            SetUsername(username);
            SetPassword(password);
            Salt = salt;
            SetRole(role);
            SetAvatar(avatar);
            State = State.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetEmail(string email)
        {
            if (EmailRegex.IsMatch(email) == false)
            {
                throw new DomainException(ErrorCodes.InvalidEmail, $"Email {email} is invalid.");
            }

            Email = email.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetUsername(string username)
        {
            if (TextRegex.IsMatch(username))
            {
                throw new DomainException(ErrorCodes.InvalidUsername, "Username cannot contains special characters.");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new DomainException(ErrorCodes.InvalidUsername, "Username can not be empty.");
            }

            if (username.Length < 2)
            {
                throw new DomainException(ErrorCodes.InvalidUsername, "Username must contain at least 2 characters.");
            }

            if (username.Length > 20)
            {
                throw new DomainException(ErrorCodes.InvalidUsername, "Username cannot contain more than 20 characters.");
            }

            Username = username.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainException(ErrorCodes.InvalidPassword, "Password cannot be empty.");
            }

            Password = password;
            UpdatedAt = DateTime.UtcNow;

        }

        public void SetRole(Role role)
        {
            var isValid = role.Equals(Role.User) ||
                          role.Equals(Role.Moderator) ||
                          role.Equals(Role.Admin);

            if (!isValid)
            {
                throw new DomainException(ErrorCodes.InvalidRole, "Invalid role.");
            }

            Role = role;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetState(State state)
        {
            var isValid = state.Equals(State.Active) ||
                          state.Equals(State.Unconfirmed) ||
                          state.Equals(State.Locked) ||
                          state.Equals(State.Deleted);

            if (!isValid)
            {
                throw new DomainException(ErrorCodes.InvalidState, "Invalid account state.");
            }

            State = state;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddReview(ReviewShortcut review)
        {
            if (review == null)
            {
                throw new ArgumentNullException();
            }

            _reviews.Add(review);

            UpdatedAt = DateTime.UtcNow;
        }


        public void SetAvatar(string avatar)
        {
            if (UrlRegex.IsMatch(avatar) == false)
            {
                throw new DomainException(ErrorCodes.InvalidAvatar, $"URL {avatar} doesn't meet required criteria");
            }

            if (AvatarPath == avatar)
                return;

            AvatarPath = avatar;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DeleteAvatar()
        {
            if (AvatarPath == DefaultAvatar)
            {
                throw new DomainException(ErrorCodes.InvalidAvatar, "AvatarPath cannot be deleted, you are using default one");
            }

            AvatarPath = DefaultAvatar;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// After email confirmation, account it's activated.
        /// </summary>
        public void Activated()
        {
            if (State == State.Active)
            {
                throw new DomainException(ErrorCodes.InvalidState, $"User with id: '{UserId}' is already activated.");
            }
            State = State.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Account should be locked after breaking community guidelines
        /// </summary>
        public void Lock()
        {
            if (State == State.Locked)
            {
                throw new DomainException(ErrorCodes.InvalidState, $"User with id: '{UserId}' is already locked.");
            }
            State = State.Locked;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unlock()
        {
            if (State != State.Locked)
            {
                throw new DomainException(ErrorCodes.InvalidState, $"User with id: '{UserId}' is already unlocked.");
            }
            State = State.Active;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}