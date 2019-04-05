using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory inventory;
    public List<Weapon> weapons;
    public List<Key> keys;
    public List<Consumable> items;
    public List<Armor> armors;

    public ItemDataBase itemdatabase;
    void Awake()
    {

        if (inventory == null)
        {
            inventory = this;
        }
        else if(inventory != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadInventory();
    }

    void LoadInventory()
    {
        for (int i = 0; i < GameManager.gm.weaponId.Length; i++)
        {
            Addweapon(itemdatabase.Getweapon(GameManager.gm.weaponId[i]));
        }
        for (int i = 0; i < GameManager.gm.itemId.Length; i++)
        {
            AddItem(itemdatabase.Getconsumable(GameManager.gm.itemId[i]));
        }
        for (int i = 0; i < GameManager.gm.armorId.Length; i++)
        {
            AddArmor(itemdatabase.Getarmor(GameManager.gm.armorId[i]));
        }
        for (int i = 0; i < GameManager.gm.keyId.Length; i++)
        {
            AddKey(itemdatabase.GetKey(GameManager.gm.keyId[i]));
        }
    }

    public void Addweapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }

    public void AddKey(Key key)
    {
        keys.Add(key);
    }

    public void AddItem(Consumable item)
    {
        items.Add(item);
    }

    public void AddArmor(Armor armor)
    {
        armors.Add(armor);
    }
    public bool CheckKey(Key key)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == key)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveItem(Consumable item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                items.RemoveAt(i);
                break;
            }
        }
    }

    public int CountItem(Consumable item)
    {
        int numberOfitem = 0;
        for (int i = 0; i < items.Count; i++)
        {
            numberOfitem++;
        }
        return numberOfitem;
    }
}
