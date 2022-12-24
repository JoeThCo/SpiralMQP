using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Check out Unity Script Reference if you are confused about any of the functions or classes
/// </summary>
public class RoomNodeGraphEditor : EditorWindow
{
    // Define a customized GUI style
    private GUIStyle roomNodeStyle;

    // Node layout values
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    // Add a menu item named "Room Node Graph Editor" 
    // And define a PATH that we can call the menuitem 
   [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]

   //  Only static functions can use the MenuItem attribute.
   private static void OpenWindow()
   {
        // A built in function in "EditorWindow" class
        // Returns the first EditorWindow of type t which is currently on the screen.
        // "Room Node Graph Editor" is the title of the window
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
   }

   private void OnEnable() 
   {
        // Define node layout style
        roomNodeStyle = new GUIStyle();
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);
   }

    /// <summary>
    /// Draw Editor GUI
    /// </summary>
   private void OnGUI() 
   {
    
   }
}
