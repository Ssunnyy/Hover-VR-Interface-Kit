using System;
using System.Linq.Expressions;
using Hover.Common.Items;
using UnityEditor;
using UnityEngine;

namespace Hover.Common.Edit.Items {

	/*================================================================================================*/
	[CustomEditor(typeof(HoverBaseItem))]
	public abstract class HoverBaseItemEditor : Editor {

		protected string vIsHiddenOpenKey;
		protected GUIStyle vVertStyle;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void OnEnable() {
			vVertStyle = new GUIStyle();
			vVertStyle.padding = new RectOffset(16, 0, 0, 0);
			
			vIsHiddenOpenKey = "IsHiddenOpen"+target.GetInstanceID();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override void OnInspectorGUI() {
			Undo.RecordObject(target, target.GetType().Name);
			
			DrawItems();
			
			if ( GUI.changed ) {
				EditorUtility.SetDirty(target);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public override bool RequiresConstantRepaint() {
			return EditorPrefs.GetBool(vIsHiddenOpenKey);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawItems() {
			DrawMainItems();
			DrawEventItemGroup();
			DrawHiddenItemGroup();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawMainItems() {
			var t = (HoverBaseItem)target;
		
			t.Id = EditorGUILayout.TextField("ID", t.Id);
			t.Label = EditorGUILayout.TextField("Label", t.Label);
			t.Width = EditorGUILayout.FloatField("Width", t.Width);
			t.Height = EditorGUILayout.FloatField("Height", t.Height);
			t.IsEnabled = EditorGUILayout.Toggle("Is Enabled", t.IsEnabled);
			t.IsVisible = EditorGUILayout.Toggle("Is Visible", t.IsVisible);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawEventItemGroup() {
		}
			
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawHiddenItemGroup() {
			bool isHiddenOpen = EditorGUILayout.Foldout(EditorPrefs.GetBool(vIsHiddenOpenKey), "Info");
			EditorPrefs.SetBool(vIsHiddenOpenKey, isHiddenOpen);
			
			if ( isHiddenOpen ) {
				EditorGUILayout.BeginVertical(vVertStyle);
				GUI.enabled = false;
				DrawHiddenItems();
				GUI.enabled = true;
				EditorGUILayout.EndVertical();
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void DrawHiddenItems() {
			var t = (HoverBaseItem)target;
			
			EditorGUILayout.IntField("Auto ID", t.AutoId);
			EditorGUILayout.Toggle("Is Ancestry Enabled", t.IsAncestryEnabled);
			EditorGUILayout.Toggle("Is Ancestry Visible", t.IsAncestryVisible);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetPropertyName<T>(Expression<Func<T>> pPropExpr) {
			return (pPropExpr.Body as MemberExpression).Member.Name;
		}

	}

}