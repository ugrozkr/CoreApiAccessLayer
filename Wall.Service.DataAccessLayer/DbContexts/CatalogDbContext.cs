using Microsoft.EntityFrameworkCore;
using System;
using Wall.Catalog.Entity;

namespace Wall.Service.DataAccessLayer.DbContexts
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var concertGuid = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            var musicalGuid = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}");
            var playGuid = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}");
            var conferenceGuid = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}");

            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = concertGuid,
                Name = "Biletler"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = musicalGuid,
                Name = "Müzik Kutuları"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = playGuid,
                Name = "Oyuncaklar"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = conferenceGuid,
                Name = "Çiçekler"
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                Name = "Tarkan konseri",
                Price = 65,
                Artist = "Harbiye",
                Date = DateTime.Now.AddMonths(6),
                Description = "Açıklama 1 ",
                ImageUrl = "",
                CategoryId = concertGuid
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"),
                Name = "Ebru gündeş",
                Price = 85,
                Artist = "Harbiye",
                Date = DateTime.Now.AddMonths(9),
                Description = "Açıklama 2",
                ImageUrl = "",
                CategoryId = concertGuid
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{B419A7CA-3321-4F38-BE8E-4D7B6A529319}"),
                Name = "Sertab Erener konseri",
                Price = 85,
                Artist = "Biletix",
                Date = DateTime.Now.AddMonths(4),
                Description = "Açıklama 3",
                ImageUrl = "",
                CategoryId = concertGuid
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{62787623-4C52-43FE-B0C9-B7044FB5929B}"),
                Name = "Kenan Doğulu konseri",
                Price = 25,
                Artist = "Harbiye",
                Date = DateTime.Now.AddMonths(4),
                Description = "Açıklama 4",
                ImageUrl = "",
                CategoryId = concertGuid
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{1BABD057-E980-4CB3-9CD2-7FDD9E525668}"),
                Name = "Gül lale sümbül",
                Price = 400,
                Artist = "FGH Çiçekçilik",
                Date = DateTime.Now.AddMonths(10),
                Description = "Açıklama 5",
                ImageUrl = "",
                CategoryId = conferenceGuid
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
                Name = "Atlı karınca müzik kutusu",
                Price = 135,
                Artist = "ABC Oyuncak",
                Date = DateTime.Now.AddMonths(8),
                Description = "Açıklama 6",
                ImageUrl = "",
                CategoryId = musicalGuid
            });
        }
    }
}
