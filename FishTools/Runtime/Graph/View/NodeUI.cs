using UnityEngine;
using UnityEngine.EventSystems;
namespace FishTools.Graph
{
    /// <summary>
    /// 节点UI
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class NodeUI : MonoBehaviour
    {
        [ReadOnly, SerializeField] private int nid;
        [SerializeField] private Transform _inport;
        [SerializeField] private Transform _outport;
        public Transform inport => _inport;
        public Transform outport => _outport;
        private RectTransform _rectTransform;
        public RectTransform rectTransform => FishUtility.LazyGet(this, ref _rectTransform);

        public int NID
        {
            get => nid;
            set => nid = value;
        }

        private void Start()
        {
            if (inport == null)
            {
                _inport = transform;
            }
            if (outport == null)
            {
                _outport = transform;
            }
        }
    }
}
