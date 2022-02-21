using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IKInstaller))]
public class IKInstallerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        IKInstaller ikInstaller = (IKInstaller)target;

        if (GUILayout.Button("Install IK"))
        {
            ikInstaller.InstallIK();
        }
    }
}
