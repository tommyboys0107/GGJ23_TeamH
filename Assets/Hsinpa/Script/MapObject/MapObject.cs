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

        public int original_energy;
        public int energy;

        public Vector2 size;
    }
}
