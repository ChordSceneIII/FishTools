using System;
using System.Collections.Generic;
using UnityEngine;

namespace FishTools.Graph
{
    /// <summary>
    /// 图 （纯 值字段关系）
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

        public List<int> Next(int nid)
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

        public List<int> Last(int nid)
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

        public List<Connection> Relative(int nid)
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

        //TODO:完成最短路径算法求路径问题

        /// <summary>
        /// 最短路径（带权）
        /// 使用 Dijkstra 算法（假定所有边权非负）
        /// 返回一系列连接（Connection），从起点到终点的顺序排列
        /// </summary>
        // public List<Connection> ShortestPath(int start, int target)
        // {
        //     // 存储从起点到各节点的最短距离
        //     Dictionary<int, int> distances = new Dictionary<int, int>();
        //     // 记录到达每个节点时所使用的连接（用于回溯路径）
        //     Dictionary<int, Connection> previous = new Dictionary<int, Connection>();

        //     // 注意：这里使用 .NET 内置的 PriorityQueue，如果 Unity 版本不支持，可以自行实现优先队列
        //     PriorityQueue<int, int> queue = new PriorityQueue<int, int>();

        //     // 初始化：起点距离为 0
        //     distances[start] = 0;
        //     queue.Enqueue(start, 0);

        //     while (queue.Count > 0)
        //     {
        //         int current = queue.Dequeue();

        //         // 如果到达目标节点，则可以提前退出
        //         if (current == target)
        //         {
        //             break;
        //         }

        //         // 遍历所有从当前节点出发的边
        //         foreach (KeyValuePair<Connection, int> kvp in connections)
        //         {
        //             Connection conn = kvp.Key;
        //             int weight = kvp.Value;
        //             if (conn.from != current)
        //                 continue;

        //             int neighbor = conn.to;
        //             int newDist = distances[current] + weight;

        //             // 如果没有记录过该邻居，或者找到更短的路径，则更新距离和前驱
        //             if (!distances.ContainsKey(neighbor) || newDist < distances[neighbor])
        //             {
        //                 distances[neighbor] = newDist;
        //                 previous[neighbor] = conn;
        //                 queue.Enqueue(neighbor, newDist);
        //             }
        //         }
        //     }

        //     // 如果目标节点不可达，则返回 null（也可以返回空列表，根据需求修改）
        //     if (!distances.ContainsKey(target))
        //         return null;

        //     // 回溯构造路径，从 target 反向回溯到 start
        //     List<Connection> path = new List<Connection>();
        //     int node = target;
        //     while (node != start)
        //     {
        //         Connection conn = previous[node];
        //         path.Add(conn);
        //         node = conn.from;
        //     }
        //     // 翻转顺序，得到从 start 到 target 的路径
        //     path.Reverse();
        //     return path;
        // }
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