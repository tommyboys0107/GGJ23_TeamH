using Hsinpa;
using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MapObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Collider2D>().tag == "Player" && transform.name == "Player")
        {
            Hsinpa.Utility.SimpleEventSystem.Send(Hsinpa.GeneralStaticFlag.EventFlag.GameFailEvent, collision.transform.parent.GetComponent<GM>());

        }
    }
}
