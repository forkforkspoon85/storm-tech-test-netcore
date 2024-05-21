using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Xunit;

namespace Todo.Tests.EntityModelMappers
{
    public class TodoListDetailViewModelFactoryTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Create(bool orderByRank)
        {
            var todoList =
                new TestTodoListBuilder(new IdentityUser("bob@smyth.com.com"), "Tasks")
                .WithItem("LowImportanceHighRank", Importance.Low, rank: 1)
                .WithItem("MediumImportanceMediumRank", Importance.Medium, rank: 2)
                .WithItem("HighImportanceLowRank", Importance.High, rank: 3)
                .Build();

            var result = TodoListDetailViewmodelFactory.Create(todoList, orderByRank);

            Assert.NotNull(result);

            if(orderByRank)
            {
                result.Items.First().Title.Should().Be(todoList.Items.First().Title);
                result.Items.Last().Title.Should().Be(todoList.Items.Last().Title);
            }
            else
            {
                result.Items.First().Title.Should().Be(todoList.Items.Last().Title);
                result.Items.Last().Title.Should().Be(todoList.Items.First().Title);
            }
        }
    }
}
