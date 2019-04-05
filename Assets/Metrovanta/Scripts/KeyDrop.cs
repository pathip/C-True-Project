using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDrop : MonoBehaviour
{
    public Key key;


    private SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = key.image;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        NewBehaviourScript playable = other.GetComponent<NewBehaviourScript>();
        if (playable != null)
        {
            Inventory.inventory.AddKey(key);
            FindObjectOfType<UIManager>().SetMessage(key.message);
            Destroy(gameObject);
        }
    }
}
