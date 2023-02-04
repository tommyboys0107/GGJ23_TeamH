using Hsinpa;
using Hsinpa.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MapObject
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Player")
        {

            SimpleEventSystem.Send((int)SimpleEventSystem.Tag.OnObstacleHit);
        }
    }
}
