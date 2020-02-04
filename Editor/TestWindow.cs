using UnityEngine;
using UnityEditor;

public class TestWindow : EditorWindow
{
  //Name, speed, curr HP, max HP, curr MP, Max MP, 
  string name = "Joe";
  float speed = 2;
  float currHP = 100;
  float maxHP = 100;
  
   
   [MenuItem("Window/Player")]
   public static void ShowWindow()
   {
	   GetWindow<TestWindow>("Player");
   }
   
   void OnGUI()
   {	   
	   name = EditorGUILayout.TextField("Name", name);
	   speed = EditorGUILayout.Slider("Speed", speed, 1, 5);
	   currHP = EditorGUILayout.Slider("Current HP", currHP, 1, 100);
	   maxHP = EditorGUILayout.Slider("Max HP", maxHP, 1, 100);
	   
	   if(GUILayout.Button("Update Player!"))
	   {
			Debug.Log("Save Player");
	   }
   }
}
