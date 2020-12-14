using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

/// <summary>
/// 参照の検索結果を表示するEditorWindow
/// this editor window shows result of finding references
/// </summary>
namespace StaTech{
	
	public class ReferenceFindResultWindow : EditorWindow {

		static ReferenceFindResultWindow window;

		private static List<ReferencedComponent> referencedComponents = new List<ReferencedComponent>();

		private static MarkedComponent findSourceComponent;
		public static MarkedComponent FindSourceComponent{
			set{findSourceComponent = value;}
		}

		private static List<MarkedComponent> findSourceComponents = new List<MarkedComponent>();
		public static List<MarkedComponent> FindSourceComponents{
			set{findSourceComponents = value;}
		}

		private static MarkedComponent selectingComponent;

		private string gameObjectLabel = "GameObject : ";
		private string componentLabel  = " |__Component : "; 
		private string propertyLabel   = "       |__Property : ";

		private string componentPadding = "    ";

		private static Vector2 minWindowSize = new Vector2(800,300);

		void OnGUI(){
			ShowReferenceInfoFromList(referencedComponents);
		}

		public static void ShowProgress(string progressDescription,float rate){
			EditorUtility.DisplayProgressBar(Define.GetText(TextKey.Progress), string.Format(progressDescription), rate);  
		}

		public static void ClearProgress(){
			EditorUtility.ClearProgressBar();
		}

		public static void ShowReferenceInfo(List<ReferencedComponent> list){
			referencedComponents = list;
			Open();
		}

		public static void Open(){
			if(window == null){
				window = CreateInstance<ReferenceFindResultWindow>();
				window.minSize = minWindowSize;
			}
			window.ShowUtility();
			selectingComponent = findSourceComponent;
		}

		private void ShowReferenceInfoFromList(List<ReferencedComponent> referencedComponets_){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical();
			ShowSelfPingButton();
			ShowFindSourceInfo();
			foreach(var component in findSourceComponents){
				ShowFindSourceComponentInfo(component);
			}
			EditorGUILayout.EndVertical();
			GUILayout.Box("", GUILayout.Height(position.height), GUILayout.Width(1));
			EditorGUILayout.BeginVertical();
			ShowReferenceInfo();
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		private void ShowSelfPingButton(){
			Color backColor = GUI.backgroundColor;
			GUI.backgroundColor = Color.magenta;
			if(GUILayout.Button(Define.GetText(TextKey.PingSourceObject),GUILayout.Width(200))){
				if(ReferenceByComponentFinder.FindSourceGameObject != null){
					EditorGUIUtility.PingObject(ReferenceByComponentFinder.FindSourceGameObject);
				}
			}
			GUI.backgroundColor = backColor;
		}

		private void ShowFindSourceInfo(){
			EditorGUILayout.BeginHorizontal();
			FitLabelField(findSourceComponent.componentName);
			if(GUILayout.Button(Define.GetText(TextKey.ShowReference),GUILayout.Width(100))){
				selectingComponent = findSourceComponent;
			};
			EditorGUILayout.EndHorizontal();
		}

		private void ShowFindSourceComponentInfo(MarkedComponent mc){
			if(mc.instanceID == findSourceComponent.instanceID) return; 
			EditorGUILayout.BeginHorizontal();
			FitLabelField(componentPadding +"|___"+ mc.componentName);
			if(GUILayout.Button(Define.GetText(TextKey.ShowReference),GUILayout.Width(100))){
				selectingComponent = mc;
			}
			EditorGUILayout.EndHorizontal();
		}

		private void ShowReferencesLabel(List<ReferencedComponent> referencedComponents_){

			foreach(var rc in referencedComponents_){
				Color backColor = GUI.backgroundColor;
				GUI.backgroundColor = Color.cyan;
				EditorGUILayout.BeginHorizontal("Button");
				string infoLabel = GenerateReferenceInfoText(rc);
				GUILayout.Label(infoLabel);
				GUI.backgroundColor = Color.magenta;
				if(GUILayout.Button("Select")){
					EditorGUIUtility.PingObject(rc.attachingGO);
					Selection.activeGameObject = rc.attachingGO;
				}
				EditorGUILayout.EndHorizontal();
				GUI.backgroundColor = backColor;
			}	
		}

		private void ShowReferenceInfo(){
			var selectedComponents = referencedComponents
				.Where(_ => _.referenceSourceComponent == selectingComponent)
				.ToList();
			if(selectedComponents.Count > 0){
				ShowReferencesLabel(selectedComponents);
			}else{
				ShowReferenceEmptyLabel();
			}
		}

		private string GenerateReferenceInfoText(ReferencedComponent rc){
			string infoText =  gameObjectLabel + rc.attachingGO.name 					   + System.Environment.NewLine + 
				               componentLabel  + rc.referentComponent.componentName        + System.Environment.NewLine + 
				               propertyLabel   + rc.propertyName;
			return infoText;
		}

		private void FitLabelField(string text){
			EditorGUILayout.LabelField(text,GUILayout.Width(220));
		}

		private void ShowReferenceEmptyLabel(){
			GUILayout.Label(Define.GetText(TextKey.ReferenceEmpty));
		}
	}
}