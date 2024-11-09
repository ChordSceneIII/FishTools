using UnityEditor;
using UnityEngine;
using FishTools;

/// <summary>
///  BaseButtonHandler的编辑器自定义
/// </summary>

namespace FishTools.SceneUI
{
    [CustomEditor(typeof(ButtonHandler), true)]
    public class ButtonHandlerEditor : BaseUIHandlerEditor
    {
        private ButtonHandler mobj;
        private SerializedProperty orignalSpriteProperty;
        private SerializedProperty h_tSpriteProperty;
        private SerializedProperty h_MaterialProperty;
        private SerializedProperty p_SpriteProperty;
        private SerializedProperty p_MaterialProperty;
        private SerializedProperty animator;
        private SerializedProperty h_AnimationClip;
        private SerializedProperty p_AnimationClip;
        private SerializedProperty i_AnimationClip;
        private SerializedProperty clickDownEvent;
        private SerializedProperty clicEvent;
        private SerializedProperty clicUpEvent;
        private SerializedProperty clickOutsideEvent;
        private SerializedProperty hoverInsideEvent;
        private SerializedProperty hoverOutsideEvent;

        //之所以用序列化属性，是因为序列化属性可以自定义编辑器字段，可以拖拽引用
        //如果用EditorGUILayout没有对应的UnityEvent栏重写
        //使用EditorGUILayout.PropertyField就必须用SerializedProperty

        private void OnEnable()
        {
            if (target == null)
            {
                return;
            }
            mobj = (ButtonHandler)target;

            orignalSpriteProperty = serializedObject.FindProperty("orignalSpriteRender");
            h_tSpriteProperty = serializedObject.FindProperty("h_tSprite");
            h_MaterialProperty = serializedObject.FindProperty("h_Material");
            p_SpriteProperty = serializedObject.FindProperty("p_Sprite");
            p_MaterialProperty = serializedObject.FindProperty("p_Material");
            animator = serializedObject.FindProperty("animator");
            h_AnimationClip = serializedObject.FindProperty("h_AnimClip");
            p_AnimationClip = serializedObject.FindProperty("p_AnimClip");
            i_AnimationClip = serializedObject.FindProperty("i_AnimCLip");
            clickDownEvent = serializedObject.FindProperty("onClickDown");
            clicEvent = serializedObject.FindProperty("onClick");
            clicUpEvent = serializedObject.FindProperty("onClickUp");
            clickOutsideEvent = serializedObject.FindProperty("onClickOutside");
            hoverInsideEvent = serializedObject.FindProperty("onHoverInside");
            hoverOutsideEvent = serializedObject.FindProperty("onHoverOutside");

        }
        private bool showEventSettings = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(orignalSpriteProperty, new GUIContent("目标渲染器"));
            mobj.transitionType = (ButtonHandler.TransitionType)EditorGUILayout.EnumPopup("过渡", mobj.transitionType);

            switch (mobj.transitionType)
            {
                case ButtonHandler.TransitionType.Sprite:
                    EditorGUILayout.PropertyField(h_tSpriteProperty, new GUIContent("高亮精灵"));
                    EditorGUILayout.PropertyField(p_SpriteProperty, new GUIContent("按下精灵"));
                    break;
                case ButtonHandler.TransitionType.Color:
                    mobj.h_Color = EditorGUILayout.ColorField("高亮颜色", mobj.h_Color);
                    mobj.p_Color = EditorGUILayout.ColorField("按下颜色", mobj.p_Color);
                    break;
                case ButtonHandler.TransitionType.Material:
                    EditorGUILayout.PropertyField(h_MaterialProperty, new GUIContent("高亮材质"));
                    EditorGUILayout.PropertyField(p_MaterialProperty, new GUIContent("按下材质"));
                    break;
                case ButtonHandler.TransitionType.Animation:
                    EditorGUILayout.PropertyField(animator, new GUIContent("动画器"));
                    EditorGUILayout.PropertyField(i_AnimationClip, new GUIContent("待机动画"));
                    EditorGUILayout.PropertyField(h_AnimationClip, new GUIContent("高亮动画"));
                    EditorGUILayout.PropertyField(p_AnimationClip, new GUIContent("按下动画"));
                    break;
            }
            //折叠框
            showEventSettings = EditorGUILayout.Foldout(showEventSettings, "事件设置");
            if (showEventSettings)
            {
                EditorGUILayout.PropertyField(clickDownEvent, new GUIContent("单击事件"));
                EditorGUILayout.PropertyField(clicEvent, new GUIContent("按住事件"));
                EditorGUILayout.PropertyField(clicUpEvent, new GUIContent("松开事件"));
                EditorGUILayout.PropertyField(clickOutsideEvent, new GUIContent("点击空白处事件"));
                EditorGUILayout.PropertyField(hoverInsideEvent, new GUIContent("内部悬停事件"));
                EditorGUILayout.PropertyField(hoverOutsideEvent, new GUIContent("外部悬停事件"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
