using Ctrl_Save.Models;
using Microsoft.EntityFrameworkCore;

namespace Ctrl_Save.Services
{
    public class OrderSimulatorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderSimulatorService> _logger;
        private readonly Random _random = new();

        private static readonly string[] FirstNames = {
            "Liam", "Noah", "Emma", "Olivia", "Aiden", "Sophia", "Lucas", "Mia",
            "Ethan", "Amara", "Jayden", "Zara", "Kwame", "Naledi", "Sipho", "Fatima",
            "Tiago", "Yuki", "Hassan", "Priya", "Kofi", "Leila", "Diego", "Amina"
        };

        private static readonly string[] LastNames = {
            "Smith", "Nkosi", "Patel", "Müller", "Santos", "Okafor", "Botha", "Kim",
            "Nakamura", "Dlamini", "Ferreira", "Mensah", "van der Berg", "Diallo",
            "Oosthuizen", "Abebe", "Rodrigues", "Naidoo", "Fourie", "Chirwa"
        };

        private static readonly string[] PaymentMethods = {
            "Cash on Delivery", "EFT", "Cash on Delivery", "EFT"
        };

        private static readonly string[] Prefixes = { "81", "85", "83" };

        private static readonly string[] SearchTerms = {
            "gaming", "laptop", "camera", "phone", "wireless", "4K", "portable",
            "budget", "premium", "gaming headset", "mirrorless", "smartphone"
        };

        private static readonly (string City, string Region, string[] Suburbs)[] CityData = {
            ("Windhoek", "Khomas", new[] {
                "Kleine Kuppe", "Olympia", "Pioneerspark", "Eros", "Ludwigsdorf",
                "Klein Windhoek", "Akademia", "Hochland Park", "Katutura", "Khomasdal",
                "Windhoek North", "Windhoek West", "Rocky Crest", "Dorado Park", "Auasblick"
            }),
            ("Swakopmund", "Erongo", new[] {
                "Mondesa", "Tamariskia", "Vineta", "Vogelstrand", "Mile 4",
                "Longbeach", "Kramersdorf", "Narraville", "DRC", "Swakopmund Central"
            }),
            ("Walvis Bay", "Erongo", new[] {
                "Narraville", "Kuisebmond", "Meersig", "Lagoon", "Walvis Bay Central",
                "Industrial", "Tutaleni", "Goreangab"
            }),
            ("Henties Bay", "Erongo", new[] { "Henties Bay Central", "Jakkalsputz" }),
            ("Oshakati", "Oshana", new[] {
                "Oshakati East", "Oshakati West", "Oshoopala", "Oneshila", "Ompumbu"
            }),
            ("Ondangwa", "Oshana", new[] { "Ondangwa Central", "Okatana", "Oluno" }),
            ("Rundu", "Kavango East", new[] {
                "Sauyemwa", "Kehemu kehemu", "Nkarapamwe", "Rundu Central", "Tutungeni"
            }),
            ("Katima Mulilo", "Zambezi", new[] {
                "Choto", "Ngweze", "Katima Central", "Cowboy", "Musica"
            }),
            ("Lüderitz", "Karas", new[] { "Lüderitz Central", "Nautilus", "Diaz Point" }),
            ("Keetmanshoop", "Karas", new[] {
                "Kronlein", "Westdene", "Keetmanshoop Central", "Noord Duin"
            }),
            ("Otjiwarongo", "Otjozondjupa", new[] {
                "Orwetoveni", "Otjiwarongo Central", "Okozonduno"
            }),
            ("Grootfontein", "Otjozondjupa", new[] {
                "Grootfontein Central", "Oshivelo", "Tsumkwe Road"
            }),
            ("Gobabis", "Omaheke", new[] { "Epako", "Gobabis Central", "Nossobville" }),
            ("Mariental", "Hardap", new[] { "Mariental Central", "Oanob", "Aan de Stroom" }),
            ("Rehoboth", "Hardap", new[] { "Rehoboth Central", "Baster Gebied", "Block D" }),
            ("Tsumeb", "Oshikoto", new[] { "Tsumeb Central", "Nomtsoub", "Extension 5" }),
            ("Outapi", "Omusati", new[] { "Outapi Central", "Oshikuku", "Onesi" }),
            ("Eenhana", "Ohangwena", new[] { "Eenhana Central", "Ohangwena", "Okongo" }),
            ("Opuwo", "Kunene", new[] { "Opuwo Central", "Uukwaluudhi", "Oruwenje" }),
            ("Nkurenkuru", "Kavango West", new[] { "Nkurenkuru Central", "Mashare", "Divundu" })
        };

