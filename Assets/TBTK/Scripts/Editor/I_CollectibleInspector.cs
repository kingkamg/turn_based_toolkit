using UnityEngine;
using UnityEditor;

using System;
using System.Collections;

using TBTK;

namespace TBTK{
	
	[CustomEditor(typeof(Collectible))]
	[CanEditMultipleObjects]
	public class I_CollectibleInspector : TBEditorInspector {
		
		private static Collectible instance;
		void Awake(){
			instance = (Collectible)target;
			LoadDB();
		}
		
		public override void OnInspectorGUI(){
			base.OnInspectorGUI();
			
			if(instance==null) Awake();
			
			GUI.changed = false;
			
			EditorGUILayout.Space();
			
			
			PrefabType type=PrefabUtility.GetPrefabType(instance);
			
			if(type==PrefabType.Prefab || type==PrefabType.PrefabInstance){
				
				bool existInDB=false;
				if(type==PrefabType.PrefabInstance) existInDB=TBEditor.ExistInDB((Collectible)PrefabUtility.GetPrefabParent(instance));
				else if(type==PrefabType.Prefab) existInDB=TBEditor.ExistInDB(instance);
				
				if(!existInDB){
					EditorGUILayout.Space();
					
					EditorGUILayout.HelpBox("This prefab hasn't been added to database hence it won't be accessible to the game.", MessageType.Warning);
					GUI.color=new Color(1f, 0.7f, .2f, 1f);
					if(GUILayout.Button("Add Prefab to Database")){
						NewCollectibleEditorWindow.Init();
						NewCollectibleEditorWindow.NewItem(instance);
						NewCollectibleEditorWindow.Init();		//call again to select the instance in editor window
					}
					GUI.color=Color.white;
				}
				else{
					EditorGUILayout.HelpBox("Editing collectible using Inspector is not recommended.\nPlease use the editor window instead.", MessageType.Info);
					if(GUILayout.Button("Collectible Editor Window")) NewCollectibleEditorWindow.Init(instance.prefabID);
				}
				
				EditorGUILayout.Space();
			}
			else{
				string text="Collectible object won't be available for game deployment, or accessible in TBTK editor until it's made a prefab and added to TBTK database.";
				text+="\n\nYou can still edit the collectible using default inspector. However it's not recommended";
				EditorGUILayout.HelpBox(text, MessageType.Warning);
				
				EditorGUILayout.Space();
				if(GUILayout.Button("Collectible Editor Window")) NewCollectibleEditorWindow.Init(instance.prefabID);
			}
			
			
			DefaultInspector();
			
			if(GUI.changed) EditorUtility.SetDirty(instance);
		}
		
	}

}