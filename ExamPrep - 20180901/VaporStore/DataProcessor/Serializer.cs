namespace VaporStore.DataProcessor
{
	using System;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using Dto.Export;
    using Data.Models;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.IO;
    using System.Text;
    using System.Xml;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
            var genres = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .Select(g => new
                    {
                        Id = g.Id,
                        Genre = g.Name,
                        Games = g.Games
                            .Where(ga => ga.Purchases.Count > 0)
                            .Select(ga => new
                            {
                                Id = ga.Id,
                                Title = ga.Name,
                                Developer = ga.Developer.Name,
                                Tags = string.Join(", ", ga.GameTags.Select(t => t.Tag.Name).ToArray()),
                                Players = ga.Purchases.Count
                            })
                            .OrderByDescending(ga => ga.Players)
                            .ThenBy(ga => ga.Id)
                            .ToArray(),
                        TotalPlayers = g.Games.Sum(ga => ga.Purchases.Count)
                    })
                .OrderByDescending(g => g.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToArray();

            return JsonConvert.SerializeObject(genres, Newtonsoft.Json.Formatting.Indented);
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
            var purchaseType = Enum.Parse<PurchaseType>(storeType);

            var users = context.Users
                .Select(u => new ExportUserDto
                {
                    Username = u.Username,
                    Purchases = u.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == purchaseType)
                        .Select(p => new ExportPurchaseDto
                        {
                            Card = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new ExportGameDto
                            {
                                Genre = p.Game.Genre.Name,
                                Title = p.Game.Name,
                                Price = p.Game.Price
                            }
                        })
                        .OrderBy(p => p.Date)
                        .ToArray(),
                    TotalSpent = u.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == purchaseType)
                        .Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Any())
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportUserDto[]), new XmlRootAttribute("Users"));
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var sb = new StringBuilder();

            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().TrimEnd();
		}
	}
}