using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GM gm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.transform.tag == "Nutrition")
        {
            gm.EnterPoint(collision.transform);
            collision.enabled = false;
            collision.transform.localScale = collision.transform.localScale * 0.5f;
        }
        if (collision.transform.tag == "Barricade")
        {
            gm.EnterDead();
            collision.gameObject.SetActive(false);
        }
    }
    
}
