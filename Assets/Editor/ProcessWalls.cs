using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.U2D;

public class ProcessWalls
{
    [MenuItem("Teledrama/Aligner les murs")]
    static void UpdateWalls()
    {
        SmartWallShape[] diagonalWalls = GameObject.FindObjectsOfType<SmartWallShape>();
        foreach(SmartWallShape wall in diagonalWalls)
        {
            Undo.RecordObject(wall.gameObject.GetComponent<SpriteShapeController>(), wall.name);
            Undo.RecordObject(wall.gameObject.GetComponent<PolygonCollider2D>(), wall.name);
            Undo.RecordObject(wall.gameObject.GetComponent<SpriteShapeRenderer>(), wall.name);
            wall.UpdateSpriteShape();
            EditorUtility.SetDirty(wall);
        }

        VerticalWallScale[] verticalWalls = GameObject.FindObjectsOfType<VerticalWallScale>();
        foreach (VerticalWallScale wall in verticalWalls)
        {
            Undo.RecordObject(wall.transform, wall.name);
            wall.UpdateWallScale();
            EditorUtility.SetDirty(wall);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
