using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Consumable item;

    private SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = item.image;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.inventory.AddItem(item);
            FindObjectOfType<UIManager>().UpdateUI();
            FindObjectOfType<UIManager>().SetMessage(item.message);
            Destroy(gameObject);
        }
    }
}
