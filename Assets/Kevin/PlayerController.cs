using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4f;

    [SerializeField]
    private TMP_Text nutritionText;

    [SerializeField]
    private float currentNutrition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nutritionText.text = currentNutrition.ToString();

        Move();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector2(horizontalInput, verticalInput) * moveSpeed * Time.deltaTime);
    }

    public void AddNutriiton(float value)
    {
        currentNutrition += value;
    }
}
