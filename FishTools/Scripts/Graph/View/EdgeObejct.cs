// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace FishTools
// {
//     public sealed class EdgeObejct : MonoBehaviour
//     {
//         public Vector2 start;
//         public Vector2 end;
//         private RectTransform lineImage;
//         public RectTransform LineImage
//         {
//             get
//             {
//                 if (lineImage == null)
//                 {
//                     lineImage = GetComponent<RectTransform>();
//                 }
//                 return lineImage;
//             }
//         }
//         public float lineWidth = 10f;   // 线条宽度

//         [ContextMenu("更新")]
//         public void UpdateLine()
//         {
//             if (start == null || end == null || LineImage == null)
//                 return;

//             // 设置线条的起点位置
//             LineImage.position = start;

//             // 计算两点之间的方向和长度
//             Vector3 direction = end - start;
//             float length = direction.magnitude;

//             // 更新线条的宽度和长度
//             LineImage.sizeDelta = new Vector2(length, lineWidth);

//             // 计算旋转角度
//             float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

//             // 设置旋转角度
//             LineImage.rotation = Quaternion.Euler(0, 0, angle);
//         }

//     }
// }
