using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeController))]
public class DyanmicWallShape : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public float wallHeight = 2;
    private SpriteShapeController controller;

    void Start()
    {
        controller = GetComponent<SpriteShapeController>();
        UpdateSpriteShape();
    }

    /*
    void Update()
    {
        UpdateSpriteShape();
    }
    */

    private void UpdateSpriteShape()
    {
        Spline shape = controller.spline;
        shape.Clear();
        shape.InsertPointAt(0, (Vector2)transform.InverseTransformPoint(leftPoint.position));
        shape.InsertPointAt(1, (Vector2)transform.InverseTransformPoint(rightPoint.position));
        shape.InsertPointAt(2, (Vector2)transform.InverseTransformPoint(rightPoint.position + Vector3.down * wallHeight));
        shape.InsertPointAt(3, (Vector2)transform.InverseTransformPoint(leftPoint.position + Vector3.down * wallHeight));
    }
}
