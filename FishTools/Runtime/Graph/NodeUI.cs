using UnityEngine;
using UnityEngine.EventSystems;
namespace FishTools.Graph
{
    [RequireComponent(typeof(RectTransform))]
    public class NodeUI : MonoBehaviour
    {
        [ReadOnly, SerializeField] private int nid;
        [SerializeField] private Transform _inport;
        [SerializeField] private Transform _outport;
        public Transform inport => _inport;
        public Transform outport => _outport;
        private RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (FishUtility.IsNull(_rectTransform))
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }

        public int NID
        {
            get => nid;
            set => nid = value;
        }

        private void Start()
        {
            if (inport == null)
            {
                Debug.LogError($"{name} 入口 未设置");
            }
            if (outport == null)
            {
                Debug.LogError($"{name} 出口 未设置");
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }
    }
}