        public OrderSimulatorService(IServiceProvider serviceProvider, ILogger<OrderSimulatorService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Order Simulator started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var waitMinutes = _random.Next(8, 13);
                await Task.Delay(TimeSpan.FromMinutes(waitMinutes), stoppingToken);
                if (stoppingToken.IsCancellationRequested) break;
                await PlaceFakeOrder();
            }
        }

        private async Task PlaceFakeOrder()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<Ctrl_SaveContext>();

                var allProducts = await context.Products.ToListAsync();
                if (!allProducts.Any()) return;

                var itemCount = _random.Next(1, 4);
                var shuffled = allProducts.OrderBy(_ => _random.Next()).Take(itemCount).ToList();

                // Only 40% of users search — rest browse directly
                if (_random.Next(100) < 100)
                {
                    var searchTerm = SearchTerms[_random.Next(SearchTerms.Length)];
                    var searchResults = allProducts.Count(p =>
                        p.Name.ToLower().Contains(searchTerm) ||
                        p.Category.ToLower().Contains(searchTerm));
                    _logger.LogInformation("product_search query={Query} results={Count}",
                        searchTerm, searchResults);
                }

                // 70% of users view product details before ordering
                foreach (var product in shuffled)
                {
                    if (_random.Next(100) < 70)
                    {
                        _logger.LogInformation("product_viewed product_name={ProductName} category={Category}",
                            product.Name, product.Category);
                    }
                }

                var firstName = FirstNames[_random.Next(FirstNames.Length)];
                var lastName = LastNames[_random.Next(LastNames.Length)];
                var cityData = CityData[_random.Next(CityData.Length)];
                var suburb = cityData.Suburbs[_random.Next(cityData.Suburbs.Length)];
                var erfNumber = _random.Next(100, 9999);
                var address = $"ERF {erfNumber}, {suburb}";
                var payment = PaymentMethods[_random.Next(PaymentMethods.Length)];
                var prefix = Prefixes[_random.Next(Prefixes.Length)];
                var phone = "+264" + prefix + _random.Next(1000000, 9999999).ToString();

                var orderTotal = shuffled.Sum(p =>
                {
                    var priceStr = p.Price.Replace("N$", "").Replace(",", "");
                    return decimal.TryParse(priceStr, out var price) ? price : 0;
                });

                var order = new Order
                {
                    OrderNumber = "CS-" + DateTime.Now.ToString("yyyyMMdd") + "-" + _random.Next(1000, 9999),
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = phone,
                    Address = address,
                    City = cityData.City,
                    Region = cityData.Region,
                    PaymentMethod = payment,
                    OrderTotal = orderTotal,
                    OrderDate = DateTime.Now,
                    Items = shuffled.Select(p => new OrderItem
                    {
                        ProductId = p.ProductId,
                        ProductName = p.Name,
                        Price = p.Price,
                        Category = p.Category
                    }).ToList()
                };

                context.Orders.Add(order);
                await context.SaveChangesAsync();

                _logger.LogInformation("checkout_success order_number={OrderNumber} user={Name} city={City} items={Items} total={Total}",
                    order.OrderNumber, $"{firstName} {lastName}", cityData.City, itemCount, orderTotal);

                _logger.LogInformation("Fake order: {OrderNumber} by {Name} — {Address}, {City}, {Region} — {Items} item(s) — N${Total}",
                    order.OrderNumber, $"{firstName} {lastName}", address, cityData.City, cityData.Region, itemCount, orderTotal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error placing fake order.");
            }
        }
    }
}