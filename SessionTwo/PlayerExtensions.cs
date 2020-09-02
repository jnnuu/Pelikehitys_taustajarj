using System;
using System.Collections.Generic;
using System.Linq;

// Extension for Player-class
public static class PlayerExtensions
{
    // Finds the highest level item by creating a copy from the Players Items List and 
    // ordering it by descending item.Level value and returning the first value in the collection.
    // (Side effects?)
    public static Item GetHighestLevelItem(this Player player)
    {
        var orderByDes = from item in player.Items orderby item.Level descending select item;
        return orderByDes.First();
    }

    // Adds an Item to Players Items List if the param isn't null.
    public static void AddItem(this Player player, Item item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        player.Items.Add(item);
    }
}