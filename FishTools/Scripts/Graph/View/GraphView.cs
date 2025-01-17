// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;

// namespace FishTools.FGraph
// {
//     public sealed class GraphView : MonoBehaviour
//     {
//         public Graph<NodeObejct> graph;
//         [Label("节点")] public GameObject nodePrefab;
//         [Label("连线")] public GameObject line;

//         [SerializeField, ReadOnly] private NodeObejct start;
//         [SerializeField, ReadOnly] private NodeObejct end;
//         [SerializeField, ReadOnly] private EdgeObejct thisLine;

//         void Update()
//         {
//             DrawLine();
//         }

//         //实现连线功能 ,采用图片，并给图片添加材质效果

//         [ContextMenu("生成节点")]
//         public void GenNodeView()
//         {
//             GameObject obj = Instantiate(nodePrefab.gameObject, transform);
//             var nodeview = obj.GetComponent<NodeObejct>();

//             if (nodeview == null)
//             {
//                 DestroyImmediate(obj);
//                 DebugF.LogError("请检查节点预制体是否有BaseNode组件");
//                 return;
//             }

//             nodeview.graphView = this;
//             nodeview.node = new Node<Data>(nodeview);
//             graph.AddNode(nodeview.node);
//         }

//         public void GenLine(NodeObejct start, NodeObejct end)
//         {
//             graph.AddEdge(start.node, end.node);
//         }

//         public bool IsHasLine(NodeObejct start, NodeObejct end)
//         {
//             return graph.GetEdgeWeight(start.node, end.node) != 0;
//         }


//         public void DrawLine()
//         {
//             if (Input.GetMouseButtonDown(0))
//             {
//                 var node = GetNodeByMouse();

//                 if (node != null && start == null && end == null)
//                 {
//                     start = node;

//                     if (thisLine == null)
//                     {
//                         thisLine = Instantiate(line, transform).GetComponent<EdgeObejct>();
//                         if (thisLine == null)
//                         {
//                             DebugF.LogError("请检查连线预制体是否有Edge组件");
//                             return;
//                         }
//                         thisLine.start = start.gameObject.transform.position;
//                         thisLine.UpdateLine();
//                     }
//                 }
//                 else if (node != null && thisLine != null && start != null)
//                 {
//                     end = node; ;
//                     Debug.Log(IsHasLine(start, end));

//                     if (IsHasLine(start, end) == false)
//                     {
//                         GenLine(start, end);
//                         thisLine.end = end.gameObject.transform.position;
//                         thisLine.UpdateLine();
//                         thisLine = null;
//                         start = null;
//                         end = null;
//                     }
//                     else
//                     {
//                         DebugF.Log($"{start}和 {end} 这两个节点之间已经有连接了");
//                     }

//                 }


//             }

//             if (start != null && end == null)
//             {
//                 thisLine.end = Input.mousePosition;
//                 thisLine.UpdateLine();
//             }

//             if (Input.GetMouseButtonDown(1))
//             {
//                 start = null;
//                 end = null;
//                 if (thisLine != null)
//                     Destroy(thisLine.gameObject);
//             }
//         }

//         public NodeObejct GetNodeByMouse()
//         {
//             PointerEventData pointerData = new PointerEventData(EventSystem.current)
//             {
//                 position = Input.mousePosition
//             };

//             List<RaycastResult> results = new List<RaycastResult>();

//             EventSystem.current.RaycastAll(pointerData, results);

//             foreach (RaycastResult result in results)
//             {
//                 var node = result.gameObject.GetComponent<NodeObejct>();
//                 if (node != null)
//                 {
//                     return node;
//                 }
//             }
//             return null;
//         }
//     }
// }