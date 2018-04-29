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
    public class FileCategoriesInit
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {

            var context = new DefaultDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<DefaultDbContext>>());

            context.Database.EnsureCreated();

            if (context.FileCategories.Count() == 0)
            {
                FileCategories(serviceProvider);
            }
        }

        private static void FileCategories(IServiceProvider serviceProvider)
        {

            var context = new DefaultDbContext(
                    serviceProvider.GetRequiredService<DbContextOptions<DefaultDbContext>>());

            var fileCategories = new FileCategory[]
                {
                    new FileCategory { Name = "Finance" },
                    new FileCategory { Name = "Trash" },
                    new FileCategory { Name = "Manuals" },
                    new FileCategory { Name = "Employes info" }
                };

            foreach (FileCategory g in fileCategories)
            {
                context.FileCategories.Add(g);
            }

            context.SaveChanges();

        }
    }
}
