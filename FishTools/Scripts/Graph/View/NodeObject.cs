// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// namespace FishTools.FGraph
// {
//     public sealed class NodeObejct : MonoBehaviour
//     {
//         [ReadOnly] public GraphView graphView;
//         public Node<Vector2> node;

//         //我们需要把Node的数据保存，那么我们就不能声明Node<NodeObject> 而是开辟一个struct 保存位置

//         //TODO: 不采用对NodeObject的存储，而是强制使用int 作为索引保存
//         public NodeObejct end;
//         void OnDestroy()
//         {
//             try
//             {
//                 graphView.graph.DelNode(node);
//             }
//             catch { }
//         }
//         [ContextMenu("S")]
//         public void HasNode()
//         {
//             Debug.Log(graphView.graph.GetEdgeWeight(this.node, end.node));
//         }

//     }

//     //TODO:重写构建GraphView 类
//     //TODO: NodeObject的存储的信息包含的应该是只有位置信息，而GraphView存储NodeObject列表

// }