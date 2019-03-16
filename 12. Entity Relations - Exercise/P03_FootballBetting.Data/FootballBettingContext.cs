namespace P03_FootballBetting.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {

        }

        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        
        public DbSet<Game> Games { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnConfiguringTeam(modelBuilder);
            OnConfiguringTown(modelBuilder);
            OnConfiguringGame(modelBuilder);
            OnConfiguringPlayerStatistic(modelBuilder);
            OnConfiguringPlayer(modelBuilder);
            
        }

        private void OnConfiguringPlayer(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<Player>(entity =>
               {
                   entity.HasKey(e => e.PlayerId);

                   entity
                       .HasOne(p => p.Team)
                       .WithMany(t => t.Players)
                       .HasForeignKey(p => p.TeamId);

                   entity
                       .HasOne(p => p.Position)
                       .WithMany(p => p.Players)
                       .HasForeignKey(p => p.PositionId);
               });
        }

        private void OnConfiguringPlayerStatistic(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<PlayerStatistic>(entity =>
               {
                   entity.HasKey(e => new { e.GameId, e.PlayerId });

                   entity
                       .HasOne(ps => ps.Player)
                       .WithMany(p => p.PlayerStatistics)
                       .HasForeignKey(ps => ps.PlayerId);

                   entity
                       .HasOne(ps => ps.Game)
                       .WithMany(g => g.PlayerStatistics)
                       .HasForeignKey(ps => ps.GameId);
               });
        }

        private void OnConfiguringGame(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<Game>(entity =>
               {
                   entity.HasKey(e => e.GameId);

                   entity
                       .HasOne(g => g.HomeTeam)
                       .WithMany(t => t.HomeGames)
                       .HasForeignKey(g => g.HomeTeamId)
                       .OnDelete(DeleteBehavior.Restrict);

                   entity
                       .HasOne(g => g.AwayTeam)
                       .WithMany(t => t.AwayGames)
                       .HasForeignKey(g => g.AwayTeamId)
                       .OnDelete(DeleteBehavior.Restrict);
               });
        }

        private void OnConfiguringTown(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Town>(entity =>
                {
                    entity.HasKey(e => e.TownId);

                    entity
                        .HasOne(t => t.Country)
                        .WithMany(c => c.Towns)
                        .HasForeignKey(t => t.CountryId);
                });
        }

        private void OnConfiguringTeam(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Team>(entity => 
                {
                    entity.HasKey(e => e.TeamId);

                    entity
                        .Property(e => e.Initials)
                        .HasColumnType("CHAR(3)")
                        .IsRequired();

                    entity
                        .HasOne(t => t.PrimaryKitColor)
                        .WithMany(kc => kc.PrimaryKitTeams)
                        .HasForeignKey(t => t.PrimaryKitColorId)
                        .OnDelete(DeleteBehavior.Restrict);

                    entity
                        .HasOne(t => t.SecondaryKitColor)
                        .WithMany(kc => kc.SecondaryKitTeams)
                        .HasForeignKey(t => t.SecondaryKitColorId)
                        .OnDelete(DeleteBehavior.Restrict);

                    entity
                        .HasOne(t => t.Town)
                        .WithMany(t => t.Teams)
                        .HasForeignKey(t => t.TownId);
                });
        }
    }
}
