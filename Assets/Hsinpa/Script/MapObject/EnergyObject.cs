using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa
{
    public class EnergyObject : MapObject
    {
        [SerializeField]
        private Sprite[] sprites;

        private void Start()
        {

            // Set srpite
            if (sprites != null && sprites.Length > 0) {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }

            //Set rotation
            //float rotation = Random.Range(-180, 180);
            //this.transform.Rotate(Vector3.right, rotation);
        }

    }
}