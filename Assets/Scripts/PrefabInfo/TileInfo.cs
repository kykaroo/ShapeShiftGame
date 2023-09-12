using Level;
using UnityEngine;

namespace PrefabInfo
{
    public class TileInfo : MonoBehaviour
    {
        [field:SerializeField] public float TileZSize { get; private set; }
        [SerializeField] private VictoryTrigger finishTileVictoryTrigger;
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;

        public Vector3 End => end.position;

        public VictoryTrigger FinishTileVictoryTrigger => finishTileVictoryTrigger;

        public Vector3 ConnectStartToCurrentEnd
        {
            set => transform.position = value + (transform.position - start.position);
        }
    }
}