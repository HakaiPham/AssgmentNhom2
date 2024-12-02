using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    
    // Vùng sát thương
    public Collider atkZone;
    
    public int damage;
    public string targetTag;
    
    public List<Collider> list_Damage = new List<Collider>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        atkZone.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag && !list_Damage.Contains(other))
        {
            list_Damage.Add(other);
            if (other.tag == "Player")
            {
                //other.GetComponent<PlayerMotor>().TakeDamage(damage);
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == targetTag && !list_Damage.Contains(other))
        {
            list_Damage.Add(other);
            if (other.tag == "Player")
            {
                //other.GetComponent<PlayerMotor>().TakeDamage(damage);
            }
            
        }
    }


    public void beginDamage()
    {
        list_Damage.Clear();
        atkZone.enabled = true;
        
    }

    public void endDamage()
    {
        list_Damage.Clear();
        atkZone.enabled = false;
    }
        
}