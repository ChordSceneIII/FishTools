using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace FishTools.Graph
{
    /// <summary>
    /// 分层式节点图随机生成器
    /// </summary>
    [RequireComponent(typeof(GraphUI))]
    public class G_BoxLevel : MonoBehaviour
    {
        [Header("参数设置")]
        [Label("节点预制体")] public NodeUI nodeUI_prefab;
        [Label("层数")] public int levelCount = 7;
        [Label("每层节点范围")] public Vector2 nodeCount_Range = new Vector2(3, 5);
        [Label("终点数量")] public int endNodeCount = 1;
        [Label("初始点数量")] public int startNodeCount = 3;
        [Label("枝节概率"), Range(0, 1)] public float branchRate = 0.5f;
        [Label("层距")] public int level_distance;
        [Label("节点距离")] public int node_distance;
        [Label("点权距")] public int node_weight_distance;
        [Label("横置")] public bool setHorizontal = false;
        [Label("随机位移范围")] public Vector2 randomPosRange = new Vector2();
        [Label("限制在父布局中")] public bool limitInParent = false;
        [ConField("limitInParent", true)] public RectTransform parentRect;
        [ConField("limitInParent", true)] public float padding = 50;

        [Header("状态详情")]
        [ReadOnly, SerializeField] private GraphUI _graphUI;
        public GraphUI graphUI => FishUtility.LazyGet(this, ref _graphUI);
        public List<List<NodeUI>> allNodes = new List<List<NodeUI>>();

        //生成图
        [DrawButton(true)]
        private void Generate()
        {
            StartCoroutine(Generate_Coroutine());
        }

        public IEnumerator Generate_Coroutine()
        {
            yield return graphUI.Clear();

            allNodes.Clear();

            // ================= 生成节点 =================
            // 生成首层（起始点）
            List<NodeUI> firstLevel = CreateLevelNodes(0, startNodeCount);
            allNodes.Add(firstLevel);

            // 生成中间层
            for (int level = 1; level < levelCount - 1; level++)
            {
                int nodeCount = UnityEngine.Random.Range((int)nodeCount_Range.x, (int)nodeCount_Range.y + 1);
                allNodes.Add(CreateLevelNodes(level, nodeCount));
            }

            // 生成末层（终止点）
            List<NodeUI> lastLevel = CreateLevelNodes(levelCount - 1, endNodeCount);
            allNodes.Add(lastLevel);

            // ================= 初始化节点=================
            graphUI.InitNodes();

            // ================= 构建路径数据 =================
            Dictionary<Connection, int> connections = new Dictionary<Connection, int>();
            Dictionary<int, int> 标记数量 = new Dictionary<int, int>();

            //遍历层(从第0层到第n-1层)
            for (int i = 0; i < allNodes.Count - 1; i++)
            {
                List<NodeUI> curLevel = allNodes[i];
                List<NodeUI> nextLevel = allNodes[i + 1];
                标记数量[i + 1] = 0;

                if (curLevel.Count <= nextLevel.Count)
                {
                    //遍历当前层节点
                    for (int j = 0; j < curLevel.Count; j++)
                    {
                        //连接数等于下一层剩余未连接节点数/本层未连接节点数
                        var 连接数量 = (nextLevel.Count - 标记数量[i + 1]) / (curLevel.Count - j);
                        // var 余数 = (nextLevel.Count - 标记数量[i + 1]) % (curLevel.Count - j);
                        // if (余数 != 0)
                        // {
                        //     连接数量 += Random.Range(0, 2);
                        // }


                        for (int k = 0; k < 连接数量; k++)
                        {
                            标记数量[i + 1]++;
                            var connection = new Connection(curLevel[j].NID, nextLevel[标记数量[i + 1] - 1].NID);
                            connections.Add(connection, 1);
                        }

                        //随机左移一单位(附加连接枝干的关键)
                        if (UnityEngine.Random.Range(0f, 1f) > 1 - branchRate)
                        {
                            标记数量[i + 1] -= 1;
                        }
                    }
                }
                else
                {
                    //遍历当前层节点
                    for (int j = 0; j < nextLevel.Count; j++)
                    {
                        //连接数等于下一层剩余未连接节点数/本层未连接节点数
                        var 连接数量 = (curLevel.Count - 标记数量[i + 1]) / (nextLevel.Count - j);
                        // var 余数 = (nextLevel.Count - 标记数量[i + 1]) % (curLevel.Count - j);
                        // if (余数 != 0)
                        // {
                        //     连接数量 += Random.Range(0, 2);
                        // }

                        for (int k = 0; k < 连接数量; k++)
                        {
                            标记数量[i + 1]++;
                            var connection = new Connection(curLevel[标记数量[i + 1] - 1].NID, nextLevel[j].NID);
                            connections.Add(connection, 1);
                        }

                        //随机左移一单位(附加连接枝干的关键)
                        if (UnityEngine.Random.Range(0f, 1f) > 1 - branchRate)
                        {
                            标记数量[i + 1] -= 1;
                        }

                    }
                }
            }
            // ================= 地图路线美化 =================
            AdjustNodePos(connections);

            SetHorizontal();

            LimitNodes();
            // ================= 初始化连线=================
            yield return null;
            graphUI.InitLines(connections);
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        private List<NodeUI> CreateLevelNodes(int level, int nodeCount)
        {
            List<NodeUI> levelNodes = new List<NodeUI>();

            //计算初步位置
            var totalWidth = (nodeCount - 1) * node_distance;
            Vector2 levelPos = transform.position + new Vector3(-totalWidth / 2f, (1 - levelCount + level) * level_distance);

            //节点创建与位置调整
            for (int i = 0; i < nodeCount; i++)
            {
                NodeUI newNode = Instantiate(nodeUI_prefab, transform);
                newNode.transform.position = levelPos + new Vector2(i * node_distance, 0);
                levelNodes.Add(newNode);
            }

            return levelNodes;
        }

        /// <summary>
        /// 根据连线数量调整节点之间的距离
        /// </summary>
        private void AdjustNodePos(Dictionary<Connection, int> connections)
        {
            DictionarySer<Connection, int> _connections = new DictionarySer<Connection, int>(connections);

            if (allNodes.Count < 2) return;

            //初步优化节点位置
            for (int i = 0; i < allNodes.Count; i++)
            {
                //对最左侧节点进行随机偏移
                allNodes[i][0].transform.position +=
                        new Vector3(-node_weight_distance + UnityEngine.Random.Range(0, randomPosRange.x), UnityEngine.Random.Range(-randomPosRange.y, randomPosRange.y))
                         * allNodes[i].Count / nodeCount_Range.x;

                for (int j = 1; j < allNodes[i].Count; j++)
                {
                    var leftnode = allNodes[i][j - 1];
                    var thisnode = allNodes[i][j];

                    var 左节点线段数量 = Graph.Relative(_connections, leftnode.NID).Count;
                    var 当前节点线段数量 = Graph.Relative(_connections, thisnode.NID).Count;
                    var max = Mathf.Max(左节点线段数量, 当前节点线段数量);
                    var min = Mathf.Min(左节点线段数量, 当前节点线段数量);
                    float 权差 = (max * 1.5f - min) * 0.5f;

                    //位移差从中间对半分
                    float randomX = 0;
                    if (j < allNodes[i].Count / 2)
                        randomX = UnityEngine.Random.Range(-randomPosRange.x, 0);
                    else
                        randomX = UnityEngine.Random.Range(0, randomPosRange.x);

                    thisnode.transform.position = leftnode.transform.position
                    + new Vector3(node_distance + 权差 * node_weight_distance, 0)
                    + new Vector3(randomX, UnityEngine.Random.Range(-randomPosRange.y, randomPosRange.y));
                }
            }
        }

        /// <summary>
        /// 把节点限制在父级Rect范围之内
        /// </summary>
        public void LimitNodes()
        {
            //限制超出范围的节点层
            if (limitInParent && parentRect != null)
            {
                float parent_min = 0;
                float parent_max = 0;
                float parentWidth = 0;

                if (setHorizontal)
                {
                    parent_min = padding + parentRect.transform.position.y - parentRect.rect.height / 2;
                    parent_max = -padding + parentRect.transform.position.y + parentRect.rect.height / 2;
                    parentWidth = parentRect.rect.height - padding * 2;
                }
                else
                {
                    parent_min = padding + parentRect.transform.position.x - parentRect.rect.width / 2;
                    parent_max = -padding + parentRect.transform.position.x + parentRect.rect.width / 2;
                    parentWidth = parentRect.rect.width - padding * 2;
                }


                for (int i = 0; i < allNodes.Count - 1; i++)
                {
                    var first = allNodes[i].First();
                    var last = allNodes[i].Last();

                    if (setHorizontal)
                    {
                        float diffY = last.transform.position.y - first.transform.position.y + first.rectTransform.rect.height;
                        Debug.Log("deltaY:" + diffY);

                        //如果整体宽度度大于限制宽度，则进行收缩（以下一为基准）
                        if (diffY > parentWidth)
                        {
                            float deltaY = (diffY - parentWidth) / (allNodes[i].Count - 1);
                            Debug.Log("deltaY:" + deltaY);
                            for (int j = 1; j < allNodes[i].Count; j++)
                            {
                                allNodes[i][j].transform.position += new Vector3(0, -deltaY) * j;
                            }
                        }

                        var _minYDelta = parent_min - (first.transform.position.y - first.rectTransform.rect.height / 2);

                        var _maxYDelta = parent_max - (last.transform.position.y + last.rectTransform.rect.height / 2);


                        if (_minYDelta > 0)
                        {
                            for (int j = 0; j < allNodes[i].Count; j++)
                            {
                                allNodes[i][j].transform.position += new Vector3(0, _minYDelta);
                            }
                        }
                        //超出范围的整体移动限制到范围内
                        if (_maxYDelta < 0)
                        {
                            for (int j = 0; j < allNodes[i].Count; j++)
                            {
                                allNodes[i][j].transform.position += new Vector3(0, _maxYDelta);
                            }
                        }

                    }
                    else
                    {
                        float diffX = last.transform.position.x - first.transform.position.x + first.rectTransform.rect.width;
                        //如果整体长度大于限制宽度，则进行收缩（以左一为基准）
                        if (diffX > parentWidth)
                        {
                            float deltaX = (diffX - parentWidth) / (allNodes[i].Count - 1);
                            for (int j = 1; j < allNodes[i].Count; j++)
                            {
                                allNodes[i][j].transform.position += new Vector3(-deltaX, 0) * j;
                            }
                        }

                        var _minXDelta = parent_min - (first.transform.position.x - first.rectTransform.rect.width / 2);

                        var _maxXDelta = parent_max - (last.transform.position.x + last.rectTransform.rect.width / 2);

                        //超出范围的整体移动限制到范围内
                        if (_minXDelta > 0)
                        {
                            for (int j = 0; j < allNodes[i].Count; j++)
                            {
                                allNodes[i][j].transform.position += new Vector3(_minXDelta, 0);
                            }
                        }

                        if (_maxXDelta < 0)
                        {
                            for (int j = 0; j < allNodes[i].Count; j++)
                            {
                                allNodes[i][j].transform.position += new Vector3(_maxXDelta, 0);
                            }
                        }
                    }

                }
            }
        }

        private void SetHorizontal()
        {
            if (setHorizontal)
            {
                foreach (var node in graphUI.nodes.Values)
                {
                    node.rectTransform.anchoredPosition = new Vector2(node.rectTransform.anchoredPosition.y, node.rectTransform.anchoredPosition.x);
                }
            }
        }

    }
}
