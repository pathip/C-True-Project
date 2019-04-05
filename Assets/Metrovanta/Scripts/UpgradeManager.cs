using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public Text upgrateCostText;
    public Text[] attributeText;
    public GameObject upgradePanel;

    private bool upgradeactive = false;
    private NewBehaviourScript player;
    private int cursorIndex;
    void Start()
    {
        player = FindObjectOfType<NewBehaviourScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            upgradeactive = !upgradeactive;
            cursorIndex = 0;
            UpdateText();
            if (upgradeactive)
            {
                upgradePanel.SetActive(true);
            }
            else
            {
                upgradePanel.SetActive(false);
            }
        }

        if (upgradeactive)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                UpdateText();
                cursorIndex++;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UpdateText();
                cursorIndex--;
            }

            if (cursorIndex == 0)
            {
                attributeText[0].text = "Heath: " + player.maxHealth + ">" + (player.maxHealth + (player.maxHealth * 0.1f));
                attributeText[0].color = Color.green; 
            }
            else if (cursorIndex == 1)
            {
                attributeText[1].text = "Mana: " + player.maxMana + ">" + (player.maxMana + (player.maxMana * 0.1f));
                attributeText[1].color = Color.green;
            }
            else if (cursorIndex == 2)
            {
                attributeText[2].text = "Force: " + player.force + ">" + (player.force + (player.force * 0.1f));
                attributeText[2].color = Color.green;
            }
        }
    }

    void UpdateText()
    {
        upgrateCostText.text = "Cost of Souls : " + GameManager.gm.upgradecost + " " + " Soul : " + player.souls;
        attributeText[0].text = "Heath: " + player.maxHealth;
        attributeText[1].text = "Mana: " + player.maxMana;
        attributeText[2].text = "Force: " + player.force;
        for (int i = 0; i < attributeText.Length; i++)
        {
            attributeText[i].color = Color.white;
        }
    }
}
