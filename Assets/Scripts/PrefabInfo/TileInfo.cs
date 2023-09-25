using Level;
using UnityEngine;

namespace PrefabInfo
{
    public class TileInfo : MonoBehaviour
    {
        [SerializeField] private Transform tileStart;
        [SerializeField] private Transform tileEnd;
        [Header("Finish tile only")]
        [SerializeField] private VictoryTrigger finishTileVictoryTrigger;
        [Header("Water tile only")]
        public GameObject waterEndRamp;

        public Vector3 End => tileEnd.position;

        public VictoryTrigger FinishTileVictoryTrigger => finishTileVictoryTrigger;

        public Vector3 ConnectStartToCurrentEnd
        {
            set => transform.position = value + (transform.position - tileStart.position);
        }
    }
}