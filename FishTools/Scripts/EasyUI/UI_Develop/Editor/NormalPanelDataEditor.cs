using UnityEditor;

/// <summary>
/// UI数据编辑器: 自动获取预制体路径
/// </summary>

namespace EasyUI
{
    [CustomEditor(typeof(NormalPanelData))]
    public class NormalPanelDataEditor : BasePanelDataEditor
    {
        public override void OnInspectorGUI()
        {
            //绘制默认Inspector
            base.OnInspectorGUI();

            if (data.panelPrefab is NormalPanel normalpanel)
            {

            }
            else
            {
                EditorGUILayout.HelpBox($"面板类型不对,需要的类型是{typeof(NormalPanel)}", MessageType.Error);
            }

        }
    }
}