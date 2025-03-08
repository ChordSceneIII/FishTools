using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace FishTools.Graph
{
    /// <summary>
    /// 连接UI，控制其形变
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class ConnectionUI : MonoBehaviour
    {
        [SerializeField, ReadOnly] private RectTransform _rectTransform;
        public RectTransform rectTransform => FishUtility.LazyGet(this, ref _rectTransform);
        public ObserveField<bool> flag = new ObserveField<bool>(false);
        public ObserveField<int> weight = new ObserveField<int>(0);
        [SerializeField] private float lineThickness = 10f;
        [SerializeField] private float offsetDistance = 200f;
        [Label("持续更新位置")] public bool updatePos = false;
        [ConditionalField("updatePos", true)] public Transform start;
        [ConditionalField("updatePos", true)] public Transform end;
        [ReadOnly] public Connection connection = default;
        public UnityEvent onFlagEvent;
        public UnityEvent offFlagEvent;

        private void OnEnable()
        {
            flag.AddListener(SetFlag);
        }
        private void OnDisable()
        {
            flag.RemoveListener(SetFlag);
        }

        public void SetFlag(bool flag)
        {
            if (flag)
                onFlagEvent?.Invoke();
            else
                offFlagEvent?.Invoke();

        }

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