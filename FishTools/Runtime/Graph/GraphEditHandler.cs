using FishTools;
using FishTools.Graph;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FishEditor.Graph
{
    /// <summary>
    /// 用于编辑连接
    /// </summary>
    [RequireComponent(typeof(GraphUI))]
    public class GraphEditHandler : MonoBehaviour, IPointerClickHandler
    {
        public ConnectionUI preview_connection;
        public int weight = 1;
        [ReadOnly] public NodeUI start;
        [ReadOnly] public NodeUI end;
        [ReadOnly, SerializeField] private GraphUI _graphUI;
        public GraphUI graphUI
        {
            get
            {
                if (_graphUI == null)
                    _graphUI = GetComponent<GraphUI>();
                return _graphUI;
            }
        }
        public static ConnectionUI cur_preview;

        private void Update()
        {
            if (cur_preview != null)
            {
                cur_preview?.SetPosition(start.outport.position, Input.mousePosition);
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (cur_preview != null)
                {
                    DestroyImmediate(cur_preview.gameObject);
                    start = null;
                    end = null;
                }
            }
        }

        public void SetStart()
        {
            //防止重复创建
            if (cur_preview != null) DestroyImmediate(cur_preview.gameObject);

            cur_preview = Instantiate(preview_connection, graphUI.ConnectionObjects.transform);
        }

        public void SetEnd(NodeUI node)
        {
            DestroyImmediate(cur_preview.gameObject);

            //如果已经连接，则断开
            if (graphUI.lines.ContainsKey(new Connection(start.NID, end.NID)))
            {
                graphUI.DisConnect(new Connection(start.NID, end.NID));
                start = null;
                end = null;
                return;
            }

            //如果没有连接，则连接
            graphUI.Connect(new Connection(start.NID, end.NID), weight);
            start = null;
            end = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var node = eventData.pointerCurrentRaycast.gameObject.GetComponent<NodeUI>();
            if (node != null)
            {
                if (start == null)
                {
                    start = node;
                    SetStart();
                    return;
                }

                if (end == null && node != start)
                {
                    end = node;
                    SetEnd(node);
                    return;
                }

            }
        }
    }
}