using Microsoft.EntityFrameworkCore;

namespace Ctrl_Save.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new Ctrl_SaveContext(
                serviceProvider.GetRequiredService<DbContextOptions<Ctrl_SaveContext>>());

            if (context.Products.Any()) return; // Already seeded

            context.Products.AddRange(
                // ===== GAMING =====
                new Product { ProductId = "nintendo-64",              Name = "Nintendo 64",                 Price = "N$850",   Image = "nintendo_64.jpg",          Category = "gaming", Condition = "Good",     Listed = "13 Jun 2026", Includes = "Console, Controller x1, Power cable, AV cable, Game (NFL Quarterback Club 98)" },
                new Product { ProductId = "snes-classic-mini",        Name = "SNES Classic Mini",           Price = "N$400",   Image = "snes_classic_mini.jpg",    Category = "gaming", Condition = "Like New", Listed = "31 May 2026", Includes = "Console, Controller x2, HDMI cable, USB power cable, Operations Manual" },
                new Product { ProductId = "nintendo-classic-edition", Name = "Nintendo Classic Edition",    Price = "N$350",   Image = "nes_classic_edition.jpg",  Category = "gaming", Condition = "Good",     Listed = "29 May 2026", Includes = "Console, Controller x2, AV cable, AC Adapter, User manual, Game list" },
                new Product { ProductId = "nintendo-switch-lite",     Name = "Nintendo Switch Lite",        Price = "N$1,500", Image = "nintendo_switch_lite.jpg", Category = "gaming", Condition = "Good",     Listed = "08 Jun 2026", Includes = "Console, Charging cable" },
                new Product { ProductId = "nintendo-switch-oled",     Name = "Nintendo Switch OLED",        Price = "N$2,200", Image = "nintendo_switch_oled.jpg", Category = "gaming", Condition = "Like New", Listed = "10 Jun 2026", Includes = "Console, Dock station, Charger" },
                new Product { ProductId = "nintendo-gba-sp",          Name = "Nintendo Gameboy Advance SP", Price = "N$750",   Image = "nintendo_gba_sp.jpg",      Category = "gaming", Condition = "Fair",     Listed = "22 May 2026", Includes = "Console, Charger" },
                new Product { ProductId = "nintendo-gba",             Name = "Nintendo Gameboy Advance",    Price = "N$950",   Image = "nintendo_gba.jpg",         Category = "gaming", Condition = "Good",     Listed = "12 Jun 2026", Includes = "Console, Games x3 (The Chronicles of Narnia: The Lion, The Witch and The Wardrobe, Pirates of the Caribbean: Dead Man's Chest, Catz" },
                new Product { ProductId = "nintendo-wii",             Name = "Nintendo Wii",                Price = "N$3,000", Image = "nintendo_wii.jpg",         Category = "gaming", Condition = "Good",     Listed = "14 Jun 2026", Includes = "Console, Wiimote x2, Charger, AV cable, Nunchuck, Sensor bar, Game x1 (WiiSPorts Resort)" },
                new Product { ProductId = "sony-playstation-3",       Name = "Sony Playstation 3",          Price = "N$3,000", Image = "ps_3.jpg",                 Category = "gaming", Condition = "Good",     Listed = "16 Jun 2026", Includes = "Console, Controller x2, HDMI cable, CHarger, USB cable, Games x3 (Assassin's Creed: Revelations, The Elder's Scroll V: Skyrim, Welcome to PlayStation 3 and PlayStation Network" },
                new Product { ProductId = "sony-psp",                 Name = "Sony PSP",                    Price = "N$1,800", Image = "sony_psp.jpg",             Category = "gaming", Condition = "Fair",     Listed = "17 Jun 2026", Includes = "Console, Charger, Console pouch" },
                new Product { ProductId = "sony-playstation-4-pro",   Name = "Sony Playstation 4 Pro",      Price = "N$3,800", Image = "ps_4_pro.jpg",             Category = "gaming", Condition = "Like New", Listed = "18 Jun 2026", Includes = "Console, Controller x1, Games x8 (Zombie Vikings: Ragnarok Edition, FIFI20, W2K20, Tomb Raider: Definitive Edition, GTA V: Premium Edition, Horizon: Zero Dawn, The Last of Us: Remastered, Mafia III " },
                new Product { ProductId = "sony-playstation-2-slim",  Name = "Sony Playstation 2 Slim",     Price = "N$2,500", Image = "ps_2_slim.jpg",            Category = "gaming", Condition = "Good",     Listed = "11 Jun 2026", Includes = "Console, Controller x1, Memory Card (16MB), Power cable, AV cable, Final Fantasy X x2, Final Fantasy XI, Tetris Worlds, Namco Museum, Tetris Plus" },

                // ===== LAPTOP =====
                new Product { ProductId = "hp-stream-15",        Name = "HP Stream 15",        Price = "N$3,500", Image = "HP_Stream_15.jpg",        Category = "laptop", Condition = "Good",     Listed = "22 May 2026", Includes = "Laptop, Charger" },
                new Product { ProductId = "hp-spectre-x360",     Name = "HP Spectre X360",     Price = "N$4,200", Image = "HP_Spectre_X360.jpg",     Category = "laptop", Condition = "Like New", Listed = "10 Jun 2026", Includes = "Laptop, Charger" },
                new Product { ProductId = "hp-elitebook-820-g2", Name = "HP EliteBook 820 G2", Price = "N$5,000", Image = "HP_Elitebook_820_G2.jpg", Category = "laptop", Condition = "Good",     Listed = "14 Jun 2026", Includes = "Laptop, Charger" },
                new Product { ProductId = "lenovo-thinkbook-14", Name = "Lenovo ThinkBook 14", Price = "N$3,800", Image = "Lenovo_Thinkbook_14.jpg", Category = "laptop", Condition = "Like New", Listed = "29 May 2026", Includes = "Laptop, Charger" },
                new Product { ProductId = "dell-latitude-5531",  Name = "Dell Latitude 5531",  Price = "N$6,500", Image = "Dell_Latitude_5531.jpg",  Category = "laptop", Condition = "Good",     Listed = "08 Jun 2026", Includes = "Laptop, Charger" },

                // ===== PHONE =====
                new Product { ProductId = "iphone-13-256gb",         Name = "iPhone 13 256GB",         Price = "N$7,000", Image = "iphone_13.jpg",         Category = "phone", Condition = "Like New", Listed = "08 Jun 2026", Description = "Phone in excellent condition.",                       Includes = "Phone, Charger" },
                new Product { ProductId = "iphone-11-128gb",         Name = "iPhone 11 128GB",         Price = "N$5,000", Image = "iphone_11.jpg",         Category = "phone", Condition = "Good",     Listed = "13 May 2026", Description = "Fully functional with no major scratches.",   Includes = "Phone, Charging cable, Original box" },
                new Product { ProductId = "iphone-8-plus-128gb",     Name = "iPhone 8 Plus 128GB",     Price = "N$2,500", Image = "iphone_8_plus.jpg",     Category = "phone", Condition = "Fair",     Listed = "10 Jun 2026", Description = "Works perfectly but has few scratches on back.",        Includes = "Phone, Charger" },
                new Product { ProductId = "iphone-13-pro-max-256gb", Name = "iPhone 13 Pro Max 256GB", Price = "N$5,800", Image = "iphone_13_pro_max.jpg", Category = "phone", Condition = "Good",     Listed = "22 May 2026", Description = "Great condition with no issues.",              Includes = "Phone, Charger, Original box, Phone case x1" },

                // ===== CAMERA =====
                new Product { ProductId = "canon-powershot-a480",     Name = "Canon Powershot A480",     Price = "N$700",   Image = "canon_powershot_a480.jpg",     Category = "camera", Condition = "Fair",     Listed = "10 Jun 2026", Includes = "Camera, SD card (32GB), Strap, Micro USB cable" },
                new Product { ProductId = "canon-powershot-sx700hs",  Name = "Canon Powershot SX700HS",  Price = "N$1,250", Image = "canon_powershot_sx700hs.jpg",  Category = "camera", Condition = "Good",     Listed = "08 Jun 2026", Includes = "Camera, Battery, Strap, Charger, USB cable" },
                new Product { ProductId = "fujifilm-instax-mini-8",   Name = "Fujifilm Instax Mini 8",   Price = "N$2,200", Image = "fujifilm_instax_mini_8.jpg",   Category = "camera", Condition = "Like New", Listed = "22 May 2026", Includes = "Camera, Strap, Insttuction manual" },
                new Product { ProductId = "canon-powershot-sx260hs",  Name = "Canon Powershot SX260HS",  Price = "N$1,300", Image = "canon_powershot_sx260hs.jpg",  Category = "camera", Condition = "Good",     Listed = "14 Jun 2026", Includes = "Camera, Battery, Strap, Charger, USB cable" },
                new Product { ProductId = "canon-powershot-elph-190", Name = "Canon Powershot ELPH 190", Price = "N$1,000", Image = "canon_powershot_elph_190.jpg", Category = "camera", Condition = "Good",     Listed = "13 May 2026", Includes = "Camera, Battery, Strap, Charger" },
                new Product { ProductId = "sony-cyber-shot-dsc-w55",  Name = "Sony Cyber-shot DSC-W55",  Price = "N$950",   Image = "sony_cyber-shot_dsc_w55.jpg",  Category = "camera", Condition = "Fair",     Listed = "31 May 2026", Includes = "Camera, Charger" },
                new Product { ProductId = "canon-vixia-hf-r400-hd",  Name = "Canon Vixia HF R400 HD",   Price = "N$1,300", Image = "canon_vixia_hf_r400_hd.jpg",  Category = "camera", Condition = "Good",       Listed = "29 May 2026", Includes = "Camcorder, Battery, Power adapter, Mini HDMI cable, SUB cable, Instruction manual, Charger" },
                new Product { ProductId = "sony-handycam-dcr-sr47",   Name = "Sony Handycam DCR-SR47",   Price = "N$900",   Image = "sony_handycam_dcr_sr47.jpg",   Category = "camera", Condition = "Fair",     Listed = "31 May 2026", Includes = "Camcorder, Battery, AC Adapter, AV cable, USB cable, Operating manual, Charger " }
            );

            context.SaveChanges();
        }
    }
}
