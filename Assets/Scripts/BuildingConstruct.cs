using UnityEngine;
using System.Collections;

public class BuildingConstruct : MonoBehaviour {
	// Sets the building position in the land
	// Default values must be changed in another script
//	public static Vector2 buildingPosition = new Vector2(0, 7);

	void Start () {
		for (float i = -5; i < 5; i++) {
			CreateBuilding (new Vector2(0, 20*i));
		}

	}
	// Use this for initialization
	void CreateBuilding (Vector2 buildingPosition) {
		// These variables determine the shape of the building
		// They're random for now
		Vector2 b0 = new Vector2 (Random.Range(-10, 0), Random.Range(10, 0));
		Vector2 b1 = new Vector2 (Random.Range(10, 0), Random.Range(10, 0));
		Vector2 b2 = new Vector2 (Random.Range(10, 0), Random.Range(-10, 0));
		Vector2 b3 = new Vector2 (Random.Range(-10, 0), Random.Range(-10, 0));

		GameObject[] building = new GameObject[15];

		building [0] = new GameObject ();
		building[0].transform.Translate (buildingPosition.x, 0, buildingPosition.y);
		BoxMesh.p0 = b0;
		BoxMesh.p1 = b1;
		BoxMesh.p2 = b2;
		BoxMesh.p3 = b3;

		// pick a texture randomly
		BoxMesh.buildingTex = "building" + Random.Range (1, 3);

		building[0].AddComponent ("BoxMesh");

		// Pile boxes in a random high
		for(int i = 1; i < Random.Range(5, 15); i++) {
			building [i] = new GameObject();
			building[i].transform.parent = building[0].transform;
			building[i].transform.Translate (buildingPosition.x, 4*i, buildingPosition.y);
			building[i].AddComponent ("BoxMesh");

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
