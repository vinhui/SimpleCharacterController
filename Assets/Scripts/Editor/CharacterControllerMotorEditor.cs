using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterControllerMotor))]
public class CharacterControllerMotorEditor : Editor 
{
    public override bool RequiresConstantRepaint() => true;

    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        
        if(!Application.isPlaying) return;
        
        var t = (CharacterControllerMotor)target;
        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        GUI.enabled = false;
        EditorGUILayout.Vector3Field("Move Dir", t.MoveDir);
        EditorGUILayout.Toggle("Is Sprinting", t.IsSprinting);
        EditorGUILayout.Toggle("Can Sprint", t.CanSprint);
        EditorGUILayout.FloatField("Stamina", t.Stamina);
        EditorGUILayout.Toggle("Is Grounded", t.IsGrounded);
    }
}