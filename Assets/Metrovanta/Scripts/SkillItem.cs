using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour
{
    public PlayerSkill skill;
    public string message;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        NewBehaviourScript player = other.GetComponent<NewBehaviourScript>();
        if (player != null)
        {
            player.SetPlayerSkill(skill);
            FindObjectOfType<UIManager>().SetMessage(message);
            Destroy(gameObject);
        }
    }
}
