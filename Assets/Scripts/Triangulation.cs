using UnityEngine;
using System.Collections;

public class Triangulation : MonoBehaviour {

  public Vector2[] points;
  public Edge startingPoints;
  public int num_triangles;

  IEnumerator DrawTriangle(Edge commonEdge, Vector3 point) {
    /*Debug.DrawLine(commonEdge.point1, commonEdge.point2, Color.white, 500f, false);
    Debug.DrawLine(commonEdge.point2, point, Color.white, 500f, false);
    Debug.DrawLine(point, commonEdge.point1, Color.white, 500f, false);*/
    Triangle tri = gameObject.AddComponent("Triangle") as Triangle;
    tri.setTriangle(commonEdge, point);

    yield return new WaitForSeconds(2);

    if (num_triangles > 0) {
      -- num_triangles;
      GenerateNextPoint(new Edge(point, commonEdge.point1));
    }
  }

  void GenerateNextPoint(Edge commonEdge) {
    if (commonEdge.point1 == commonEdge.point2) {
      Debug.LogError("Invalid common edge");
    }

    Vector3 nextPoint = new Vector3(Random.Range(5f,20f), 0, Random.Range(-10f,10f));
    nextPoint += (commonEdge.point1+commonEdge.point2)/2;
    Debug.Log(nextPoint);
    StartCoroutine(DrawTriangle(commonEdge,nextPoint));

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

