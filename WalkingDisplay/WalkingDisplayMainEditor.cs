using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(WalkingDisplayMain))]
public class WalkingDisplayMainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WalkingDisplayMain walkingDisplayMain = target as WalkingDisplayMain;

        base.OnInspectorGUI();

        if (GUILayout.Button("Walk Straight"))
        {
            walkingDisplayMain.WalkStraight(0);
        }

        if (GUILayout.Button("Walk Stop"))
        {
            walkingDisplayMain.WalkStop();
        }
    }
}