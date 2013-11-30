using UnityEngine;
using System.Collections;

public class BuildingConstruct : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject building = new GameObject ("Predio");
		GameObject floor1 = new GameObject ("Floor1");

		building.transform.Translate (7, 0, 0);
		building.AddComponent ("BoxMesh");
		floor1.transform.parent = building.transform;
		floor1.transform.Translate (7, 2, 0);
		floor1.AddComponent ("BoxMesh");

		GameObject[] testBuilding = new GameObject[10];

		testBuilding [0] = new GameObject ();
		testBuilding[0].transform.Translate (0, 0, 7);
		testBuilding[0].AddComponent ("BoxMesh");
		for(int i = 1; i < 10; i++) {
			testBuilding [i] = new GameObject();
			testBuilding[i].transform.parent = testBuilding[0].transform;
			testBuilding[i].transform.Translate (0, 2*i, 7);
			testBuilding[i].AddComponent ("BoxMesh");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
