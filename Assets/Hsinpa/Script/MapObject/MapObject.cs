using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa
{
    public class MapObject : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer spriteRenderer => _spriteRenderer;

        [SerializeField]
        protected Sprite[] sprites;

        private CircleCollider2D polygonCollider;

        public int original_energy;
        public int energy;

        public Vector2 size;

        private void Start()
        {
            // Set srpite
            if (sprites != null && sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

                polygonCollider = this.gameObject.AddComponent<CircleCollider2D>();
                polygonCollider.radius = polygonCollider.radius * 0.5f;
                polygonCollider.isTrigger = true;
            }

            //Set rotation
            //float rotation = Random.Range(-180, 180);
            //this.transform.Rotate(Vector3.right, rotation);
        }

    }
}
