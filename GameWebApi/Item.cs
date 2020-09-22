
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
public class Item {
    public int Level { get; set; }
    public ItemType Type { get; set; }
    public DateTime CreationTime { get; set; }
}

    public enum ItemType {
SWORD, POTION, SHIELD
};
