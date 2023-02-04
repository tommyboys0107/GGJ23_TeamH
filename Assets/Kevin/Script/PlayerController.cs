
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hsinpa.Utility;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4f;

    [SerializeField]
    private TMP_Text nutritionText;

    [SerializeField]
    private float currentNutrition;

    private void OnEnable()
    {
        SimpleEventSystem.CustomEventListener += AddNutrition;
        SimpleEventSystem.CustomEventListener += ReturnToLastCheckPoint;
    }

    private void OnDisable()
    {
        SimpleEventSystem.CustomEventListener -= AddNutrition;
        SimpleEventSystem.CustomEventListener -= ReturnToLastCheckPoint;
    }

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

    public void AddNutrition(int tag, object[] value)
    {
        if(tag == (int)SimpleEventSystem.Tag.AddNutrition)
        {
            currentNutrition += (float)value[0];
        }
        
    }


    public void ReturnToLastCheckPoint(int tag, object[] objects)
    {
        if(tag == (int)SimpleEventSystem.Tag.OnObstacleHit)
        {
            Debug.Log("The Head Has Returned to the Last Check Point");
        }
        
    }

    public void SendEndGame()
    {
        SimpleEventSystem.Send((int)SimpleEventSystem.Tag.OnGameOver);
    }
}
