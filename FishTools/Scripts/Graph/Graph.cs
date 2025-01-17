using System;
using System.Collections.Generic;
using UnityEngine;

namespace FishTools.FGraph
{
    [Serializable]
    public class Graph<T> : IGraph<T>
    {
        //这两个数据可以包含所有内容
        [SerializeField] private List<Node<T>> nodes; //顶点数组
        private int[,] matrix; //邻接矩阵
        //TODO:思考matrix二维矩阵怎么去序列化,用不了字典貌似，字典是一对一，二维矩阵至少是三个元素

        //TODO:当我们序列化的时候需要去把matrix矩阵同步List<Node<T>>才行

        //顶点数目
        public int NodeCount => nodes.Count;

        //边的数目
        public int EdgeCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] > 0)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        // 更新图（顶点数组，邻接矩阵）
        public void UpdateGraph(Node<T>[] newNodes, int[,] newMatrix)
        {
            if (newNodes.Length != newMatrix.GetLength(0) || newNodes.Length != newMatrix.GetLength(1))
                throw new ArgumentException("节点数量和矩阵维度不匹配");

            nodes = new List<Node<T>>(newNodes);
            matrix = newMatrix;
        }

        // 添加顶点(同时更新邻接矩阵)
        public void AddNode(Node<T> newNode)
        {
            // 防止重复添加顶点
            if (nodes.Contains(newNode))
            {
                throw new InvalidOperationException("该顶点已存在，无法重复添加");
            }

            nodes.Add(newNode);

            int newSize = nodes.Count;
            var newMatrix = new int[newSize, newSize];

            // 确保保留旧矩阵内容
            if (matrix != null)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        newMatrix[i, j] = matrix[i, j];
                    }
                }
            }

            // 替换旧矩阵
            matrix = newMatrix;
        }

        //移除顶点
        public void DelNode(Node<T> node)
        {
            // 检查顶点是否存在
            if (!nodes.Contains(node))
            {
                throw new InvalidOperationException("要移除的顶点不存在！");
            }

            // 获取顶点索引
            int index = nodes.IndexOf(node);

            // 从节点列表中移除顶点
            nodes.Remove(node);

            // 更新邻接矩阵
            int newSize = nodes.Count;
            var newMatrix = new int[newSize, newSize];

            // 收紧矩阵
            for (int i = 0, newI = 0; i < matrix.GetLength(0); i++)
            {
                if (i == index) continue; // 跳过被移除的行

                for (int j = 0, newJ = 0; j < matrix.GetLength(1); j++)
                {
                    if (j == index) continue; // 跳过被移除的列

                    newMatrix[newI, newJ] = matrix[i, j];
                    newJ++;
                }
                newI++;
            }

            matrix = newMatrix;
        }

        // 添加边
        public void AddEdge(Node<T> start, Node<T> end, int weight = 1)
        {
            // 确保两个顶点都存在
            if (!nodes.Contains(start) || !nodes.Contains(end))
            {
                throw new InvalidOperationException("指定的顶点不存在！");
            }

            // 获取两个顶点的索引
            int startIndex = nodes.IndexOf(start);
            int endIndex = nodes.IndexOf(end);

            // 更新邻接矩阵
            matrix[startIndex, endIndex] = weight;
        }

        //查询边的权值
        public int GetEdgeWeight(Node<T> start, Node<T> end)
        {
            Debug.Log(nodes.Contains(start));
            Debug.Log(nodes.Contains(end));

            // 确保两个顶点都存在
            if (!nodes.Contains(start) || !nodes.Contains(end))
            {
                throw new InvalidOperationException("指定的顶点不存在！");
            }

            // 获取两个顶点的索引
            int startIndex = nodes.IndexOf(start);
            int endIndex = nodes.IndexOf(end);

            // 返回边的权值
            return matrix[startIndex, endIndex];
        }

        //移除边
        public void DelEdge(Node<T> start, Node<T> end)
        {
            // 确保两个顶点都存在
            if (!nodes.Contains(start) || !nodes.Contains(end))
            {
                throw new InvalidOperationException("指定的顶点不存在！");
            }

            // 获取两个顶点的索引
            int startIndex = nodes.IndexOf(start);
            int endIndex = nodes.IndexOf(end);

            // 清除邻接矩阵中的边信息
            matrix[startIndex, endIndex] = 0;
        }
    }
}