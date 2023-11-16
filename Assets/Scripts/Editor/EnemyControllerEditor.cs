using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : Editor
{
  void OnSceneGUI()
  {
    EnemyController ec = (EnemyController)target;
    Handles.color = Color.red;
    Handles.DrawWireArc(ec.transform.position, Vector3.up, ec.transform.forward, ec.fovHalfAngle + ec.fovHalfAngle, 10.0f);
  }
}
