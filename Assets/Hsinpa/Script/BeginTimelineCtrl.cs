using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using DG.Tweening;
using TMPro;
using Hsinpa.Utility;
using System.Security.Cryptography;

public class BeginTimelineCtrl : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector timelime;

    [SerializeField]
    private Hsinpa.InsectMapBuilder insectMapBuilder;

    [SerializeField]
    private GameObject branch_player;

    [SerializeField]
    private Vector3 end_game_cam_position;

    [SerializeField]
    private TextMeshProUGUI end_game_text;

    [SerializeField]
    private float end_game_cam_size = 4;

    private bool game_end_flag = false;

    Camera _mainCamera;

    public void Start()
    {
        _mainCamera = Camera.main;

        timelime.stopped += (x) =>
        {
            Debug.Log("Timeline Stop");

            if (insectMapBuilder != null)
                insectMapBuilder.AutoBuild();

            if (branch_player != null)
                branch_player.gameObject.SetActive(true);
        };

        Hsinpa.Utility.SimpleEventSystem.CustomEventListener += Onstart;
    }


    private void OnDisable()
    {
        SimpleEventSystem.CustomEventListener -= Onstart;
    }

    public void Update()
    {
        if (game_end_flag && Input.GetKeyDown(KeyCode.Space)) {
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    private void OnBrainHitEvent(GM gm, bool isSuccess) {
        gm.enabled = false;

        float anim_duration = 2;
        _mainCamera.transform.DOMove(end_game_cam_position, anim_duration);
        _mainCamera.DOOrthoSize(end_game_cam_size, anim_duration).onComplete = () => {
            game_end_flag = true;

            if (end_game_text != null) end_game_text.gameObject.SetActive(true);
        };
    }

    private void Onstart(int id, object[] parameters)
    {

        if (id == Hsinpa.GeneralStaticFlag.EventFlag.GameSuccessEvent && parameters.Length == 1)
            OnBrainHitEvent((GM)parameters[0], true);

        if (id == Hsinpa.GeneralStaticFlag.EventFlag.GameFailEvent && parameters.Length == 1)
            OnBrainHitEvent((GM)parameters[0], false);
    }
}