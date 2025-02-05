using System.Collections.Generic;
using System.Linq;
using FishTools.EasyUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FishTools.Graph
{
    public class GraphUI : MonoBehaviour
    {
        [Header("GraphManager设置")]
        [Label("graph数据")] public Graph graph;
        public ConnectionUI linePrefab;

        [Header("数据和引用")]
        [ReadOnly, Label("自增器")] public int count_recorder = 0;
        [Label("连接引用")] public DictionarySer<Connection, ConnectionUI> lines = new DictionarySer<Connection, ConnectionUI>();
        [Label("节点引用")] public DictionarySer<int, NodeUI> nodes = new DictionarySer<int, NodeUI>();
        [ReadOnly, SerializeField] private GameObject _ConnectionObjects;
        public GameObject ConnectionObjects
        {
            get
            {
                if (_ConnectionObjects == null)
                {
                    _ConnectionObjects = new GameObject("连接线");
                    _ConnectionObjects.transform.SetParent(transform);
                }
                return _ConnectionObjects;
            }
            set { _ConnectionObjects = value; }
        }

        private void Start()
        {
            if (linePrefab == null) { DebugF.LogError("linePrefab预制体未设置"); return; }

            LoadGraph();
        }

        /// <summary>
        /// 初始化GraphManager
        /// </summary>
        [DrawButton("载入数据")]
        public void LoadGraph()
        {
            InitNodes();
            InitLines();
        }

        [DrawButton("保存数据")]
        public void SaveGraph()
        {
            graph.Clear();
            foreach (var line in lines)
            {
                graph.ConnectNode(line.Key, line.Value.weight.field);
            }
            DebugF.LogColor(ColorCode.Green, "保存成功");
        }


        /// <summary>
        /// 连接两个节点
        /// </summary>
        public void Connect(Connection connection, int weight)
        {

            if (lines.ContainsKey(connection)) { DebugF.LogWarning("已存在连接"); return; }

            var new_line = Instantiate(linePrefab, ConnectionObjects.transform);
            var start = nodes[connection.from];
            var end = nodes[connection.to];

            if (start == null || end == null) { DebugF.LogError("连接失败，节点不存在"); return; }

            new_line.SetPosition(start.outport, end.inport);
            new_line.weight.field = weight;

            lines.Add(connection, new_line);
            new_line.connection = connection;
        }

        /// <summary>
        /// 断开两个节点
        /// </summary>
        public void DisConnect(Connection connection)
        {

            if (!lines.ContainsKey(connection)) { DebugF.LogWarning("不存在连接"); return; }

            var line = lines[connection];
            line.connection = default;
            Destroy(line.gameObject);
            lines.Remove(connection);
        }

        /// <summary>
        /// 记录节点并编号
        /// </summary>
        public void RecordNode(NodeUI node)
        {
            if (node == null || node.gameObject == null) { DebugF.LogWarning("节点为空"); return; }

            count_recorder++;
            node.NID = count_recorder;
            this.nodes[node.NID] = node;
        }

        /// <summary>
        /// 更新所有连接
        /// </summary>
        public void InitLines()
        {
            FishUtility.DestroyAllChilds(ConnectionObjects.transform);
            lines.Clear();

            foreach (var connection in graph.connections)
            {
                Connect(connection.Key, connection.Value);
            }

            DebugF.Log("加载连接完成");
        }

        /// <summary>
        /// 记录目录下所有节点并编号
        /// </summary>
        public void InitNodes()
        {
            count_recorder = 0;
            nodes.Clear();
            foreach (Transform child in transform)
            {
                var node = child.GetComponent<NodeUI>();
                if (node != null)
                {
                    RecordNode(node);
                }
            }
            DebugF.Log("获取节点完成");
        }

    }

    //TODO：先完成基本的Graph，然后在游戏中对其拓展层级观念，房间逻辑按住每一层级设置，并且制作依赖种子的房间系统
    //TOODO: 利用种子生成房间，并为不同的房间添加价值也就是权重，利用最短路径求解最有价值的路径
}