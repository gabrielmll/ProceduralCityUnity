using UnityEngine;
using System.Collections;

public class BuildingConstruct : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/* Exemplo de 1 unico predio com 2 andares para ilustrar o loop

		GameObject testBuilding = new GameObject ("Predio");
		GameObject floor1 = new GameObject ("Floor1");

		testBuilding.transform.Translate (7, 0, 0);
		testBuilding.AddComponent ("BoxMesh");
		floor1.transform.parent = testBuilding.transform;
		floor1.transform.Translate (7, 2, 0);
		floor1.AddComponent ("BoxMesh");
*/

		GameObject[] building = new GameObject[10];

		building [0] = new GameObject ();
		building[0].transform.Translate (0, 0, 7);
		building[0].AddComponent ("BoxMesh");
		for(int i = 1; i < 5; i++) {
			building [i] = new GameObject();
			building[i].transform.parent = building[0].transform;
			building[i].transform.Translate (0, 2*i, 7);
			building[i].AddComponent ("BoxMesh");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
