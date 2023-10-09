using UnityEngine;

namespace PrefabInfo
{
    public class BackgroundInfo : MonoBehaviour
    {
        [SerializeField] private Terrain terrain;

        public Vector3 StartPosition
        {
            set => transform.position = value;
        }

        public Vector3 EndPosition => transform.position + Vector3.forward * terrain.terrainData.size.z;
    }
}