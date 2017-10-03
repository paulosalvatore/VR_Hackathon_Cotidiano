﻿#define FREE_VERSION
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Kit.Extend;

namespace CF.CameraBot
{
    [CustomPropertyDrawer(typeof(VirtualPosition))]
    public class VirtualPositionDrawer : PropertyDrawer
    {
        private GUIContent CameraLabel = new GUIContent("Camera", "Camera relative position of chase target.");
        private GUIContent VirtualFaceLabel = new GUIContent("Face", "Virtual facing relative position of chase target.");
        private GUIContent EnableVirtualFaceTarget = new GUIContent("Enable virtual face target", "Calculate the relative position to facing, relative to chase target.");
        private GUIContent TargetOffsetLabel = new GUIContent("Target Offset", "Adjust Chase target pivot point offset.");
        private GUIContent CameraOffsetLabel = new GUIContent("Camera Offset", "Adjust Camera pivot point offset.");

		readonly float lineH = EditorGUIUtility.singleLineHeight, lineS = EditorGUIUtility.standardVerticalSpacing;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			Rect line = position.Clone(height: lineH).Adjust(y: lineS); 
			EditorGUI.PropertyField(line, property.FindPropertyRelative("m_TargetOffset"), TargetOffsetLabel, false);

			line = line.GetRectBottom(height: lineH).Adjust(y: lineS);
			EditorGUI.PropertyField(line, property.FindPropertyRelative("m_CameraOffset"), CameraOffsetLabel, false);

			line = line.GetRectBottom(height: VirtualPointDrawer.GetStaticHeight(property.FindPropertyRelative("Camera")));

			EditorGUI.PropertyField(line, property.FindPropertyRelative("m_Camera"), CameraLabel, true);
			
			EditorGUI.EndProperty();
        }

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return GetStaticHeight(property);
		}

		public static float GetStaticHeight(SerializedProperty property)
		{
			float rst = 129f;
			return rst;
		}
	}
}
#endif