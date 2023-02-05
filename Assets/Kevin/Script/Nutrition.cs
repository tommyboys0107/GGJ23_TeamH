using System.Collections;
using UnityEngine;
using Hsinpa.Utility;
using Hsinpa;
using CliffLeeCL;

public class Nutrition : EnergyObject
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
    protected CircleCollider2D thisCollider;

    protected bool isProviding;

    int enterTimes;

    float enterMultiplier;
   

    // Start is called before the first frame update
    void Start()
    {
        if (!thisCollider)
        {
            //Debug.LogWarning($"Collder2D doesn't exist on  { transform.name}");
        }

        currentScale = transform.localScale;

        maxNutritionValue = totalNutritionValue;

        if (sprites != null && sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

            thisCollider = this.gameObject.AddComponent<CircleCollider2D>();
            thisCollider.isTrigger = true;
            thisCollider.radius = thisCollider.radius * 0.8f;
        }

        enterMultiplier = enterTimes * .55f;
    }

    private IEnumerator ProvideNutritionRoutine()
    {
        while (totalNutritionValue > 0)
        {
            SimpleEventSystem.Send((int)SimpleEventSystem.Tag.AddNutrition, nutritionOverTime * (1+enterMultiplier));

            totalNutritionValue -= nutritionOverTime * (1 + enterMultiplier);

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
            enterTimes++;

            enterMultiplier = enterTimes * 0.25f;

            if (!isProviding)
            {
                StartCoroutine(ProvideNutritionRoutine());

                isProviding = true;

                AudioManager.Instance.PlaySound(AudioManager.AudioName.EnergyHit);
            }
        }
    }





}
