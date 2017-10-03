using UnityEditor;
using Kit.Physic;

[CustomEditor(typeof(RaycastHelper))]
public class RaycastHelperEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUI.BeginChangeCheck();
		SerializedProperty rayTypeProp = serializedObject.FindProperty("m_RayType");
		RaycastHelper.eRayType type = (RaycastHelper.eRayType) rayTypeProp.intValue;

		EditorGUILayout.PropertyField(rayTypeProp);
		
		if (type == RaycastHelper.eRayType.Raycast)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Distance"));

		}
		else if (type == RaycastHelper.eRayType.SphereCast)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Distance"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Radius"));
		}
		else if (type == RaycastHelper.eRayType.SphereOverlap)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Radius"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MemoryArraySize"));
		}
		else if (type == RaycastHelper.eRayType.BoxCast)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Distance"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_HalfExtends"));
			SerializedProperty syncProp = serializedObject.FindProperty("m_SyncRotation");
			EditorGUILayout.PropertyField(syncProp);
			if (!syncProp.boolValue)
				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LocalRotation"));
		}
		else if (type == RaycastHelper.eRayType.BoxOverlap)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_HalfExtends"));
			SerializedProperty syncProp = serializedObject.FindProperty("m_SyncRotation");
			EditorGUILayout.PropertyField(syncProp);
			if (!syncProp.boolValue)
				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LocalRotation"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MemoryArraySize"));
		}

		SerializedProperty fixedUpdateProp = serializedObject.FindProperty("m_FixedUpdate");
		EditorGUILayout.PropertyField(fixedUpdateProp);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LayerMask"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("m_QueryTriggerInteraction"));

		EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Color"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("m_HitColor"));

		if (fixedUpdateProp.boolValue) EditorGUILayout.PropertyField(serializedObject.FindProperty("OnHitEnter"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("OnHit"));
		if (fixedUpdateProp.boolValue) EditorGUILayout.PropertyField(serializedObject.FindProperty("OnHitLeave"));

		if (EditorGUI.EndChangeCheck())
			serializedObject.ApplyModifiedProperties();
	}
}
