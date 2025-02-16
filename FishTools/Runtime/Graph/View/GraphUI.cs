using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace FishTools.Graph
{
    /// <summary>
    /// 图管理器（核心结构）
    /// </summary>
    public class GraphUI : MonoBehaviour
    {
        [Header("GraphManager设置")]
        [Label("graph数据")] public Graph graph;
        public ConnectionUI linePrefab;

        [Header("数据和引用")]
        [ReadOnly, Label("自增器")] public int count_recorder = 0;
        [Label("连接引用")] public DictionarySer<Connection, ConnectionUI> lines = new DictionarySer<Connection, ConnectionUI>();
        public DictionarySer<Connection, int> temp_connections
        {
            get
            {
                DictionarySer<Connection, int> temp = new DictionarySer<Connection, int>();
                foreach (var line in lines)
                {
                    temp.Add(line.Key, line.Value.weight.field);
                }
                return temp;
            }

        }
        [Label("节点引用")] public DictionarySer<int, NodeUI> nodes = new DictionarySer<int, NodeUI>();
        [ReadOnly, SerializeField] private GameObject _lineObjs;
        public GameObject lineObjs
        {
            get
            {
                if (_lineObjs == null)
                {
                    _lineObjs = new GameObject("lineObjs");
                    lineObjs.transform.SetParent(transform);
                }
                return _lineObjs;
            }
            set { _lineObjs = value; }
        }

        private void Start()
        {
            if (linePrefab == null) { DebugF.LogError("linePrefab预制体未设置"); return; }

            // LoadGraph();
        }

        /// <summary>
        /// 初始化GraphUI
        /// </summary>
        public void AfterClear(Action action)
        {
            count_recorder = 0;
            nodes.Clear();
            lines.Clear();

            List<Transform> childs = new List<Transform>();
            foreach (Transform child in transform)
            {
                childs.Add(child);
            }

            FMonitor.AfterDestory(childs).OnComplete(action);
        }


        /// <summary>
        /// 初始化GraphManager
        /// </summary>
        [DrawButton("载入数据")]
        public void LoadGraph()
        {
            InitNodes();
            InitLines(graph.connections.ToDic());
        }

        [DrawButton("保存数据")]
        public void SaveGraph()
        {
            graph.Clear();
            foreach (var line in lines)
            {
                graph.ConnectNode(line.Key, line.Value.weight.field);
            }

#if UNITY_EDITOR
            graph.SetDirty();
#endif
            DebugF.LogColor(ColorCode.Green, "保存成功");
        }


        /// <summary>
        /// 连接两个节点
        /// </summary>
        public void Connect(Connection connection, int weight)
        {

            if (lines.ContainsKey(connection)) { DebugF.LogWarning("已存在连接"); return; }

            var new_line = Instantiate(linePrefab, lineObjs.transform);
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
        public void InitLines(Dictionary<Connection, int> connections)
        {
            FishUtility.DestroyAllChilds(lineObjs.transform);
            lines.Clear();

            foreach (var connection in connections)
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
            var new_nodes = GetComponentsInChildren<NodeUI>();
            foreach (var node in new_nodes)
            {
                if (node != null)
                {
                    RecordNode(node);
                }
            }
            DebugF.Log("获取节点完成");
        }
    }
}