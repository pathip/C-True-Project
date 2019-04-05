using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public Weapon weapon;


    private SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = weapon.image;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        NewBehaviourScript playable = other.GetComponent<NewBehaviourScript>();
        if (playable != null)
        {
           playable.addweapon(weapon);
           Inventory.inventory.Addweapon(weapon);
           FindObjectOfType<UIManager>().SetMessage(weapon.message);
           Destroy(gameObject);
        }
    }
}
