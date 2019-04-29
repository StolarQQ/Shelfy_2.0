using System;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace Shelfy.Core.Domain
{
    public class User
    {
        private static readonly Regex UrlRegex = new Regex("(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)");
        private static readonly Regex EmailRegex = new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
        private const string DefaultAvatar = "https://www.stolarstate.pl/avatar/user/default.png";

        [BsonId]
        public Guid UserId { get; protected set; }
        public string Email { get; protected set; }
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        // Path to user avatar
        public string Avatar { get; protected set; }
        public Role Role { get; protected set; }
        public State State { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        protected User()
        {

        }

        public User(Guid userid, string email, string username, string password, string salt, Role role, string avatar)
        {
            UserId = userid;
            SetEmail(email);
            SetUsername(username);
            SetPassword(password);
            Salt = salt;
            Role = Role.User;
            SetAvatar(avatar);
            State = State.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void SetEmail(string email)
        {
            if (EmailRegex.IsMatch(email) == false)
            {
                throw new ArgumentException($"Email is invalid {email}.");
            }
            
            Email = email.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username can not be empty.");
            }

            if (username.Length < 2)
            {
                throw new ArgumentException("Username must contain at least 2 characters.");
            }

            if (username.Length > 20)
            {
                throw new ArgumentException("Username can not contain more than 20 characters.");
            }

            Username = username.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password can not be empty.");
            }

            if (password.Length < 4)
            {
                throw new ArgumentException("Password must contain at least 4 characters.");
            }

            if (password.Length > 15000)
            {
                throw new ArgumentException("Password can not contain more than 55 characters.");
            }

            Password = password;
            UpdatedAt = DateTime.UtcNow;

        }

        public void SetRole(Role role)
        {
            if (role.Equals(0) || role.Equals(1) || role.Equals(2))
            {
                Role = role;
                UpdatedAt = DateTime.UtcNow;
            }
            throw new ArgumentException("Invalid role");


        }

        public void SetAvatar(string avatar)
        {
            if (UrlRegex.IsMatch(avatar) == false)
            {
                throw new ArgumentException($"URL {avatar} doesn't meet required criteria");
            }

            if(Avatar == avatar)
                return;

            Avatar = avatar;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DeleteAvatar()
        {
            Avatar = DefaultAvatar;
            UpdatedAt = DateTime.UtcNow;
        }

        ///// <summary>
        ///// After email confirmation, account it's activated.
        ///// </summary>
        //public void Activated()
        //{
        //    if (State == States.Active)
        //    {
        //        throw new Exception($"User with id: '{UserId}' is already activated.");
        //    }
        //    State = States.Active;
        //    UpdatedAt = DateTime.UtcNow;
        //}

        ///// <summary>
        ///// Account should be locked after breaking community guidelines
        ///// </summary>
        //public void Lock()
        //{
        //    if (State == States.Locked)
        //    {
        //        throw new Exception($"User with id: '{UserId}' is already locked.");
        //    }
        //    State = States.Locked;
        //    UpdatedAt = DateTime.UtcNow;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public void Unlock()
        //{
        //    if (State != States.Locked)
        //    {
        //        throw new Exception($"User with id: '{UserId}' is already unlocked.");
        //    }
        //    State = States.Active;
        //    UpdatedAt = DateTime.UtcNow;
        //}
        
        // Regex extensions class, 
        // Shelf, WantToRead, currently-reading, read, list of Reviews.
    }
}