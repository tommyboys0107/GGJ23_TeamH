using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Nutrition : MonoBehaviour
{
    [SerializeField]
    private float duration, interval;

    [SerializeField]
    private float totalNutritionValue, nutritionOverTime, maxNutritionValue;

    private float scaleMultiplier;

    private Vector3 currentScale;

    [SerializeField]
    private Collider2D thisCollider;

    private bool isStarted;

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



    private IEnumerator ProvideNutritionRoutine(PlayerController playerController)
    {
        while (totalNutritionValue > 0)
        {
            playerController.AddNutriiton(nutritionOverTime);

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

            if (!isStarted)
            {
                StartCoroutine(ProvideNutritionRoutine(collision.gameObject.GetComponent<PlayerController>()));

                isStarted = true;
            }
        }
    }





}
