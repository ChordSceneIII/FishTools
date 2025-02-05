using TMPro;
using UnityEngine;
namespace FishTools.Graph
{
    [RequireComponent(typeof(RectTransform))]
    public class ConnectionUI : MonoBehaviour
    {
        [SerializeField, ReadOnly] private RectTransform _rectTransform;
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
        [ReadOnly] public Connection connection = default;
        public ObserveField<int> weight = new ObserveField<int>(0);
        [SerializeField] private float lineThickness = 2f;
        [SerializeField] private float offsetDistance = 1f;
        [Label("持续更新位置")] public bool updatePos = false;
        [ConditionalField("updatePos", true)] public Transform start;
        [ConditionalField("updatePos", true)] public Transform end;
        private void Update()
        {
            if (updatePos && start != null && end != null)
            {
                SetPosition(start.position, end.position);
            }
        }

        public void SetPosition(Transform start, Transform end)
        {
            this.start = start;
            this.end = end;
            SetPosition(start.position, end.position);
        }

        public void SetPosition(Vector2 start, Vector2 end)
        {
            Vector2 dir = end - start;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float length = dir.magnitude - offsetDistance;

            rectTransform.sizeDelta = new Vector2(length, lineThickness);
            rectTransform.position = start + dir * 0.5f;
            rectTransform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}