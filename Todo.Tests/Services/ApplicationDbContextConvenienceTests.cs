using System.Linq;
using FluentAssertions;
using Todo.Data;
using Todo.Tests.Infrastructure;
using Xunit;
using AutoFixture;



using Todo.Data.Entities;
using System.Threading.Tasks;
using Todo.Services;
using Microsoft.AspNetCore.Identity;

namespace Todo.Tests.Services
{
    public class ApplicationDbContextConvenienceTests
    {
        private readonly IFixture _fixture;

        public ApplicationDbContextConvenienceTests()

        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        }

        [Fact]
        public async Task RelevantTodoListsWithMultipleUsers()
        {
            var user1EmailAddress = "bob@smyth.com";
            var user2EmailAddress = "tom@smyth.com";
            var user1IdentityUser = new IdentityUser() { Id = user1EmailAddress };
            var user2IdentityUser = new IdentityUser() { Id = user2EmailAddress };

            var user1ToDoItem = _fixture.Build<TodoList>()
                .With(m => m.Owner, user1IdentityUser)
                .With(m => m.Items, _fixture.Build<TodoItem>()
                    .With(x => x.ResponsibleParty, user1IdentityUser)
                    .Without(x => x.TodoList)
                    .CreateMany().ToList())
                .Create();

            var user2ToDoItem = _fixture.Build<TodoList>()
                .With(m => m.Owner, user2IdentityUser)
                .With(m => m.Items, _fixture.Build<TodoItem>()
                    .With(x => x.ResponsibleParty, user2IdentityUser)
                    .Without(x => x.TodoList)
                    .CreateMany().ToList())
                .Create();
            user2ToDoItem.Items.Last().ResponsibleParty = user1IdentityUser;
            using var dbContext = new ApplicationDbContext(InMemoryDbHelper.GetDbContextOptions());

            dbContext.TodoLists.Add(user1ToDoItem);
            dbContext.TodoLists.Add(user2ToDoItem);

            await dbContext.SaveChangesAsync();

            var user1Results = dbContext.RelevantTodoLists(user1EmailAddress);
            user1Results.Should().NotBeNull();
            user1Results.Count().Should().Be(2);
            user1Results.Should().ContainEquivalentOf(user1ToDoItem);
            user1Results.Should().ContainEquivalentOf(user2ToDoItem);


            var user2Results = dbContext.RelevantTodoLists(user2EmailAddress);
            user2Results.Should().NotBeNull();
            user2Results.Count().Should().Be(1);
            user2Results.Should().NotContainEquivalentOf(user1ToDoItem);
            user2Results.Should().ContainEquivalentOf(user2ToDoItem);
        }

        //TODO: Backfill tests in tech debt time
    }
}
