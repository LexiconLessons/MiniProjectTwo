﻿using MiniProjectTwo.Model;
using Spectre.Console;
using System;
using System.Linq;
using System.Threading;

namespace MiniProjectTwo.Utilities
{
    class CreateOrUpdateProductForm
    {
        public static void Run(int id = -1)
        {
            string[] categories = Category.GetAll().Select(x => x.Name.ToString()).ToArray();
            string[] offices = Office.GetAll().Select(x => x.Name.ToString()).ToArray();

            string category = SelectOption(categories, "category");
            string office = SelectOption(offices, "Office Locations");
            var brand = GetValue<string>("Brand name: ");
            var model = GetValue<string>("Model name: ");
            var price = GetValue<decimal>("Product price: ");
            var purchaseDate = GetValue<DateTime>("Purchase Date: ");

            Product product = new(
                new Category(category),
                new Office(office),
                brand, model, price, purchaseDate
            );

            // if ID param exist update else create new product
            if (id > 0)
            {
                product.Id = id;
                Product.Update(product);
            }
            else
            {
                Product.Create(product);
            }

            Loader();
        }

        private static T GetValue<T>(string prompt)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<T>(prompt)
            );
        }

        private static string SelectOption(string[] options, string name = "option")
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[green]Choose a {name}[/]\n[grey]Use the up and down arrow to select an option.[/]")
                    .PageSize(10)
                    .MoreChoicesText($"[grey](Move up and down to reveal more {name}s)[/]")
                    .AddChoices(options)
            );
        }

        private static void Loader()
        {
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Aesthetic)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start("Initiating database...", ctx =>
                {
                    // Simulate some work
                    AnsiConsole.MarkupLine("\nAdding product into database...");
                    Thread.Sleep(900);

                    AnsiConsole.MarkupLine("\nLoading products...");
                    Thread.Sleep(600);
                }
            );
        }
    }
}