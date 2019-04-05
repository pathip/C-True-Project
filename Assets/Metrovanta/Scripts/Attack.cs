using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator anim;
    private int damage;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void playanimation(AnimationClip clip)
    {
        anim.Play(clip.name);

    }

    public void setweapon(int damagevalue)
    {
        damage = damagevalue;
    }

    public int getdamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage + FindObjectOfType<NewBehaviourScript>().force);
        }
    }
}
