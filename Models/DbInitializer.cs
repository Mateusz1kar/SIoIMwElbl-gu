using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracaDyplomowa.Models
{
    class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Tags.Any())
            {
                context.AddRange(
                    new Tag { Name = "Muzyczne" },
                    new Tag { Name = "Naukowe " },
                    new Tag { Name = "Sport" },
                    new Tag { Name = "Biznes" },
                    new Tag { Name = "Targ" },
                    new Tag { Name = "Koncert" },
                    new Tag { Name = "Kabaret" },
                    new Tag { Name = "Młodzieżowe " },
                    new Tag { Name = "Inne" },
                    new Tag { Name = "Ognisko" },
                    new Tag { Name = "Darmowy wstęp" },
                    new Tag { Name = "Płatny wstęp" },
                    new Tag { Name = "Dla dorosłych" },
                    new Tag { Name = "Ograniczone czasowo" },
                    new Tag { Name = "Cyrk" },
                    new Tag { Name = "Wesołe miasteczko" },
                    new Tag { Name = "Parada" },
                    new Tag { Name = "Strajk" },
                    new Tag { Name = "Pochód" },
                    new Tag { Name = "Pogrzeb" },
                    new Tag { Name = "Urodziny" },
                    new Tag { Name = "Komunia" },
                    new Tag { Name = "Bierzmowanie" },
                    new Tag { Name = "Religijne" },
                    new Tag { Name = "E-sport" }
                    );
            }
            context.SaveChanges();
        }
    }
}
