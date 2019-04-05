using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor.Profiling.Memory.Experimental;

[Serializable]
class PlayerData
{
    public int heath;
    public int mana;
    public int force;
    public float playerPosX, playerPosY;
    public float minCamX, minCamY, maxCamX, maxCamY;
    public int soul;
    public int[] weaponId;
    public int[] armorId;
    public int[] itemId;
    public int[] keyId;
    public int upgradecost;
    public int currentweaponId;
    public int currentarmorId;
    public bool canDoubleJump;
    public bool canBackDash;
}
public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public int heath = 100;
    public int mana = 50;
    public int force = 10;
    public float playerPosX, playerPosY;
    public float minCamX, minCamY, maxCamX, maxCamY;
    public int soul;
    public int[] weaponId;
    public int[] armorId;
    public int[] itemId;
    public int[] keyId;
    public int upgradecost;
    public int currentweaponId;
    public int currentarmorId;
    public bool canDoubleJump = false;
    public bool canBackDash = false;

    private String filePath;
    void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else if (gm != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/playerinfo.dat";

        Load();
    }


    public void Save()
    {
        NewBehaviourScript player = FindObjectOfType<NewBehaviourScript>();
        CameraFollow camera = FindObjectOfType<CameraFollow>();

        itemId = new int[Inventory.inventory.items.Count];
        weaponId = new int[Inventory.inventory.weapons.Count];
        armorId = new int[Inventory.inventory.armors.Count];
        keyId = new int[Inventory.inventory.keys.Count];

        for (int i = 0; i < itemId.Length; i++)
        {
            itemId[i] = Inventory.inventory.items[i].itemID;
        }

        for (int i = 0; i < weaponId.Length; i++)
        {
            weaponId[i] = Inventory.inventory.weapons[i].itemID;
        }

        for (int i = 0; i < armorId.Length; i++)
        {
            armorId[i] = Inventory.inventory.armors[i].itemID;
        }

        for (int i = 0; i < keyId.Length; i++)
        {
            keyId[i] = Inventory.inventory.keys[i].itemID;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerData data = new PlayerData();

        data.heath = player.maxHealth;
        data.mana = player.maxMana;
        data.playerPosX = player.transform.position.x;
        data.playerPosY = player.transform.position.y;
        data.soul = player.souls;
        data.force = player.force;
        data.upgradecost = upgradecost;
        data.maxCamX = camera.maxXAndY.x;
        data.maxCamY = camera.maxXAndY.y;
        data.minCamX = camera.minXAndY.x;
        data.minCamY = camera.minXAndY.y;
        if (player.weaponepuipped != null)
        {
            data.currentweaponId = player.weaponepuipped.itemID;
        }
        if (player.armor != null)
        {
            data.currentarmorId = player.armor.itemID;
        }

        data.canDoubleJump = player.DoubleJumpSkill;
        data.canBackDash = player.DashSkill;

        data.itemId = itemId;
        data.weaponId = weaponId;
        data.armorId = armorId;
        data.keyId = keyId;

        bf.Serialize(file,data);

        file.Close();

        Debug.Log("Data Save");
        FindObjectOfType<UIManager>().SetMessage("Data Saved");
    }

    public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);

        PlayerData data = (PlayerData) bf.Deserialize(file);
        file.Close();

        heath = data.heath;
        mana = data.mana;
        force = data.force;
        playerPosX = data.playerPosX;
        playerPosY = data.playerPosY;
        maxCamX = data.maxCamX;
        maxCamY = data.maxCamY;
        minCamX = data.minCamX;
        minCamY = data.minCamY;
        soul = data.soul;
        upgradecost = data.upgradecost;
        currentarmorId = data.currentarmorId;
        currentweaponId = data.currentweaponId;
        canDoubleJump = data.canDoubleJump;
        canBackDash = data.canBackDash;
        itemId = data.itemId;
        weaponId = data.weaponId;
        armorId = data.armorId;
        keyId = data.keyId;
    }
}
