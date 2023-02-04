using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hsinpa {
    public class InsectMapBuilder : MonoBehaviour
    {
        [SerializeField]
        private Texture2D insect_mask;

        [SerializeField]
        private MapObject energy_prefab;

        [SerializeField]
        private MapObject barricade_prefab;

        [SerializeField]
        private int border_width = 1;

        [SerializeField]
        private Vector3 offset_position;

        private System.Random m_random_engine;

        List<MapObject> _map_objects = new List<MapObject>();

        private void Awake()
        {
            m_random_engine = new System.Random();

            MapStruct.MapBuilerOption option = new MapStruct.MapBuilerOption();
            option.energy_spawn_trial = 25;
            option.energy_spawn_size_max = 0.4f;
            option.energy_spawn_size_min = 0.1f;

            option.barricade_spawn_trial = 10;
            option.barricade_spawn_size_max = 2.5f;
            option.barricade_spawn_size_min = 1f;

            Debug.Log($"Width {insect_mask.width}, Height {insect_mask.height}");


            float aspect_ratio = insect_mask.height / (float) insect_mask.width;
            int width = border_width;
            int height = Mathf.RoundToInt(width * aspect_ratio);

            generateMap(energy_prefab, option.energy_spawn_trial, option.energy_spawn_size_max, option.energy_spawn_size_min, width, height, offset_position);

            generateMap(barricade_prefab, option.barricade_spawn_trial, option.barricade_spawn_size_max, option.barricade_spawn_size_min, width, height, offset_position);
        }

        public void generateMap(MapObject targetPrefab, int spawnTrial, float sizeMax, float sizeMin, int width, int height, Vector2 map_position) {
            int texture_width = insect_mask.width;
            int texture_height = insect_mask.height;

            //Spawn Energy
            for (int i = 0; i < spawnTrial; i++)
            {
                int mapObjectLens = _map_objects.Count;
                int pixel_x = m_random_engine.Next(texture_width);
                int pixel_y = m_random_engine.Next(texture_height);

                float random_size = Random.Range(sizeMin, sizeMax);

                Color mask = this.insect_mask.GetPixel(pixel_x, pixel_y);

                if (mask.r > 0.1 || mask.a < 0.5) continue;

                float x_ratio = pixel_x / (float)texture_width;
                float y_ratio = pixel_y / (float)texture_height;

                float spawn_x = x_ratio * width;
                float spawn_y = y_ratio * height;

                var spawnObject =  Instantiate(targetPrefab, this.transform);
                spawnObject.transform.localScale = new Vector3(random_size, random_size, random_size);
                spawnObject.transform.position = new Vector2( spawn_x + map_position.x, spawn_y + map_position.y);

                //spawnObject.spriteRenderer.bounds.Intersects

                bool collide_flag = false; 
                for (int k = 0; k < mapObjectLens; k++) {
                    if (spawnObject.spriteRenderer.bounds.Intersects(_map_objects[k].spriteRenderer.bounds)) {
                        collide_flag = true;
                        break;
                    }
                }

                if (collide_flag) {
                    GameObject.Destroy(spawnObject.gameObject);

                    continue;
                }

                _map_objects.Add(spawnObject);
            }
        }

        private void OnDrawGizmos()
        {
            float aspect_ratio = insect_mask.height / (float)insect_mask.width;
            int width = border_width;
            int height = Mathf.RoundToInt(width * aspect_ratio);

            Vector3 center = new Vector3(
                offset_position.x + (width * 0.5f),
                offset_position.y + (height * 0.5f), 1
            );

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(center, new Vector3(width, height, 1));
        }

    }
}
