using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Nutrition : MonoBehaviour
{
    [SerializeField]
    protected float duration;
   
    [SerializeField]
    private float interval;

    [SerializeField]
    private float totalNutritionValue, nutritionOverTime, maxNutritionValue;

    protected float scaleMultiplier;

    protected Vector3 currentScale;

    [SerializeField]
    protected Collider2D thisCollider;

    protected bool isProviding;

    // Start is called before the first frame update
    void Start()
    {
        if (!thisCollider)
        {
            Debug.LogWarning($"Collder2D doesn't exist on  { transform.name}");
        }

        currentScale = transform.localScale;

        maxNutritionValue = totalNutritionValue;
    }



    private IEnumerator ProvideNutritionRoutine(IPlayer playerController)
    {
        while (totalNutritionValue > 0)
        {
            playerController.AddNutrition(nutritionOverTime);

            totalNutritionValue -= nutritionOverTime;

            scaleMultiplier =  totalNutritionValue / maxNutritionValue;

            transform.localScale = currentScale * scaleMultiplier ;

            if (totalNutritionValue == 0)
            {
                gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(interval);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player")
        {
            Debug.Log("Enter");

            if (!isProviding)
            {
                StartCoroutine(ProvideNutritionRoutine(collision.gameObject.GetComponent<PlayerController>()));

                isProviding = true;
            }
        }
    }





}
