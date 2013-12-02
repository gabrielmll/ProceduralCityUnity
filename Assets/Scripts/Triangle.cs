using UnityEngine;
using System.Collections;

public class Triangle : MonoBehaviour {
  public Vector3 point1;
  public Vector3 point2;
  public Vector3 point3;
  
  public void setTriangle(Vector3 _point1, Vector3 _point2, Vector3 _point3){
    point1 = _point1;
    point2 = _point2;
    point3 = _point3;
  }

  /**
   *            ^ 3 (new point)
   *          /  \ 
   *         /    \ Potentialy new spaces
   *       1 ----- 2
   *      Common Edge
   *      Drawn space
   */
  public void setTriangle(Edge commonEdge, Vector3 point){
    point1 = commonEdge.point1;
    point2 = commonEdge.point2;
    point3 = point;

    Debug.Log("New triangle. Angles: ");
    Edge edge1 = new Edge(point, commonEdge.point1);
    Edge edge2 = new Edge(point, commonEdge.point2);
    commonEdge.AngleBetween(edge1);
    commonEdge.AngleBetween(edge2);
    edge1.AngleBetween(edge2);
  }
  
  public void Update() {
    Debug.DrawLine(point1, point2, Color.white, 0f, false);
    Debug.DrawLine(point2, point3, Color.white, 0f, false);
    Debug.DrawLine(point3, point1, Color.white, 0f, false);
  }

  //public Vector3 getNextPoint() {

  //}
}
