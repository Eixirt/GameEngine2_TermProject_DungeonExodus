using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace StaTech{
		
	/// <summary>
	/// コンポーネントからの参照を検索するクラス
	/// Searchselected object's references by other components
	/// </summary>
	public class ReferenceByComponentFinder{

		/// <summary>
		/// 参照されたコンポーネントのリスト
		/// </summary>
		private static List<ReferencedComponent> refs;

		private static List<MarkedComponent> findSourceComponents;
		public static GameObject FindSourceGameObject;

		/// <summary>
		/// シーン内のすべてのオブジェクト
		/// </summary>
		/// <value>All objects in scene.</value>
		private static GameObject[] allObjectsInScene{
			get{
				return (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
			}
		}

		/// <summary>
		/// 他のコンポーネントからの参照を検索
		/// </summary>
		[MenuItem("GameObject/Find References by Other Component",false,-1)]
		public static void FindReferenceFromOtherComponent(){
			var selectedGameObject = Selection.activeGameObject;
			FindSourceGameObject = selectedGameObject;
			if(!selectedGameObject){
				Debug.LogWarning("please select gameobject on hierarchy");
				return;
			}
			if(Application.isPlaying) {
				Debug.Break();
			}

			findSourceComponents = new List<MarkedComponent>();
			var targetGO = new MarkedComponent(selectedGameObject);
			ReferenceFindResultWindow.FindSourceComponent = targetGO;
			findSourceComponents.Add(targetGO);

			foreach(var component in selectedGameObject.GetComponents<Component>()){
				findSourceComponents.Add(new MarkedComponent(component));
			}
			ReferenceFindResultWindow.FindSourceComponents = findSourceComponents;

			StartAnalyzingGameObjectsInScene();
		}

		/// <summary>
		/// シーン内のゲームオブジェクトの解析開始
		/// </summary>
		private static void StartAnalyzingGameObjectsInScene(){
			refs = new List<ReferencedComponent>();
			int size = findSourceComponents.Count;
			for(var i = 0; i < size;i++){

				//検索の進捗を表示
				float rate = (float)i/(float)size;
				ReferenceFindResultWindow.ShowProgress(string.Format("{0}/{1}",i,size),rate);

				foreach (GameObject obj in allObjectsInScene)
				{
					AnalyzeComponents(obj,findSourceComponents[i]);
				}
			}
			ReferenceFindResultWindow.ClearProgress();
			ReferenceFindResultWindow.ShowReferenceInfo(refs);
		}

		/// <summary>
		/// 対象のゲームオブジェクトが持つComponentを解析
		/// </summary>
		/// <param name="go">Go.</param>
		private static void AnalyzeComponents(GameObject go,MarkedComponent sourceComponent){
			// SerializedObjectを通してアセットのプロパティを取得する
			List<MarkedComponent> findTargetComponents = new List<MarkedComponent>();
			findTargetComponents.AddRange(go.GetComponents<Component>()
				.Where(component => component != null)
				.Select(component => new MarkedComponent(component))
			);

			foreach(var markedComponent in findTargetComponents){
				//Transformはコンポーネントのアタッチ操作に関わらず子階層のinstanceIDを持っているのでスキップする
				if(markedComponent.componentName == "Transform") continue;

				var serializedObj = markedComponent.serializedObject;
				SerializedProperty property = serializedObj.GetIterator();
				while (property.Next(true)) {
					if (property.propertyType == SerializedPropertyType.ObjectReference &&
						property.objectReferenceInstanceIDValue == sourceComponent.instanceID) {
						if(property.name == "m_GameObject") continue;
						ReferencedComponent goRef = new ReferencedComponent(go,property,markedComponent,sourceComponent);
						goRef.attachingGO = go;
						goRef.propertyName = property.name;
						goRef.referentComponent = markedComponent;
						refs.Add(goRef);
					}
				}
			}
		}
	}

	/// <summary>
	/// 名前とIDがマークされたSerializedObject
	/// </summary>
	[System.Serializable]
	public class MarkedComponent{
		public SerializedObject serializedObject;
		public int instanceID;
		public string componentName;

		public MarkedComponent(Component component){
			instanceID = component.GetInstanceID();
			componentName = component.GetType().Name;
			serializedObject = new SerializedObject(component);
		}

		public MarkedComponent(GameObject gameObject){
			instanceID = gameObject.GetInstanceID();
			componentName = "GameObject(" + gameObject.name + ")";
			serializedObject = new SerializedObject(gameObject);
		}
	}

	/// <summary>
	/// 参照情報を含んだComponent
	/// </summary>
	[System.Serializable]
	public class ReferencedComponent{
		/// <summary>
		/// 参照元のコンポーネント
		/// </summary>
		public MarkedComponent referenceSourceComponent;
		/// <summary>
		/// 参照先のコンポーネント
		/// </summary>
		public MarkedComponent referentComponent;
		/// <summary>
		/// アタッチしているGameObject
		/// </summary>
		public GameObject attachingGO;
		public string propertyName;

		public ReferencedComponent(GameObject attachingGO_,SerializedProperty property_,MarkedComponent referentComponent_,MarkedComponent referenceSouceCompoennt_){
			attachingGO = attachingGO_;
			propertyName = property_.name;
			referentComponent = referentComponent_;
			referenceSourceComponent = referenceSouceCompoennt_;
		}
	}
}