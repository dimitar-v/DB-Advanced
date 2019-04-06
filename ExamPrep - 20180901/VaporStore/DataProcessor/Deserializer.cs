namespace VaporStore.DataProcessor
{
    using Data;
    using Data.Models;
    using Dto.Import;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDto = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);
            var sb = new StringBuilder();
            var games = new List<Game>();

            foreach (var gameDto in gamesDto)
            {
                if (!IsValid(gameDto) || gameDto.Tags.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };

                game.Developer = GetDeveloper(context, gameDto.Developer);
                game.Genre = GetGenre(context, gameDto.Genre);

                foreach (var tagName in gameDto.Tags)
                {
                    var tag = GetTag(context, tagName);
                    game.GameTags.Add(new GameTag
                    {
                        Game = game,
                        Tag = tag
                    });
                }

                games.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }



        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersDto = JsonConvert.DeserializeObject<ImportUsersDto[]>(jsonString);
            var sb = new StringBuilder();
            var users = new List<User>();

            foreach (var userDto in usersDto)
            {
                if (!IsValid(userDto)
                    || userDto.Cards.Length == 0
                    || !userDto.Cards.All(IsValid)
                    || !userDto.Cards.All(c => Enum.TryParse(c.Type, out CardType test)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User
                {
                    Username = userDto.Username,
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    Age = userDto.Age
                };

                foreach (var cardDto in userDto.Cards)
                {
                    user.Cards.Add(new Card
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.CVC,
                        Type = Enum.Parse<CardType>(cardDto.Type)
                    });
                }

                users.Add(user);
                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportPurchaseDto[]), new XmlRootAttribute("Purchases"));

            var purchasesDto = (ImportPurchaseDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var purchases = new List<Purchase>();

            foreach (var purchaseDto in purchasesDto)
            {
                var game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.Title);
                var card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.Card);

                if (!IsValid(purchaseDto) || !Enum.TryParse(purchaseDto.Type, out PurchaseType purchaseType)
                    || game == null || card == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase
                {
                    Game = game,
                    ProductKey = purchaseDto.Key,
                    Type = purchaseType,
                    Card = card,
                    Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                };

                purchases.Add(purchase);
                sb.AppendLine($"Imported {game.Name} for {card.User.Username}");
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static Tag GetTag(VaporStoreDbContext context, string tagName)
        {
            var tag = context.Tags.FirstOrDefault(t => t.Name == tagName);

            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                context.Tags.Add(tag);
                context.SaveChanges();
            }

            return tag;
        }

        private static Genre GetGenre(VaporStoreDbContext context, string genreName)
        {
            var genre = context.Genres.FirstOrDefault(g => g.Name == genreName);

            if (genre == null)
            {
                genre = new Genre { Name = genreName };
                context.Genres.Add(genre);
                context.SaveChanges();
            }

            return genre;
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, string developerName)
        {
            var developer = context.Developers.FirstOrDefault(d => d.Name == developerName);

            if (developer == null)
            {
                developer = new Developer { Name = developerName };
                context.Developers.Add(developer);
                context.SaveChanges();
            }

            return developer;
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(entity, validationContext, validationResults, true);
        }
    }
}