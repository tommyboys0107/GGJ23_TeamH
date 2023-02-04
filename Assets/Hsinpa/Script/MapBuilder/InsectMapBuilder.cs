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
        private float border_width = 1;

        [SerializeField]
        private GameObject insectTextureHolder;

        [SerializeField]
        private Vector3 offset_position;

        [Header("Object Config")]
        [SerializeField, Range(1, 200)]
        private int energy_spawn_trial = 10;

        [SerializeField, Range(0.01f, 5)]
        private float energy_max_size = 1;

        [SerializeField, Range(0.01f, 5)]
        private float energy_min_size = 1;

        [SerializeField, Range(1, 200)]
        private int barricade_spawn_trial = 10;

        [SerializeField, Range(0.01f, 5)]
        private float barricade_max_size = 1;

        [SerializeField, Range(0.01f, 5)]
        private float barricade_min_size = 1;

        private System.Random m_random_engine;

        [SerializeField]
        private bool debugModeFlag = false;


        public float width => border_width;
        public float height { get {
                float aspect_ratio = insect_mask.height / (float)insect_mask.width;
                return (border_width * aspect_ratio);
            }
        }

        private Vector3 _center = new Vector3();

        public Vector3 center
        {
            get
            {
                _center.Set(offset_position.x + (width * 0.5f), offset_position.y + (height * 0.5f), 1);

                return _center;
            }
        }

        List<MapObject> _map_objects = new List<MapObject>();

        public void Start()
        {
            if (debugModeFlag)
                AutoBuild();
        }

        public void AutoBuild()
        {
            m_random_engine = new System.Random();

            MapStruct.MapBuilerOption option = new MapStruct.MapBuilerOption();
            option.energy_spawn_trial = energy_spawn_trial;
            option.energy_spawn_size_max = energy_max_size;
            option.energy_spawn_size_min = energy_min_size;

            option.barricade_spawn_trial = barricade_spawn_trial;
            option.barricade_spawn_size_max = barricade_max_size;
            option.barricade_spawn_size_min = barricade_min_size;

            Debug.Log($"Width {insect_mask.width}, Height {insect_mask.height}");

            float aspect_ratio = insect_mask.height / (float) insect_mask.width;
            float width = border_width;
            float height = (width * aspect_ratio);

            generateMap(energy_prefab, option.energy_spawn_trial, option.energy_spawn_size_max, option.energy_spawn_size_min, width, height, offset_position);
            generateMap(barricade_prefab, option.barricade_spawn_trial, option.barricade_spawn_size_max, option.barricade_spawn_size_min, width, height, offset_position);

            if (insectTextureHolder != null)
                insectTextureHolder.gameObject.SetActive(true);
        }

        public void generateMap(MapObject targetPrefab, int spawnTrial, float sizeMax, float sizeMin, float width, float height, Vector2 map_position) {
            int texture_width = insect_mask.width;
            int texture_height = insect_mask.height;

            //Spawn Energy
            for (int i = 0; i < spawnTrial; i++)
            {
                int mapObjectLens = _map_objects.Count;

                //Avoid barricade at start point
                float threshold = 0.9f;
                int pixel_x = Random.Range(0, Mathf.RoundToInt(texture_width * threshold));
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
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(center, new Vector3(width, height, 1));
        }
    }
}
