using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public List<Weapon> weapon;
    public List<Consumable> consumable;
    public List<Armor> armor;
    public List<Key> keys;

    public Weapon Getweapon(int itemid)
    {
        foreach (var item in weapon)
        {
            if (item.itemID == itemid)
            {
                return item;
            }
        }

        return null;
    }

    public Consumable Getconsumable(int itemid)
    {
        foreach (var item in consumable)
        {
            if (item.itemID == itemid)
            {
                return item;
            }
        }

        return null;
        {

        }
    }

    public Armor Getarmor(int itemid)
    {
        foreach (var item in armor)
        {
            if (item.itemID == itemid)
            {
                return item;
            }
        }

        return null;
        {

        }
    }

    public Key GetKey(int itemid)
    {
        foreach (var item in keys)
        {
            if (item.itemID == itemid)
            {
                return item;
            }
        }

        return null;
        {

        }
    }
}
