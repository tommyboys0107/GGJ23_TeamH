
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayer
{
    [SerializeField]
    private float moveSpeed = 4f;

    [SerializeField]
    private TMP_Text nutritionText;

    [SerializeField]
    private float currentNutrition;

    // Update is called once per frame
    void Update()
    {
        nutritionText.text = currentNutrition.ToString();

        Move();

        if (Input.GetKeyDown(KeyCode.K))
        {
            currentNutrition -= 1;
        }

        if(currentNutrition <= 0)
        {
            SendEndGame();
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector2(horizontalInput, verticalInput) * moveSpeed * Time.deltaTime);
    }

    public void AddNutrition(float value)
    {
        currentNutrition += value;
    }

    public void RemoveNutrition(float value)
    {

    }

    public void SpeedUP()
    {

    }

    public void SendEndGame()
    {
        gameObject.SendMessage("EndGame");
    }
}
