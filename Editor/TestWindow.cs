using UnityEngine;
using UnityEditor;

public class TestWindow : EditorWindow
{
   [Header("Test")]
   Color color;
   
   [MenuItem("Window/Colorizer")]
   public static void ShowWindow()
   {
	   GetWindow<TestWindow>("Colorizer");
   }
   
   void OnGUI()
   {
	   //Window Code
	   GUILayout.Label("Color Selected Objects", EditorStyles.boldLabel);
	   
	   color = EditorGUILayout.ColorField("Color", color);
	   
	   if(GUILayout.Button("Colorize Me!"))
	   {
			Colorize();
	   }
   }
   
   void Colorize()
   {
	   foreach(GameObject obj in Selection.gameObjects)
	   {
		   Renderer ren = obj.GetComponent<Renderer>();
		   if(ren != null)
		   {
			   ren.sharedMaterial.color = color;
			}
		}
   }
}
