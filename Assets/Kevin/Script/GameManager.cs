using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Utility;

public class GameManager : MonoBehaviour
{
    public static GameManager instanceRef;

    [SerializeField]
    private UIManager uIManager;

    private bool isStarted;

    private bool isEnded;


    private void OnEnable()
    {
        SimpleEventSystem.CustomEventListener += OnGameOver;
    }

    private void OnDisable()
    {
        SimpleEventSystem.CustomEventListener -= OnGameOver;
    }


    void Awake()
    {
        if (instanceRef == null)  //this code hasn't been executed before
        {
            instanceRef = this; //point to object instance currently executing this code
            DontDestroyOnLoad(gameObject); //don't destroy the gameObject this is attached to
        }
        else  //this object is not the first, it's an imposter that must be destroyed
        {
            DestroyImmediate(gameObject);
            Debug.Log("Destroy GameManager Imposter");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isStarted)
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isEnded)
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        isStarted = true;
        uIManager.ShowStartText();
    }

    public void EndGame()
    {
        isEnded = true;
        uIManager.ShowEndText();

    }

    public void OnGameOver(int tag, object[] objects)
    {
        if(tag == (int)SimpleEventSystem.Tag.OnGameOver)
        {
            EndGame();
        }
    }

}
