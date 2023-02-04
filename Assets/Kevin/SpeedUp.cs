using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Nutrition
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player")
        {
            Debug.Log("Enter");

            if (!isProviding)
            {
                //StartCoroutine(ProvideNutritionRoutine(collision.gameObject.GetComponent<PlayerController>()));

                isProviding = true;
            }
        }
    }
}
