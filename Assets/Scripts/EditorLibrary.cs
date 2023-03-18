using System;
using UnityEditor;
using UnityEngine;

namespace ICKT.Editor
{
	public class EditorLibrary : MonoBehaviour
	{
		// Unity Editor/Inspector related

		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
		public class ReadOnlyFieldAttribute : PropertyAttribute { }


#if UNITY_EDITOR
		[CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
		public class ReadOnlyDrawer : PropertyDrawer
		{
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
				GUI.enabled = false;
				EditorGUI.PropertyField(position, property, label, true);
				GUI.enabled = true;
			}
		}
	}
#endif
}
