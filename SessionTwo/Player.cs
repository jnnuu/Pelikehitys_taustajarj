using System;
using System.Collections.Generic;
using System.Linq;

public class Player : IPlayer
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public List<Item> Items { get; set; }

    // Constructor creates the random Guid and initializes the Items List
    public Player()
    {
        Id = Guid.NewGuid();
        Items = new List<Item>();
    }

    // public static Item GetHighestLevelItem()
    // {
    //     // Side effects(?)
    //     var orderByDes = from item in Items orderby item.Level descending select item;
    //     return orderByDes.First();
    // }
}