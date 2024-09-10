using Domain.Brands;
using Domain.Coins;
using Domain.Products;
using Domain.Products.ValueObjects;
using Infrastructure.Data.PostgreSQL;

namespace Migrator;

public interface IDataSeeder
{
    Task SeedAsync();
}

public sealed class DataSeeder(
    ApplicationDbContext dbContext): IDataSeeder
{
    public async Task SeedAsync()
    {
        if (!dbContext.Set<Brand>().Any())
        {
            var cocaCola = new Brand("CocaCola");
            var pepsi = new Brand("Pepsi");
            var sprite = new Brand("Sprite");
            var fanta = new Brand("Fanta");
            var schweppes = new Brand("Schweppes");
            var lipton = new Brand("Lipton");
            var drPepper = new Brand("DrPepper");
            var mountainDew = new Brand("MountainDew");

            dbContext.Set<Brand>().AddRange(cocaCola, pepsi, sprite, fanta, schweppes, lipton, drPepper, mountainDew);

            dbContext.Set<Product>().AddRange(
                new Product("Газированный напиток", 105, cocaCola, new Image("http://localhost:4566/vending-machine/image1.jpg"), 10),
                new Product("Газированный напиток", 100, pepsi, new Image("http://localhost:4566/vending-machine/image2.jpg"), 5),
                new Product("Газированный напиток", 83, sprite, new Image("http://localhost:4566/vending-machine/image3.jpg"), 12),
                new Product("Газированный напиток", 98, fanta, new Image("http://localhost:4566/vending-machine/image4.jpg"), 10),
                new Product("Газированный напиток Zero", 70, sprite, new Image("http://localhost:4566/vending-machine/image5.jpg"), 8),
                new Product("Холодный чай", 90, lipton, new Image("http://localhost:4566/vending-machine/image6.jpg"), 10),
                new Product("Газированный напиток", 110, drPepper, new Image("http://localhost:4566/vending-machine/image7.png"), 10),
                new Product("Газированный напиток Zero", 85, cocaCola, new Image("http://localhost:4566/vending-machine/image8.jpg"), 5)
            );

            var coins = new List<Coin>
            {
                new(CoinDenomination.One, 100),
                new(CoinDenomination.Two, 100),
                new(CoinDenomination.Five, 100),
                new(CoinDenomination.Ten, 100)
            };

            dbContext.Set<Coin>().AddRange(coins);

            await dbContext.SaveChangesAsync();
        }
    }
}