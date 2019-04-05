using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed;
    public int health;
    public GameObject itemdrop;
    public Consumable item;
    public int damage;
    public int soul;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 playerDistance;
    private bool facingright = false;
    private bool Isdead = false;
    private SpriteRenderer sprite;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Isdead)
        {
            playerDistance = player.transform.position - transform.position;
            if (Mathf.Abs(playerDistance.x) < 12 && Mathf.Abs(playerDistance.y) < 3)
            {
                rb.velocity = new Vector2(speed * (playerDistance.x / Mathf.Abs(playerDistance.x)), rb.velocity.y);
            }

            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

            if (rb.velocity.x > 0 && !facingright)
            {
                Flip();
            }
            else if (rb.velocity.x < 0 && facingright)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        facingright = !facingright;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Isdead = true;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Dead");
            FindObjectOfType<NewBehaviourScript>().souls += soul;
            FindObjectOfType<UIManager>().UpdateUI();
            if (item != null)
            {
                GameObject tempitem = Instantiate(itemdrop,transform.position,transform.rotation);
                tempitem.GetComponent<ItemDrop>().item = item;
            }
        }
        else
        {
            StartCoroutine(DamageCoroutine());
        }
    }

    IEnumerator DamageCoroutine()
    {
        for (float i = 0; i < 0.2f; i += 0.2f)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DestoryEnemy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        NewBehaviourScript player = other.gameObject.GetComponent<NewBehaviourScript>();
        if (player != null)
        {
            player.Takedamage(damage);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10 * (playerDistance.x / Mathf.Abs(playerDistance.x)), ForceMode2D.Impulse);
        }
    }
}
