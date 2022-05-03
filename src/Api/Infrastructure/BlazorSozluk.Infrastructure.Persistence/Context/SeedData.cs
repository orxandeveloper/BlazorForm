using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Common.Infrastructure;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Infrastructure.Persistence.Context
{
    internal class SeedData
    {
        private static List<User> GetUsers()
        {
            return new Faker<User>("ru")
                .RuleFor(k => k.Id, v => Guid.NewGuid())
                .RuleFor(k => k.CreateDate, v => DateTime.Now)
                .RuleFor(k => k.FirstName, v => v.Person.FirstName)
                .RuleFor(k => k.LastName, v => v.Person.LastName)
                .RuleFor(k => k.EmailAddress, v => v.Internet.Email())
                .RuleFor(k => k.UserName, v => v.Internet.UserName())
                .RuleFor(k => k.Password, v => PasswordEncryptor.Encrypt(v.Internet.Password()))
                .RuleFor(k => k.EmailConfirmed, v => v.PickRandom(true,false))
                .Generate(500);
                
        }
        public async Task SeedAsync(IConfiguration configuration)
        {
            var dbContextBuilder = new DbContextOptionsBuilder();
            dbContextBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            var context=new BlazorSozlukContext(dbContextBuilder.Options);

            if (context.Users.Any())
            {
                await Task.CompletedTask;
                return;

            }
            var users=GetUsers();
            var userIds=users.Select(x => x.Id);
            await context.Users.AddRangeAsync(users);

            var guids=Enumerable.Range(0,150).Select(x=> Guid.NewGuid()).ToList();

            int counter = 0;
            var entries = new Faker<Entry>("ru")
                .RuleFor(k => k.Id, v=> guids[counter++])
                .RuleFor(k => k.CreateDate, DateTime.Now)
                .RuleFor(k => k.Subject, v => v.Lorem.Sentence(5, 5))
                .RuleFor(k => k.Content, v => v.Lorem.Paragraph(2))
                .RuleFor(k => k.CreatedById, v => v.PickRandom(userIds))
                .Generate(150);
            await context.Entries.AddRangeAsync(entries);

            var comments = new Faker<EntryComment>("ru")
                .RuleFor(k => k.Id,v=> Guid.NewGuid())
                .RuleFor(k => k.CreateDate, DateTime.Now)
                .RuleFor(k => k.Content, v => v.Lorem.Paragraph(2))
                .RuleFor(k => k.CreatedById, v => v.PickRandom(userIds))
                .RuleFor(k => k.EntryId, v => v.PickRandom(guids))
                .Generate(1000);

            await context.EntryComments.AddRangeAsync(comments);

            await context.SaveChangesAsync();

        }
    }
}
