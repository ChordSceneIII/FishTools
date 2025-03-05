using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FishTools.Graph
{
    [RequireComponent(typeof(GraphUI))]
    public class WalkSimulator : MonoBehaviour, IPointerClickHandler
    {
        [ReadOnly, SerializeField] private GraphUI _graphUI;
        public GraphUI graphUI => FishUtility.LazyGet(this, ref _graphUI);
        [Label("指示针")] public Sprite pointer_sprite;
        public bool isCompleted;

        [Header("状态")]
        [Label("可用节点")] public List<int> available_list = new List<int>();
        [Label("访问过的节点")] public List<int> visited_list = new List<int>();
        [Label("当前节点"), ReadOnly] public NodeUI current_node;
        [ReadOnly, SerializeField] private GameObject _pointer;
        public GameObject pointer
        {
            get
            {
                if (_pointer == null)
                {
                    _pointer = new GameObject("pointer");
                    _pointer.transform.SetParent(transform);
                    _pointer.AddComponent<Image>().sprite = pointer_sprite;
                }
                return _pointer;
            }
        }

        [SerializeField] private bool usePointerClick = true;

        [DrawButton("标记最短路径", true)]
        public void PrintShorest(int from, int to)
        {
            //从lines中读取数据
            Dictionary<Connection, int> connects = new Dictionary<Connection, int>();

            //根据dijstra算法计算得到最短路径
            List<Connection> path = Graph.DijkstraPath(graphUI.temp_connections, from, to);

            //打印最短路径
            foreach (var connect in path)
            {
                DebugF.Log($"{connect.from}-{connect.to}");
                graphUI.lines[connect].flag.field = true;
            }
        }

        [DrawButton("标记访问过的路线", true)]
        public void VisitedRoutes()
        {
            VisitedRoutes(visited_list);
        }

        public void VisitedRoutes(List<int> visited_nids)
        {
            if (visited_nids.Count <= 1) return;

            List<Connection> connects = new List<Connection>();

            //标记访问过的路线
            for (int index = 0; index < visited_nids.Count - 1; index++)
            {
                var connect = new Connection(visited_nids[index], visited_nids[index + 1]);
                connects.Add(connect);
            }

            //如果当前节点不为空，则标记上一节点节点到当前节点的路线
            if (current_node != null && current_node.NID != visited_nids.Last())
                connects.Add(new Connection(visited_nids.Last(), current_node.NID));

            foreach (var connect in connects)
            {
                graphUI.lines[connect].flag.field = true;
            }
        }

        [DrawButton("取消所有标记", true)]
        public void CancelAllFlags()
        {
            foreach (var line in graphUI.lines.Values)
            {
                line.flag.field = false;
            }
        }


        /// <summary>
        /// 设置可用节点
        /// </summary>
        public void SetNodeAvailable(List<int> list)
        {

            //取消原来的Available标记
            if (available_list.Count > 0)
                foreach (var nid in available_list)
                {
                    var iwalk = graphUI.nodes[nid].GetComponent<IWalkedEvent>();
                    if (iwalk != null)
                    {
                        iwalk.isAvailable = false;
                    }
                }

            //更新available_list
            available_list = new List<int>(list);

            //设置新的Available标记
            if (available_list.Count > 0)
                foreach (var nid in available_list)
                {
                    var iwalk = graphUI.nodes[nid].GetComponent<IWalkedEvent>();
                    if (iwalk != null)
                    {
                        iwalk.isAvailable = true;
                    }
                }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(List<int> available_nids, List<int> visited_nids)
        {
            //初始化地图
            CancelAllFlags();
            Destroy(pointer);
            SetNodeAvailable(available_nids);
            visited_list = new List<int>(visited_nids);
            current_node = null;

            //设置初始可访问节点

            //初始化所有Iwalk接口
            foreach (var node in graphUI.nodes.Values)
            {
                var iwalk = node.GetComponent<IWalkedEvent>();
                if (iwalk != null)
                {
                    iwalk.simulator = this;
                    iwalk.isAvailable = false;
                }
            }

            //初始化初始Iwalk接口
            foreach (var nid in available_nids)
            {
                var iwalk = graphUI.nodes[nid].GetComponent<IWalkedEvent>();
                if (iwalk != null)
                {
                    iwalk.isAvailable = true;
                }
            }

            //初始化模拟器状态可用
            isCompleted = true;
            VisitedRoutes(visited_nids);
        }

        /// <summary>
        /// 开始行走
        /// </summary>
        public void StartWalk(NodeUI node)
        {

            if (isCompleted == false) return;

            if (node == null) return;

            if (available_list.Contains(node.NID) == false) return;

            //更新指示针位置
            pointer.transform.position = node.transform.position;
            current_node = node;
            isCompleted = false;
            //取消可选
            SetNodeAvailable(new List<int>());

            //进入节点
            var iWalk = node.GetComponent<IWalkedEvent>();
            if (iWalk != null)
            {
                //回调节点事件
                iWalk.OnStartWalk();

            }

            //更新行走过的路线flag (连接当前与上一个节点)
            if (visited_list.Count > 0)
            {
                var connection = new Connection(visited_list.Last(), current_node.NID);
                graphUI.lines[connection].flag.field = true;
            }
        }


        /// <summary>
        /// 结束行走
        /// </summary>
        [DrawButton(true)]
        public void EndWalk()
        {
            if (current_node == null) { DebugF.Log("当前为空节点"); return; }

            if (isCompleted == false)
            {
                isCompleted = true;
                SetNodeAvailable(Graph.Next(graphUI.temp_connections, current_node.NID));

                visited_list.Add(current_node.NID);
                current_node?.GetComponent<IWalkedEvent>()?.OnEndWalk();
            }
            else
            {
                DebugF.Log($"{current_node.NID}当前节点已完成");
            }
        }

        /// <summary>
        /// 选择节点(无按钮状态下父级测试使用)
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (usePointerClick)
            {
                var node = eventData.pointerCurrentRaycast.gameObject.GetComponent<NodeUI>();
                StartWalk(node);
            }
        }
    }

    public interface IWalkedEvent
    {
        NodeUI node { get; }
        WalkSimulator simulator { get; set; }
        bool isAvailable { get; set; }
        public void OnStartWalk();
        public void OnEndWalk();
    }

}