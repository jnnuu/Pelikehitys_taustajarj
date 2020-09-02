using System;

public class Item
{
    public Guid Id { get; set; }
    public int Level { get; set; }
    delegate void DelegateExample(Item item);

    public Item(int lvl)
    {
        Id = Guid.NewGuid();
        Level = lvl;
    }
}