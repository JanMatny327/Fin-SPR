using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    private Rigidbody2D rig2D;

    private void Awake()
    {
        rig2D = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            rig2D.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }
}
