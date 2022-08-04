using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    public class FenceGenerator : MonoBehaviour
    {
        [SerializeField] List<FenceData> _fences = new List<FenceData>();
        [SerializeField] List<FencePiece> _fencePieces;
        [SerializeField] string _fencePrefix = "fence";
        [SerializeField] int _fenceMaxLength = 10000;

        public void BuildFences()
        {
            foreach (var fence in _fences)
            {
                for (int j = 0; j < fence.Parent.childCount; j++)
                    if (fence.Parent.GetChild(j).name.StartsWith(_fencePrefix))
                        DestroyImmediate(fence.Parent.GetChild(j).gameObject);

                float dist = Vector3.Distance(fence.Point1.position, fence.Point2.position);
                Vector3 dir = (fence.Point2.position - fence.Point1.position).normalized;

                float distSum = 0f;
                int i = 0;

                while (i < _fenceMaxLength)
                {
                    FencePiece piece = _fencePieces.GetRandomItem();
                    GameObject go = Instantiate(piece.Prefab, fence.Parent);

                    go.transform.position = fence.Point1.position + dir * (distSum + piece.Length * 0.5f);
                    go.transform.right = dir;

                    distSum += piece.Length;
                    if (distSum > dist)
                        break;
                }
            }
        }
    }

    [System.Serializable]
    struct FenceData
    {
        public Transform Parent;
        public Transform Point1;
        public Transform Point2;
    }

    [System.Serializable]
    struct FencePiece
    {
        public GameObject Prefab;
        public float Length;
    }
}
