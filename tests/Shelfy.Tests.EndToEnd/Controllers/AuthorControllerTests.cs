using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shelfy.API;
using Shelfy.Core.Helper;
using Shelfy.Infrastructure.Commands;
using Shelfy.Infrastructure.DTO.Author;
using Shelfy.Infrastructure.DTO.Jwt;
using Shelfy.Tests.EndToEnd.Helpers;
using Xunit;

namespace Shelfy.Tests.EndToEnd.Controllers
{
    public class AuthorControllerTests : ControllerTestsBase
    {
        public AuthorControllerTests(WebApplicationFactory<Startup> fixture) : base(fixture)
        {

        }

        [Fact]
        public async Task get_should_return_author_for_valid_id()
        {
            var authorId = "2a975e6b-f64e-4446-a30a-a01886a3c060";
            var response = await Client.GetAsync($"author/{authorId}");
            var content = await response.Content.ReadAsStringAsync();
            var author = JsonConvert.DeserializeObject<AuthorDto>(content);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            author.Should().NotBeNull();
        }

        [Fact]
        public async Task get_should_return_not_found_when_id_not_exist()
        {
            var authorId = Guid.NewGuid();
            var response = await Client.GetAsync($"author/{authorId}");

            var content = await response.Content.ReadAsStringAsync();
            var errorDto = JsonConvert.DeserializeObject<ErrorDto>(content);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
            errorDto.Message.Should().BeEquivalentTo($"Author with id '{authorId}' was not found.");
        }

        [Fact]
        public async Task browse_authors_should_return_not_empty_collection()
        {
            var response = await Client.GetAsync("author");
            var content = await response.Content.ReadAsStringAsync();
            var authors = JsonConvert.DeserializeObject<PagedResult<AuthorDto>>(content);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            authors.Source.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task browse_authors_with_currentPage_and_pageSize_should_return_paginated_result()
        {
            var queryString = "?pageSize=2";
            var response = await Client.GetAsync($"author{queryString}");
            var content = await response.Content.ReadAsStringAsync();
            var authors = JsonConvert.DeserializeObject<PagedResult<AuthorDto>>(content);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            authors.Source.Count().Should().Be(2);
        }

        [Fact]
        public async Task post_should_return_created_for_valid_input()
        {
            var login = new Login
            {
                Email = "test123@gmaail.com",
                Password = "test123"
            };
            var loginCredentials = GetPayload(login);
            var loginResponse = await Client.PostAsync("user/login", loginCredentials);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var tokenDto = JsonConvert.DeserializeObject<TokenDto>(loginContent);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);

            var commandCreateAuthor = new CreateAuthor
            {
                FirstName = "Jeffrey",
                LastName = "Richter",
                Description = "Jeffrey Richter is a Software Architect on Microsoft's Azure team.",
                ImageUrl = "https://www.stolarstate.pl/avatar/author/default.png",
                DateOfBirth = new DateTime(1955, 12, 12),
                DateOfDeath = null,
                BirthPlace = "Chicago",
                AuthorWebsite = "https://csharpindepth.com",
                AuthorSource = "https://csharpindepth.com"
            };

            var command = GetPayload(commandCreateAuthor);
            var postResponse = await Client.PostAsync("author", command);

            postResponse.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
        }

        [Fact]
        public async Task post_without_authorization_should_return_unauthorized()
        {
            var existAuthorId = "d3e3e83d-68ec-48e8-b8ce-450e22c2dda6";

            var commandCreateAuthor = new CreateAuthor
            {
                FirstName = "Jeffrey",
                LastName = "Richter",
                Description = "Jeffrey Richter is a Software Architect on Microsoft's Azure team.",
                ImageUrl = "https://www.stolarstate.pl/avatar/author/default.png",
                DateOfBirth = new DateTime(1955, 12, 12),
                DateOfDeath = null,
                BirthPlace = "Chicago",
                AuthorWebsite = "https://csharpindepth.com",
                AuthorSource = "https://csharpindepth.com"
            };

            var command = GetPayload(commandCreateAuthor);
            var postResponse = await Client.PostAsync("author", command);

            postResponse.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }
        
        // TODO mapping fix
        [Fact]
        public async Task patch_for_valid_input_should_return_nocontent()
        {
            var login = new Login
            {
                Email = "test123@gmaail.com",
                Password = "test123"
            };
            var existAuthorId = "2a975e6b-f64e-4446-a30a-a01886a3c060";
            var loginCredentials = GetPayload(login);
            var loginResponse = await Client.PostAsync("user/login", loginCredentials);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var tokenDto = JsonConvert.DeserializeObject<TokenDto>(loginContent);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);
                
            var jsonDoc = "[{\"op\":\"replace\",\"path\":\"/FirstName\",\"value\":\"Marcelo\"}]";
            var partialUpdate = JsonConvert.DeserializeObject<JsonPatchDocument<UpdateAuthor>>(jsonDoc);
            var test = GetPayload(partialUpdate);

            var patchResponse = await Client.PatchAsync($"author/{existAuthorId}", test);
            
            patchResponse.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task delete_should_delete_author_after_authorization()
        {
            var login = new Login
            {
                Email = "test123@gmaail.com",
                Password = "test123"
            };
            var existAuthorId = "6a8eb6b8-b67f-4ac7-b0a9-d6c12699abad";
            var loginCredentials = GetPayload(login);
            var loginResponse = await Client.PostAsync("user/login", loginCredentials);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var tokenDto = JsonConvert.DeserializeObject<TokenDto>(loginContent);

            Client.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Bearer", tokenDto.Token);
            var response = await Client.DeleteAsync($"author/{existAuthorId}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task delete_without_login_should_return_unauthorized()
        {
            var authorId = "9a8107af-f43a-4011-a69c-aa924704c7cb";

            var response = await Client.DeleteAsync($"author/{authorId}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }
    }
}