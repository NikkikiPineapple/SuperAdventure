using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class World
    {
        public static readonly List<Recipe> Recipes = new List<Recipe>();
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Quest> Quests = new List<Quest>();
        public static readonly List<Location> Locations = new List<Location>();

        public const int ITEM_ID_RUSTY_SWORD = 1;
        public const int ITEM_ID_RAT_TAIL = 2;
        public const int ITEM_ID_PIECE_OF_FUR = 3;
        public const int ITEM_ID_SNAKE_FANG = 4;
        public const int ITEM_ID_SNAKESKIN = 5;
        public const int ITEM_ID_CLUB = 6;
        public const int ITEM_ID_HEALING_POTION = 7;
        public const int ITEM_ID_SPIDER_FANG = 8;
        public const int ITEM_ID_SPIDER_SILK = 9;
        public const int ITEM_ID_ADVENTURER_PASS = 10;
        public const int ITEM_ID_SQUISH = 11;
        public const int ITEM_ID_GREAT_AXE = 12;
        public const int ITEM_ID_FIREBALL = 13;
        public const int ITEM_ID_BONE = 14;
        public const int ITEM_ID_BONE_SWORD = 15;
        public const int ITEM_ID_FIRE_RUNE = 16;
        public const int ITEM_ID_GOBLIN_HEAD = 17;
        public const int ITEM_ID_FIRE_SWORD = 18;
        // this is the recipe ID (fix later)
        public const int ITEM_ID_LEATHER_VEST = 19;
        // This is the item ID
        public const int ITEM_ID_LEATHER_VEST_ARMOR = 21;
        public const int ITEM_ID_LEATHER_SCRAP = 20;
        public const int ITEM_ID_IRON_SCRAPS = 23;
        public const int ITEM_ID_IRON_SWORD = 24;

        public const int CRAFTING_RECIPE_IRON_SWORD_ID = 22;
        



        public const int MONSTER_ID_RAT = 1;
        public const int MONSTER_ID_SNAKE = 2;
        public const int MONSTER_ID_GIANT_SPIDER = 3;
        public const int MONSTER_ID_ZOMBIE = 4;
        public const int MONSTER_ID_SLIMES = 5;
        public const int MONSTER_ID_SKELETON = 6;
        public const int MONSTER_ID_GOBLIN = 7;


        public const int QUEST_ID_CLEAR_ALCHEMIST_GARDEN = 1;
        public const int QUEST_ID_CLEAR_FARMERS_FIELD = 2;
        public const int QUEST_ID_CLEAR_SWAMP = 3;
        public const int QUEST_ID_CLEAR_FOREST = 4;

        public const int LOCATION_ID_START_CAVE = 1;
        public const int LOCATION_ID_FOREST = 2;
        public const int LOCATION_ID_GUARD_POST = 3;
        public const int LOCATION_ID_ALCHEMIST_HUT = 4;
        public const int LOCATION_ID_ALCHEMISTS_GARDEN = 5;
        public const int LOCATION_ID_FARMHOUSE = 6;
        public const int LOCATION_ID_FARM_FIELD = 7;
        public const int LOCATION_ID_BRIDGE = 8;
        public const int LOCATION_ID_SPIDER_FIELD = 9;
        public const int LOCATION_ID_SWAMP = 10;
        

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateLocations();
            PopulateRecipes();
        }

        private static void PopulateRecipes()
        {
            Recipes.Add(new Recipe(ITEM_ID_LEATHER_VEST, "Leather Vest", "Leather Vests", 20, 5, ITEM_ID_LEATHER_VEST_ARMOR));
            Recipes.Add(new Recipe(CRAFTING_RECIPE_IRON_SWORD_ID, "Iron Sword", "Iron Swords", 23, 5, ITEM_ID_IRON_SWORD));
        }

        private static void PopulateItems()
        {
            Items.Add(new Weapon(ITEM_ID_FIREBALL, "Fireball", "Fireballs", 50, 100));
            Items.Add(new Weapon(ITEM_ID_GREAT_AXE, "Great Axe", "Great Axes", 5, 25));
            Items.Add(new Weapon(ITEM_ID_RUSTY_SWORD, "Rusty sword", "Rusty swords", 15, 50));
            Items.Add(new Item(ITEM_ID_RAT_TAIL, "Rat tail", "Rat tails"));
            Items.Add(new Item(ITEM_ID_PIECE_OF_FUR, "Piece of fur", "Pieces of fur"));
            Items.Add(new Item(ITEM_ID_SNAKE_FANG, "Snake fang", "Snake fangs"));
            Items.Add(new Item(ITEM_ID_SNAKESKIN, "Snakeskin", "Snakeskins"));
            Items.Add(new Weapon(ITEM_ID_CLUB, "Club", "Clubs", 3, 10));
            Items.Add(new HealingPotion(ITEM_ID_HEALING_POTION, "Healing potion", "Healing potions", 5));
            Items.Add(new Item(ITEM_ID_SPIDER_FANG, "Spider fang", "Spider fangs"));
            Items.Add(new Item(ITEM_ID_SPIDER_SILK, "Spider silk", "Spider silks"));
            Items.Add(new Item(ITEM_ID_ADVENTURER_PASS, "Adventurer pass", "Adventurer passes"));
            Items.Add(new Item(ITEM_ID_SQUISH, "One Squish", "Squishies"));
            Items.Add(new Item(ITEM_ID_BONE, "Bone", "Bones"));
            Items.Add(new Weapon(ITEM_ID_BONE_SWORD, "Bone Sword", "Bone Swords", 50, 100));
            Items.Add(new Item(ITEM_ID_FIRE_RUNE, "Fire Rune", "Fire Runes"));
            Items.Add(new Item(ITEM_ID_GOBLIN_HEAD, "Goblin Head", "Goblin Heads"));
            Items.Add(new Weapon(ITEM_ID_FIRE_SWORD, "Fire Sword", "Fire Swords", 50, 100));
            Items.Add(new Item(ITEM_ID_LEATHER_SCRAP, "Leather Scrap", "Leather Scraps"));
            Items.Add(new Item(ITEM_ID_LEATHER_VEST_ARMOR, "Leather Armor Vest", "Leather Armor Vests"));
            Items.Add(new Item(ITEM_ID_IRON_SCRAPS, "Iron Scap", "Iron Scraps"));
            Items.Add(new Weapon(ITEM_ID_IRON_SWORD, "Iron sword", "Iron swords", 15, 50));


        }

        private static void PopulateMonsters()
        {
            Monster goblin = new Monster(MONSTER_ID_GOBLIN, "Goblin", 5, 3, 10, 10, 10);
            goblin.LootTable.Add(new LootItem(ItemByID(ITEM_ID_GOBLIN_HEAD), 100, false));
            goblin.LootTable.Add(new LootItem(ItemByID(ITEM_ID_IRON_SCRAPS), 100, false));

            Monster skeleton = new Monster(MONSTER_ID_SKELETON, "Skeleton", 5, 3, 10, 10, 10);
            skeleton.LootTable.Add(new LootItem(ItemByID(ITEM_ID_BONE), 75, false));
            skeleton.LootTable.Add(new LootItem(ItemByID(ITEM_ID_LEATHER_SCRAP), 100, false));

            Monster rat = new Monster(MONSTER_ID_RAT, "Rat", 5, 3, 10, 1000, 1000);
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_RAT_TAIL), 75, false));
            rat.LootTable.Add(new LootItem(ItemByID(ITEM_ID_PIECE_OF_FUR), 75, true));

            Monster snake = new Monster(MONSTER_ID_SNAKE, "Snake", 5, 3, 10, 3, 3);
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKE_FANG), 75, false));
            snake.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SNAKESKIN), 75, true));

            Monster giantSpider = new Monster(MONSTER_ID_GIANT_SPIDER, "Giant spider", 20, 5, 40, 10, 10);
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_FANG), 75, true));
            giantSpider.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SPIDER_SILK), 25, false));


            Monster slime = new Monster(MONSTER_ID_SLIMES, "Slime",  15, 20, 50, 30, 50);
            slime.LootTable.Add(new LootItem(ItemByID(ITEM_ID_SQUISH), 75, true));

            Monsters.Add(rat);
            Monsters.Add(snake);
            Monsters.Add(giantSpider);
            Monsters.Add(slime);
            Monsters.Add(skeleton);
            Monsters.Add(goblin);
        }

        private static void PopulateQuests()
        {

            // Give quest at cave to kill 5 goblins
            Quest clearForest =
                new Quest(
                    QUEST_ID_CLEAR_FOREST,
                    "Clear the forest",
                    "Please help traveller. The way through the forest is filled with goblins. Please kill 5 of them so I can escape.", 100, 100);


            clearForest.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_GOBLIN_HEAD), 5));

            clearForest.RewardItem = ItemByID(ITEM_ID_FIRE_RUNE);




            Quest clearSwamp =
                new Quest(
                    QUEST_ID_CLEAR_SWAMP,
                    "Clear the swamp",
                    "Kill the evil Slimes to recieve 5 squish and a Great Axe.", 20, 10);

            clearSwamp.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SQUISH), 3));

            clearSwamp.RewardItem = ItemByID(ITEM_ID_GREAT_AXE);


            Quest clearAlchemistGarden =
                new Quest(
                    QUEST_ID_CLEAR_ALCHEMIST_GARDEN,
                    "Clear the alchemist's garden",
                    "Kill rats in the alchemist's garden and bring back 3 rat tails. You will receive a healing potion and 10 gold pieces.", 20, 10);

            clearAlchemistGarden.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_RAT_TAIL), 3));

            clearAlchemistGarden.RewardItem = ItemByID(ITEM_ID_HEALING_POTION);

            Quest clearFarmersField =
                new Quest(
                    QUEST_ID_CLEAR_FARMERS_FIELD,
                    "Clear the farmer's field",
                    "Kill snakes in the farmer's field and bring back 3 snake fangs. You will receive an adventurer's pass and 20 gold pieces.", 20, 20);

            clearFarmersField.QuestCompletionItems.Add(new QuestCompletionItem(ItemByID(ITEM_ID_SNAKE_FANG), 3));

            clearFarmersField.RewardItem = ItemByID(ITEM_ID_ADVENTURER_PASS);

            Quests.Add(clearAlchemistGarden);
            Quests.Add(clearFarmersField);
            Quests.Add(clearSwamp);
            Quests.Add(clearForest);
        }

        private static void PopulateLocations()
        {
            // Create each location

            Location startLocation = new Location(LOCATION_ID_START_CAVE, "A Dark Cave", "A dark room with anicient glowing runes. One red, one blue and one green.");
            startLocation.MonsterLivingHere = MonsterByID(MONSTER_ID_SKELETON);
            startLocation.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FOREST);

            Location forest = new Location(LOCATION_ID_FOREST, "An Enchanted Forest", "You hear birds chirping and leaves rustling.");
            forest.MonsterLivingHere = MonsterByID(MONSTER_ID_GOBLIN);

            Location swamp = new Location(LOCATION_ID_SWAMP, "Swamp", "A dark smelly swamp with lots of squishy sounds.");
            swamp.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_SWAMP);
            swamp.MonsterLivingHere = MonsterByID(MONSTER_ID_SLIMES);

            Location alchemistHut = new Location(LOCATION_ID_ALCHEMIST_HUT, "Alchemist's hut", "There are many strange plants on the shelves.");
            alchemistHut.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_ALCHEMIST_GARDEN);

            Location alchemistsGarden = new Location(LOCATION_ID_ALCHEMISTS_GARDEN, "Alchemist's garden", "Many plants are growing here.");
            alchemistsGarden.MonsterLivingHere = MonsterByID(MONSTER_ID_RAT);

            Location farmhouse = new Location(LOCATION_ID_FARMHOUSE, "Farmhouse", "There is a small farmhouse, with a farmer in front.");
            farmhouse.QuestAvailableHere = QuestByID(QUEST_ID_CLEAR_FARMERS_FIELD);

            Location farmersField = new Location(LOCATION_ID_FARM_FIELD, "Farmer's field", "You see rows of vegetables growing here.");
            farmersField.MonsterLivingHere = MonsterByID(MONSTER_ID_SNAKE);

            Location guardPost = new Location(LOCATION_ID_GUARD_POST, "Guard post", "There is a large, tough-looking guard here.", ItemByID(ITEM_ID_ADVENTURER_PASS));

            Location bridge = new Location(LOCATION_ID_BRIDGE, "Bridge", "A stone bridge crosses a wide river.");

            Location spiderField = new Location(LOCATION_ID_SPIDER_FIELD, "Forest", "You see spider webs covering covering the trees in this forest.");
            spiderField.MonsterLivingHere = MonsterByID(MONSTER_ID_GIANT_SPIDER);

            // Link the locations together
            startLocation.LocationToNorth = forest;

            forest.LocationToNorth = alchemistHut;
            forest.LocationToSouth = startLocation;
            forest.LocationToEast = guardPost;
            forest.LocationToWest = farmhouse;

            farmhouse.LocationToEast = forest;
            farmhouse.LocationToWest = farmersField;

            farmersField.LocationToEast = farmhouse;
            farmhouse.LocationToWest = swamp;

            swamp.LocationToEast = farmersField;

            alchemistHut.LocationToSouth = forest;
            alchemistHut.LocationToNorth = alchemistsGarden;

            alchemistsGarden.LocationToSouth = alchemistHut;

            guardPost.LocationToEast = bridge;
            guardPost.LocationToWest = forest;

            bridge.LocationToWest = guardPost;
            bridge.LocationToEast = spiderField;

            spiderField.LocationToWest = bridge;

            // Add the locations to the static list
            Locations.Add(startLocation);
            Locations.Add(forest);
            Locations.Add(guardPost);
            Locations.Add(alchemistHut);
            Locations.Add(alchemistsGarden);
            Locations.Add(farmhouse);
            Locations.Add(farmersField);
            Locations.Add(bridge);
            Locations.Add(spiderField);
            Locations.Add(swamp);
        }

        public static Item ItemByID(int id)
        {
            foreach (Item item in Items)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }


        public static Recipe RecipeByID(int id)
        {
            foreach (Recipe recipe in Recipes)
            {
                if (recipe.ID == id)
                {
                    return recipe;
                }
            }

            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }

            return null;
        }

        public static Quest QuestByID(int id)
        {
            foreach (Quest quest in Quests)
            {
                if (quest.ID == id)
                {
                    return quest;
                }
            }

            return null;
        }

        public static Location LocationByID(int id)
        {
            foreach (Location location in Locations)
            {
                if (location.ID == id)
                {
                    return location;
                }
            }

            return null;
        }
    }
}