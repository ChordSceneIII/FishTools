using UnityEditor;
using UnityEngine;

/// <summary>
/// FollowPanelData 的自定义编辑器：自动同步followPanel预制体中的属性
/// </summary>

namespace EasyUI
{
    [CustomEditor(typeof(FollowPanelData))]
    public class FollowPanelDataEditor : BasePanelDataEditor
    {
        // 新增在Data资源的  Inspector 中显示的属性
        Transform newTarget;
        bool newAlwaysFollow;
        Vector3 newOffset;
        bool newcloseWhenClickOut;

        public override void OnInspectorGUI()
        {
            //绘制默认Inspector
            base.OnInspectorGUI();

            // 检查 panelPrefab 是否为 FollowPanel 的派生类
            if (data.panelPrefab is FollowPanel followPanel)
            {
                //获取panelPrefab中的序列化标记的属性(私有属性可以使用反射实现修改，不过更麻烦)
                newTarget = (Transform)EditorGUILayout.ObjectField("Follow Target", followPanel.target, typeof(Transform), true);
                newAlwaysFollow = EditorGUILayout.Toggle("Always Follow", followPanel.alwaysFollow);
                newOffset = EditorGUILayout.Vector3Field("Offset", followPanel.offsetVector);
                newcloseWhenClickOut = EditorGUILayout.Toggle("Close When Click Out", followPanel.closeWhenClickOut);


                // 如果值有变化，更新 FollowPanel 的属性
                if (newTarget != followPanel.target || newAlwaysFollow != followPanel.alwaysFollow ||
                newOffset != followPanel.offsetVector || newcloseWhenClickOut != followPanel.closeWhenClickOut)
                {
                    followPanel.target = newTarget;
                    followPanel.alwaysFollow = newAlwaysFollow;
                    followPanel.offsetVector = newOffset;
                    followPanel.closeWhenClickOut = newcloseWhenClickOut;

                    // 标记预制体为已修改
                    EditorUtility.SetDirty(followPanel);
                }
            }
            else
            {
                EditorGUILayout.HelpBox($"面板类型不对,需要的类型是{typeof(FollowPanel)}", MessageType.Error);
            }
        }
    }
}