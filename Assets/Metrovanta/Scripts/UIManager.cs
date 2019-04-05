using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Transform cursor;
    public GameObject[] MenuOptions;
    public GameObject optionPanel;
    public GameObject itemList;
    public GameObject itemListPrefab;
    public RectTransform content;
    public Text DescriptionText;
    public Scrollbar scrollVertical;
    public Text heathText, manatext, forcetext, attacktext, defensetext;
    public Text heathUi, manaUi, soulUi, potionUi;
    public Text messagetext;

    private bool pauseMenu = false;
    private int cursorIndex = 0;
    private Inventory inventory;
    public List<ItemList> items;
    private bool itemlistactive = false;
    private NewBehaviourScript player;
    private bool isMessagetext = false;
    private float texttimer;
    void Start()
    {
        inventory = Inventory.inventory;
        player = FindObjectOfType<NewBehaviourScript>();
        
    }

    void Update()
    {
        if (isMessagetext)
        {
            Color color = messagetext.color;
            color.a += 2f * Time.deltaTime;
            messagetext.color = color;
            if (color.a >= 1)
            {
                isMessagetext = false;
                texttimer = 0;
            }
        }
        else if (!isMessagetext)
        {
            texttimer += Time.deltaTime;
            if (texttimer >= 2f)
            {
                Color color = messagetext.color;
                color.a -= 2f * Time.deltaTime;
                messagetext.color = color;
                if (color.a <= 0)
                {
                    messagetext.text = "";
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu = !pauseMenu;
            cursorIndex = 0;
            itemlistactive = false;
            DescriptionText.text = "";
            itemList.SetActive(false);
            optionPanel.SetActive(true);
            updateatributes();
            UpdateUI();
            if (pauseMenu)
            {
                pausePanel.SetActive(true);
            }
            else
            {
                pausePanel.SetActive(false);
            }
        }

        if (pauseMenu)
        {
            Vector3 cursorPosition = new Vector3();
            if (!itemlistactive)
            {
                cursorPosition = MenuOptions[cursorIndex].transform.position;
                cursor.position = new Vector3(cursorPosition.x - 100, cursorPosition.y, cursorPosition.z);
            }
            else if (itemlistactive && items.Count > 0)
            {
                cursorPosition = items[cursorIndex].transform.position;
                cursor.position = new Vector3(cursorPosition.x - 75,cursorPosition.y,cursorPosition.z);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (itemlistactive && cursorIndex >= MenuOptions.Length - 1)
                {
                    cursorIndex = MenuOptions.Length - 1;
                }
                else if (itemlistactive && cursorIndex >= items.Count - 1)
                {
                    if (items.Count == 0)
                    {
                        cursorIndex = 0;
                    }
                    else
                    {
                        cursorIndex = items.Count - 1;
                    }
                }
                else
                cursorIndex++;

                if (itemlistactive && items.Count > 0)
                {
                    scrollVertical.value -= (1f / (items.Count - 1));
                    UpdateDescription();
                }
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (cursorIndex == 0)
                {
                    cursorIndex = 0;
                }
                else
                cursorIndex--;

                if (itemlistactive && items.Count > 0)
                {
                    scrollVertical.value += (1f / (items.Count - 1));
                    UpdateDescription();
                }
            }

            if (Input.GetButtonDown("Submit") && !itemlistactive)
            {
                optionPanel.SetActive(false);
                itemList.SetActive(true);
                Refreshitemlist();
                UpdateItemList(cursorIndex);
                cursorIndex = 0;
                if (items.Count > 0)
                {
                    UpdateDescription();
                    itemlistactive = true;
                }
            }
            else if (Input.GetButtonDown("Submit") && itemlistactive)
            {
                if (items.Count > 0)
                {
                    useitem();
                }
            }
        }
    }

    public void useitem()
    {
        if (items[cursorIndex].weapon != null)
        {
            player.addweapon(items[cursorIndex].weapon);
        }
        else if (items[cursorIndex].consumableItem != null)
        {
            player.UseItem(items[cursorIndex].consumableItem);
            inventory.RemoveItem(items[cursorIndex].consumableItem);
            cursorIndex = 0;
            Refreshitemlist();
            UpdateItemList(2);
            scrollVertical.value = 1;
        }
        else if (items[cursorIndex].armor != null)
        {
            player.addarmor(items[cursorIndex].armor);
        }
        updateatributes();
        UpdateDescription();
    }

    void UpdateDescription()
    {
        if (items[cursorIndex].weapon != null)
        {
            DescriptionText.text = items[cursorIndex].weapon.description;
        }
        else if (items[cursorIndex].consumableItem != null)
        {
            DescriptionText.text = items[cursorIndex].consumableItem.description;
        }
        else if (items[cursorIndex].key != null)
        {
            DescriptionText.text = items[cursorIndex].key.description;
        }
        else if (items[cursorIndex].armor != null)
        {
            DescriptionText.text = items[cursorIndex].armor.description;
        }
    }

    void Refreshitemlist()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }
        items.Clear();
    }
    void UpdateItemList(int option)
    {
        if (option == 0)
        {
            for (int i = 0; i < inventory.weapons.Count; i++)
            {
                GameObject tempitem = Instantiate(itemListPrefab, content.transform);
                tempitem.GetComponent<ItemList>().SetupWeapon(inventory.weapons[i]);
                items.Add(tempitem.GetComponent<ItemList>());
            }
        }
        else if (option == 1)
        {
            for (int i = 0; i < inventory.armors.Count; i++)
            {
                GameObject tempitem = Instantiate(itemListPrefab, content.transform);
                tempitem.GetComponent<ItemList>().SetupArmor(inventory.armors[i]);
                items.Add(tempitem.GetComponent<ItemList>());
            }
        }
        else if (option == 2)
        {
            for (int i = 0; i < inventory.items.Count; i++)
            {
                GameObject tempitem = Instantiate(itemListPrefab, content.transform);
                tempitem.GetComponent<ItemList>().SetUpItem(inventory.items[i]);
                items.Add(tempitem.GetComponent<ItemList>());
            }
        }
        else if (option == 3)
        {
            for (int i = 0; i < inventory.keys.Count; i++)
            {
                GameObject tempitem = Instantiate(itemListPrefab, content.transform);
                tempitem.GetComponent<ItemList>().Setupkey(inventory.keys[i]);
                items.Add(tempitem.GetComponent<ItemList>());
            }
        }
    }

    void updateatributes()
    {
        heathText.text = "Heath: " + player.getheath() + "/" + player.maxHealth;
        manatext.text = "Mana: " + player.getmana() + "/" + player.maxMana;
        forcetext.text = "Force: " + player.force;
        attacktext.text = "Attack: " + (player.force + player.GetComponentInChildren<Attack>().getdamage());
        defensetext.text = "Defense: " + player.defense;
    }

    public void UpdateUI()
    {
        heathUi.text = player.getheath() + " / " + player.maxHealth;
        manaUi.text = player.getmana() + " / " + player.maxMana;
        soulUi.text = "Soul: " + player.souls;
        potionUi.text = "x" + inventory.CountItem(player.items);
    }

    public void SetMessage(string message)
    {
        messagetext.text = message;
        Color color = messagetext.color;
        color.a = 0;
        messagetext.color = color;
        isMessagetext = true;
    }

}
