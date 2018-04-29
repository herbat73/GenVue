using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using GenVue.Model;
using System.Collections.Generic;
using System.Linq;

namespace GenVue.Model.InitData
{
    public class GroupsInit
    {

        static Random random = new Random();

        public static void Initialize(IServiceProvider serviceProvider)
        {

            var context = new DefaultDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<DefaultDbContext>>());

            context.Database.EnsureCreated();

            if (context.Groups.Count() == 0)
            {
                CreateGroups(serviceProvider);
                AddRandomUsersToGroups(serviceProvider);
            }
        }

        private static void CreateGroups(IServiceProvider serviceProvider)
        {

            var context = new DefaultDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<DefaultDbContext>>());

            var groups = new Group[]
                {
                    new Group { Name = "General" },
                    new Group { Name = "Corpo documents" },
                    new Group { Name = "Agreements" }
                };

            foreach (Group g in groups)
            {
                context.Groups.Add(g);
            }

            context.SaveChanges();

        }

        private static void AddRandomUsersToGroups(IServiceProvider serviceProvider)
        {
            var context = new DefaultDbContext(
                           serviceProvider.GetRequiredService<DbContextOptions<DefaultDbContext>>());




        }
    }
}
