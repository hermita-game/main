using Items;
using UnityEngine;

public static class Tools
{
    private static Fighting.Player _player;
    public static Fighting.Player GetPlayer()
        => _player ? _player : _player = GameObject.FindWithTag("Player").GetComponent<Fighting.Player>();
    
    private static Inventory _inventory;
    public static Inventory GetInventory()
        => _inventory ? _inventory : _inventory = GameObject.Find("UIScripts").GetComponent<Inventory>();
}
