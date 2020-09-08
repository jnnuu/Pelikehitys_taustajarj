using System;
using System.Linq;

namespace SessionTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part 1.
            Player[] players = InstantiatePlayers(1000000);

            // TESTCODE for CheckDuplicates
            // var player1 = new Player();
            // var player2 = new Player();
            // player2.Id = player1.Id;
            // Player[] players = { player1, player2 };

            CheckDuplicates(players);


            // Part 2.
            Player player = new Player();
            player.AddItem(new Item(19));
            player.AddItem(new Item(2));
            player.AddItem(new Item(200));
            player.AddItem(new Item(57));

            var item = player.GetHighestLevelItem();

            Console.WriteLine("Highest level: " + item.Level);

            // Part 3 and 4.
            Item[] newArray = GetItems(player);

            foreach (var slot in player.Items)
                Console.WriteLine(slot.Id);
            Console.WriteLine("\n");
            foreach (var slot in newArray)
                Console.WriteLine(slot.Id);

            Console.WriteLine(FirstItem(player) + "\n");
            Console.WriteLine(FirstItemWithLinq(player) + "\n");

            // Part 5.
            // del == a method that will be passed as a parameter to another method.
            Action<Item> del = PrintItem;
            ProcessEachItem(player, del);

            // Part 6.
            del = (item) => Console.WriteLine("Item id: " + item.Id + " , Level: " + item.Level);
            ProcessEachItemLambda(player, del);
        }

        public static Action<Player, Action<Item>> ProcessEachItemLambda = (player, process) =>
            {
                foreach (var item in player.Items)
                    process(item);
            };

        public static void ProcessEachItem(Player player, Action<Item> process)
        {
            foreach (var item in player.Items)
                process(item);
        }

        public static void PrintItem(Item item)
        {
            Console.WriteLine("Item id: " + item.Id + " , Level: " + item.Level);
        }

        // ....
        private static void CheckDuplicates(Player[] players)
        {
            var duplicates = players.GroupBy(x => new { x.Id }).Where(x => x.Skip(1).Any()).ToList();
            if (!duplicates.Any())
                Console.WriteLine("No duplicates found");
            else
                Console.WriteLine("Gawrsh darn you found one!");
        }

        // Populate the array of players.
        public static Player[] InstantiatePlayers(int amt)
        {
            var players = new Player[amt];

            for (int i = 0; i < players.Length; i++)
                players[i] = new Player();

            return players;
        }

        // Hyi
        public static Item[] GetItems(Player player)
        {
            var items = new Item[player.Items.Count];
            var i = 0;
            foreach (var item in player.Items)
            {
                items[i] = item;
                i++;
            }
            return items;
        }

        // awwwwyeeee
        public static Item[] GetItemsWithLinq(Player player)
        {
            return player.Items.ToArray();
        }

        public static Item FirstItem(Player player)
        {
            var items = new Item[player.Items.Count];
            var i = 0;
            foreach (var item in player.Items)
            {
                items[i] = item;
                i++;
            }
            return items[i];
        }

        public static Item FirstItemWithLinq(Player player)
        {
            return player.Items.First();
        }
    }
}
