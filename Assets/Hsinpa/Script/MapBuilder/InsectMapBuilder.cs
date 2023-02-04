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

        private System.Random m_random_engine;

        List<MapObject> _map_objects = new List<MapObject>();

        private void Awake()
        {
            m_random_engine = new System.Random();

            MapStruct.MapBuilerOption option = new MapStruct.MapBuilerOption();
            option.energy_spawn_trial = 25;
            option.energy_spawn_size = 5;

            option.barricade_spawn_trial = 10;
            option.barricade_spawn_size = 3;


            int width = 10;
            int height = 10;

            generateMap(energy_prefab, option.energy_spawn_trial, option.energy_spawn_size, width, height, new Vector2(-width * 0.5f, -height * 0.5f));
            generateMap(barricade_prefab, option.barricade_spawn_trial, option.barricade_spawn_size, width, height, new Vector2(-width * 0.5f, -height * 0.5f));
        }

        public void generateMap(MapObject targetPrefab, int spawnTrial, int spawnSize, int width, int height, Vector2 map_position) {
            int texture_width = insect_mask.width;
            int texture_height = insect_mask.height;

            //Spawn Energy
            for (int i = 0; i < spawnTrial; i++)
            {
                int mapObjectLens = _map_objects.Count;
                int pixel_x = m_random_engine.Next(texture_width);
                int pixel_y = m_random_engine.Next(texture_height);

                int random_size = m_random_engine.Next(spawnSize - 1) + 1;

                Color mask = this.insect_mask.GetPixel(pixel_x, pixel_y);

                if (mask.r > 0.1) continue;

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



    }
}
