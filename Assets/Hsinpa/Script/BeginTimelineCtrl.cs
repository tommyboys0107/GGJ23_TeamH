using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BeginTimelineCtrl : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector timelime;

    [SerializeField]
    private Hsinpa.InsectMapBuilder insectMapBuilder;

    [SerializeField]
    private GameObject branch_player;

    public void Start()
    {

        timelime.stopped += (x) =>
        {
            Debug.Log("Timeline Stop");

            if (insectMapBuilder != null)
                insectMapBuilder.AutoBuild();

            if (branch_player != null)
                branch_player.gameObject.SetActive(true);
        };

    }

}
