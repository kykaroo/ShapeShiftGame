using Level;
using UnityEngine;

namespace PrefabInfo
{
    public class TileInfo : MonoBehaviour
    {
        [SerializeField] private Transform tileStart;
        [SerializeField] private Transform tileEnd;
        [Header("First tile only")]
        [SerializeField] private Transform playerStartPosition;
        [SerializeField] private Transform[] aiStartPositions;
        [Header("Finish tile only")]
        [SerializeField] private LevelEndTrigger finishTileLevelEndTrigger;
        [Header("Water tile only")]
        public GameObject waterEndRamp;

        public Vector3 End => tileEnd.position;

        public Transform PlayerStartPosition => playerStartPosition;

        public Transform[] AIStartPositions => aiStartPositions;

        public LevelEndTrigger FinishTileLevelEndTrigger => finishTileLevelEndTrigger;

        public Vector3 ConnectStartToCurrentEnd
        {
            set => transform.position = value + (transform.position - tileStart.position);
        }
    }
}