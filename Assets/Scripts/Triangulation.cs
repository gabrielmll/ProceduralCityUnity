using UnityEngine;
using System.Collections;

public class Triangulation : MonoBehaviour {

  [System.Serializable]
  public class Edge {
    public Vector2 point1;
    public Vector2 point2;

    public Edge(Vector2 _point1, Vector2 _point2){
      point1 = _point1;
      point2 = _point2;
    }
  };

  public Vector2[] points;
  public Edge startingPoints;


  void DrawTriangle(Edge commonEdge, Vector2 point) {
    Debug.DrawLine(commonEdge.point1, commonEdge.point2, Color.white, -1.0f, false);
    Debug.DrawLine(commonEdge.point2, point, Color.white, -1.0f, false);
    Debug.DrawLine(point, commonEdge.point1, Color.white, -1.0f, false);
  }

  void GenerateNextPoint(Edge commonEdge) {
    if (commonEdge.point1 == commonEdge.point2) {
      Debug.LogError("Invalid common edge");
    }

    Vector2 nextPoint = new Vector2(commonEdge.point1.x+Random.Range(10f,20f), commonEdge.point2.y+Random.Range(10f,20f));

    DrawTriangle(commonEdge,nextPoint);

    Edge newEdge = new Edge(nextPoint, commonEdge.point1);
  }

	// Use this for initialization
	void Start () {
    GenerateNextPoint(startingPoints);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
