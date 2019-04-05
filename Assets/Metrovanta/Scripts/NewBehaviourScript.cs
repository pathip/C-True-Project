using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerSkill
{
    dash , doublejump
}

public class NewBehaviourScript : MonoBehaviour
{
    public float maxspeed;
    public Transform groundCheck;
    public float jumpforce;
    public float firerate;
    public Consumable items;
    public int maxHealth;
    public int maxMana;
    public int force;
    public int defense;
    public int souls;
    public float dashforce;
    public bool DoubleJumpSkill = false;
    public bool DashSkill = false;

    private float speed;
    private Rigidbody2D rb;
    private bool FacingRight = true;
    private bool onground;
    private bool jump = false;
    private bool doublejump;
    public Weapon weaponepuipped;
    private Animator anim;
    private Attack attack;
    private float nextattack;
    private int health;
    private int mana;
    public Armor armor;
    private bool candamage = true;
    private SpriteRenderer sprite;
    private bool isdead = false;
    private bool dash = false;
    private GameManager gm;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attack = GetComponentInChildren<Attack>();
        sprite = GetComponent<SpriteRenderer>();
        gm = GameManager.gm;
        SetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isdead)
        {
            onground = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            if (onground)
            {
                doublejump = false;
            }
            //Jump and Double Jump
            if (Input.GetButtonDown("Jump") && (onground || (!doublejump && DoubleJumpSkill)))
            {
                jump = true;
                if (!doublejump && !onground)
                {
                    doublejump = true;
                }
            }
            //Attack
            if (Input.GetButtonDown("Fire1") && Time.time > nextattack && weaponepuipped != null)
            {
                dash = false;
                anim.SetTrigger("Attack");
                attack.playanimation(weaponepuipped.animation);
                nextattack = Time.time + firerate;
            }
            //Use Item
            if (Input.GetButtonDown("Fire3"))
            {
                UseItem(items);
                Inventory.inventory.RemoveItem(items);
                FindObjectOfType<UIManager>().UpdateUI();
            }
            //Back Dash
            if (Input.GetKeyDown(KeyCode.Q) && onground && !dash && DashSkill)
            {
                rb.velocity = Vector2.zero;
                anim.SetTrigger("Dash");
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isdead)
        {
            float h = Input.GetAxisRaw("Horizontal");


            anim.SetFloat("Speed",Mathf.Abs(h));


            if (candamage && !dash)
                rb.velocity = new Vector2(h * speed, rb.velocity.y);

            if (h > 0 && !FacingRight)
            {
                Flip();
            }
            else if (h < 0 && FacingRight)
            {
                Flip();
            }

            if (jump)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpforce);
                jump = false;
            }

            if (dash)
            {
                int hforce = FacingRight ? 1 : -1;
                rb.velocity = Vector2.left * dashforce * hforce;
            }
        }
    }

    void Flip()
    {
        FacingRight = !FacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void addweapon(Weapon weapon)
    {
        weaponepuipped = weapon;
        attack.setweapon(weaponepuipped.damage);
    }

    public void addarmor(Armor item)
    {
        armor = item;
        defense = armor.defence;
    }

    public void UseItem(Consumable item)
    {
        health += item.healthGain;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        mana += item.manaGain;
        if (mana >= maxMana)
        {
            mana = maxMana;
        }
    }

    public int getheath()
    {
        return health;
    }

    public int getmana()
    {
        return mana;
    }

    public void Takedamage(int damage)
    {
        if (candamage)
        {
            candamage = false;
            health -= (damage - defense);
            FindObjectOfType<UIManager>().UpdateUI();
            if (health <= 0)
            {
                anim.SetTrigger("Dead");
                Invoke("ReloadScene",3f);
                isdead = true;
            }
            else
            {
                StartCoroutine(DamageCoroutine());
            }
        }
    }
    void ReloadScene()
    {
        Souls.instance.gameObject.SetActive(true);
        Souls.instance.souls = souls;
        Souls.instance.transform.position = transform.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator DamageCoroutine()
    {
        for (float i = 0; i < 0.6f; i+=0.2f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        candamage = true;
    }

    public void DashTrue()
    {
        dash = true;
    }

    public void DashFalse()
    {
        dash = false;
    }

    public void SetPlayerSkill(PlayerSkill skill)
    {
        if (skill == PlayerSkill.dash)
        {
            DashSkill = true;
        }
        else if(skill == PlayerSkill.doublejump)
        {
            DoubleJumpSkill = true;
        }
    }

    public void SetPlayer()
    {
        Vector3 playerPos = new Vector3(gm.playerPosX,gm.playerPosY,0);
        transform.position = playerPos;
        maxHealth = gm.heath;
        maxMana = gm.mana;
        speed = maxspeed;
        health = maxHealth;
        mana = maxMana;
        force = gm.force;
        souls = gm.soul;
        doublejump = gm.canDoubleJump;
        dash = gm.canBackDash;
        if (gm.currentarmorId > 0)
        {
            addarmor(Inventory.inventory.itemdatabase.Getarmor(gm.currentarmorId));
        }

        if (gm.currentweaponId > 0)
        {
            addweapon(Inventory.inventory.itemdatabase.Getweapon(gm.currentweaponId));
        }
    }
}
