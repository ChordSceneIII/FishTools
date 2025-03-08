using UnityEngine;
using System.Collections.Generic;

namespace FishTools.RayCast
{
    /// <summary>
    /// TODO: 圆形扫描器，之后再改进
    /// </summary>
    public class CircleDector2D : MonoBehaviour
    {
        [Header("扫描配置")]
        [Tooltip("扫描起始世界坐标")]
        public Vector2 scanOrigin;

        [Tooltip("最大扫描半径")]
        [Min(0.1f)] public float scanRadius = 5f;

        [Tooltip("每圈扫描点数")]
        [Range(3, 720)] public int samplesPerCircle = 36;

        [Tooltip("检测层级")]
        public LayerMask detectionMask;

        [Header("调试显示")]
        [Tooltip("显示扫描轨迹")]
        public bool showScanPath = true;

        [Tooltip("有效点连接颜色")]
        public Color connectedColor = Color.green;

        [Tooltip("断点颜色")]
        public Color breakColor = Color.red;

        // 扫描结果数据
        public List<Vector2> _hitPoints = new List<Vector2>();
        public List<int> _breakIndices = new List<int>();

        private void Update()
        {
            PerformRadarScan();
        }

        void PerformRadarScan()
        {
            scanOrigin = transform.position;
            _hitPoints.Clear();
            _breakIndices.Clear();

            float angleStep = 360f / samplesPerCircle;
            bool wasHitting = false;

            for (int i = 0; i <= samplesPerCircle; i++)
            {
                float currentAngle = i * angleStep;
                Vector2 dir = AngleToDirection(currentAngle);

                RaycastHit2D hit = Physics2D.Raycast(
                    scanOrigin,
                    dir,
                    scanRadius,
                    detectionMask
                );

                if (hit.collider != null)
                {
                    _hitPoints.Add(hit.point);
                    if (!wasHitting) wasHitting = true;
                }
                else
                {
                    if (wasHitting)
                    {
                        _breakIndices.Add(_hitPoints.Count - 1);
                        wasHitting = false;
                    }
                }
            }
        }

        Vector2 AngleToDirection(float degrees)
        {
            return Quaternion.Euler(0, 0, degrees) * Vector2.up;
        }

        private void OnDrawGizmos()
        {
            if (!showScanPath) return;
            scanOrigin = transform.position;

            // 绘制扫描原点
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(scanOrigin, 0.2f);

            // 绘制有效线段
            if (_hitPoints.Count > 1)
            {
                int segmentStart = 0;

                foreach (int breakIndex in _breakIndices)
                {
                    DrawSegment(segmentStart, breakIndex);
                    segmentStart = breakIndex + 1;
                }

                // 绘制最后一段
                if (segmentStart < _hitPoints.Count)
                {
                    DrawSegment(segmentStart, _hitPoints.Count - 1);
                }
            }
        }

        void DrawSegment(int start, int end)
        {
            Gizmos.color = connectedColor;
            for (int i = start; i < end; i++)
            {
                Gizmos.DrawLine(_hitPoints[i], _hitPoints[i + 1]);
            }

            // 绘制断点标记
            if (end < _hitPoints.Count - 1)
            {
                Gizmos.color = breakColor;
                Gizmos.DrawWireCube(_hitPoints[end], Vector3.one * 0.15f);
            }
        }

        // 自动同步到物体位置
        private void Reset()
        {
            scanOrigin = transform.position;
        }
    }
}