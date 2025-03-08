using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FishTools.Graph
{
    /// <summary>
    /// 图 （图数据，算法)
    /// </summary>
    [CreateAssetMenu(fileName = "Graph", menuName = "FishTools/Graph", order = 0)]
    public class Graph : ScriptableObject
    {
        //连接关系（from,to），权重(weight)
        public DictionarySer<Connection, int> connections = new DictionarySer<Connection, int>();

        public void Clear()
        {
            connections.Clear();
        }

        public bool Contains(int from, int to)
        {
            return connections.ContainsKey(new Connection(from, to));
        }

        public void ConnectNode(Connection connection, int weight = 1)
        {
            connections[connection] = weight;
        }

        public void DisconnectNode(Connection connection)
        {
            connections.Remove(connection);
        }

        public static List<int> Next(DictionarySer<Connection, int> connections, int nid)
        {
            var next = new List<int>();
            foreach (var connection in connections)
            {
                if (connection.Key.from == nid)
                {
                    next.Add(connection.Key.to);
                }
            }
            return next;
        }

        public static List<int> Last(DictionarySer<Connection, int> connections, int nid)
        {
            var last = new List<int>();
            foreach (var connection in connections)
            {
                if (connection.Key.to == nid)
                {
                    last.Add(connection.Key.from);
                }
            }
            return last;
        }

        public static List<Connection> Relative(DictionarySer<Connection, int> connections, int nid)
        {
            var relative = new List<Connection>();
            foreach (var connection in connections)
            {
                if (connection.Key.from == nid || connection.Key.to == nid)
                {
                    relative.Add(connection.Key);
                }
            }
            return relative;
        }

        /// <summary>
        /// 最短路径（带权）使用 Dijkstra 算法，手动实现优先队列逻辑
        /// </summary>
        public static List<Connection> DijkstraPath(DictionarySer<Connection, int> connections, int start, int target)
        {
            // 收集图中所有节点（from 和 to）
            HashSet<int> nodes = new HashSet<int>();
            foreach (var conn in connections.Keys)
            {
                nodes.Add(conn.from);
                nodes.Add(conn.to);
            }

            // 如果起点或终点不存在，直接返回空
            if (!nodes.Contains(start) || !nodes.Contains(target))
            {
                DebugF.LogWarning("起点或终点不存在");
                return null;
            }

            // 初始化距离和前驱
            Dictionary<int, int> distances = new Dictionary<int, int>();
            Dictionary<int, Connection> previous = new Dictionary<int, Connection>();
            List<int> unprocessed = new List<int>();

            foreach (int node in nodes)
            {
                distances[node] = int.MaxValue; // 初始距离设为无穷大
                unprocessed.Add(node);
            }
            distances[start] = 0; // 起点距离为0

            while (unprocessed.Count > 0)
            {
                // 手动查找当前未处理的最小距离节点
                int current = -1;
                int minDist = int.MaxValue;
                foreach (int node in unprocessed)
                {
                    if (distances[node] < minDist)
                    {
                        minDist = distances[node];
                        current = node;
                    }
                }

                // 所有剩余节点不可达或已找到目标
                if (current == -1 || current == target)
                    break;

                unprocessed.Remove(current);

                // 遍历所有从当前节点出发的边
                foreach (var kvp in connections)
                {
                    Connection conn = kvp.Key;
                    if (conn.from != current)
                        continue;

                    int neighbor = conn.to;
                    int weight = kvp.Value;

                    // 跳过未更新的初始节点（防止溢出）
                    if (distances[current] == int.MaxValue)
                        continue;

                    // 计算新距离
                    int alt = distances[current] + weight;
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = conn;
                    }
                }
            }

            // 目标不可达
            if (distances[target] == int.MaxValue)
                return null;

            // 回溯路径
            List<Connection> path = new List<Connection>();
            int currentNode = target;
            while (previous.ContainsKey(currentNode))
            {
                Connection conn = previous[currentNode];
                path.Add(conn);
                currentNode = conn.from;

                // 回溯到起点时终止
                if (currentNode == start)
                    break;
            }

            // 路径顺序反转并校验
            path.Reverse();
            if (path.Count == 0 || (path[0].from != start && path[0].to != target))
                return null;

            return path;
        }

#if UNITY_EDITOR
        public new void SetDirty()
        {
            EditorUtility.SetDirty(this);
        }
#endif
    }


    [Serializable]
    public struct Connection : IEquatable<Connection>
    {
        public int from;
        public int to;

        public Connection(int from, int to)
        {
            this.from = from;
            this.to = to;
        }

        public bool Equals(Connection other) => from == other.from && to == other.to;
        public override bool Equals(object obj) => obj is Connection other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(from, to);
    }
}