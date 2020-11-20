using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(SpriteShapeController))]
[RequireComponent(typeof(PolygonCollider2D))]
public class SmartWallShape : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public float wallHeight = 1;

    public void UpdateSpriteShape()
    {
        Spline shape = GetComponent<SpriteShapeController>().spline;
        PolygonCollider2D coll = GetComponent<PolygonCollider2D>();
        
        Vector2[] path = new Vector2[4];
        path[0] = (Vector2)transform.InverseTransformPoint(leftPoint.position);
        path[1] = (Vector2)transform.InverseTransformPoint(rightPoint.position);
        path[2] = (Vector2)transform.InverseTransformPoint(rightPoint.position + Vector3.down * wallHeight);
        path[3] = (Vector2)transform.InverseTransformPoint(leftPoint.position + Vector3.down * wallHeight);

        shape.Clear();
        for (int i = 0; i < 4; i++)
        {
            shape.InsertPointAt(i, path[i]);
            shape.SetCorner(i, true);
        }
        coll.SetPath(0, path);

        float angle = 0.5f + (Mathf.Atan2(leftPoint.position.x - rightPoint.position.x, leftPoint.position.y - rightPoint.position.y) / (2*Mathf.PI));
        float lightAngle = 0.2f;
        float brightness = 1 - Mathf.Abs(lightAngle - angle);
        GetComponent<SpriteShapeRenderer>().color = new Color(brightness, brightness, brightness);
    }
}

/*
for(int i = 0; i < pointCount; i++)
{
    shape.InsertPointAt(i, (Vector2)transform.InverseTransformPoint(points[i].position));
}
for (int i = 0; i < pointCount; i++)
{
    shape.InsertPointAt(pointCount + i, (Vector2)transform.InverseTransformPoint(points[pointCount - 1 - i].position + Vector3.down * wallHeight));
}
*/
