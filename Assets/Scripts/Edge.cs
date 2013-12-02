using UnityEngine;
using System.Collections;

[System.Serializable]
public class Edge {
  public Vector3 point1;
  public Vector3 point2;
  
  public Edge(Vector3 _point1, Vector3 _point2){
    point1 = _point1;
    point2 = _point2;
  }

  // Return the Vector between the points
  public Vector3 GetEdgeVector() {
    return point1-point2;
  }

  // Return the length of the edge
  public float GetLength() {
    return (GetEdgeVector()).magnitude;
  }

  // Verify if both edges share a common point and return it
  public Vector3 CommonPoint(Edge another){
    if (point1 == another.point1 || point1 == another.point2) {
      return point1;
    } else if (point2 == another.point1 || point2 == another.point2) {
      return point2;
    } else {
      // If they don't, set the X coordinate of the return vector to NaN
      return new Vector3(float.NaN, 0f, 0f);
    }
  }

  // Calculate the Angle between two edges
  public float AngleBetween(Edge another){
    Vector3 commonPoint = CommonPoint(another);
    if (float.IsNaN(commonPoint.x)) {
      // If there is no common point, return -1.0f as we assume the edges do not touch
      return -1.0f;
    } else {
      // The two edges form a V shape. The points that do not meet form another edge
      /*Edge opposingEdge = new Edge( (point1 == commonPoint) ? point2 : point1,
                                    (another.point1 == commonPoint) ? another.point2 : another.point2
                                  );*/

      float alphaAngle = Vector3.Angle(GetEdgeVector(), another.GetEdgeVector());
      Debug.Log("Found angle "+alphaAngle+" between vectors"+GetEdgeVector()+" and "+another.GetEdgeVector());


      return alphaAngle;
    }
  }
};
