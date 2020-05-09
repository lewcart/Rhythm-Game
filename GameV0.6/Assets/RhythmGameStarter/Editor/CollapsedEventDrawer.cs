using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace RhythmGameStarter
{
    [CustomPropertyDrawer(typeof(CollapsedEventAttribute))]
    public class CollapsedEventDrawer : UnityEventDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // EditorGUI.BeginProperty(position, label, property);

            EditorGUI.indentLevel++;

            var attr = this.attribute as CollapsedEventAttribute;

            position.height = EditorGUIUtility.singleLineHeight;
            var temp = new GUIContent(label);

            SerializedProperty persistentCalls = property.FindPropertyRelative("m_PersistentCalls.m_Calls");
            if (persistentCalls != null)
                temp.text += " (" + persistentCalls.arraySize + ")";

            attr.visible = EditorGUI.Foldout(position, attr.visible, temp, true);

#if UNITY_2019_1_OR_NEWER
            attr.visible = EditorGUI.BeginFoldoutHeaderGroup(position, attr.visible, temp);
#else
            attr.visible = EditorGUI.Foldout(position, attr.visible, temp, true);
#endif
            if (attr.visible)
            {
                label.text = null;
                position.height = base.GetPropertyHeight(property, label);
                position.y += EditorGUIUtility.singleLineHeight;
                base.OnGUI(position, property, label);
            }
#if UNITY_2019_1_OR_NEWER
            EditorGUI.EndFoldoutHeaderGroup();
#endif

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var attr = this.attribute as CollapsedEventAttribute;

            return attr.visible ? base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight;
        }

    }
}
