using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace FishTools
{
    /// <summary>
    /// <para> 监听字段(当字段改变时触发事件）</para>
    /// <para> 通过调用field访问器来触发 </para>
    /// </summary>
    [Serializable]
    public class ObserveField<T> : IObserveEvent
    {
        public ObserveField() { }
        public ObserveField(T value) => _field = value;
        public ObserveField(UnityAction action) => @event.AddListener(action);
        public ObserveField(UnityAction<T> action) => @eventT.AddListener(action);
        [SerializeField] private T _field;
        private UnityEvent @event = new UnityEvent();
        private UnityEvent<T> @eventT = new UnityEvent<T>();

        // 暴露事件接口
        public UnityEvent Event => @event;
        public UnityEvent<T> EventT => @eventT;

        public T field
        {
            get => _field;
            set
            {
                _field = value;
                Invoke();
            }
        }

        public ObserveField<T> AddListener(UnityAction action)
        {
            @event.AddListener(action);
            return this;
        }

        public ObserveField<T> AddListener(UnityAction<T> action)
        {
            @eventT.AddListener(action);
            return this;

        }

        public void RemoveListener(UnityAction action) => @event.RemoveListener(action);
        public void RemoveListener(UnityAction<T> action) => @eventT.RemoveListener(action);
        public void RemoveAllListeners()
        {
            @event.RemoveAllListeners();
            @eventT.RemoveAllListeners();
        }

        public void Invoke()
        {
            @event?.Invoke();
            @eventT?.Invoke(_field);
        }

        public void Refresh()
        {
            Invoke();
        }
    }

    public interface IObserveEvent
    {
        void Invoke();
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ObserveField<>))]
    public class ObserveFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 获取 _value 字段
            var valueProp = property.FindPropertyRelative("_field");

            if (valueProp == null)
            {
                EditorGUI.LabelField(position, label.text, "Unsupported type");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            // 显示字段值并允许编辑
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.PropertyField(position, valueProp, label);
            if (EditorGUI.EndChangeCheck())
            {
                // 应用修改后的值
                property.serializedObject.ApplyModifiedProperties();

                // 通过反射获取 ObField 对象
                var targetObject = fieldInfo.GetValue(property.serializedObject.targetObject);
                if (targetObject is IObserveEvent invokeable)
                {
                    // 调用事件
                    invokeable.Invoke();
                }
            }

            EditorGUI.EndProperty();
        }
    }
#endif

}