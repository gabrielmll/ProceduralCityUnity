using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquareGen : MonoBehaviour {

  const int MIN_SIDE = 10;
  const int MAX_SIDE = 20;

  float squareDimension = 70.0f;

  public Transform floor_prefab;

  public Transform player;

  struct squareInfo{
    public Vector2 origin;
    public Vector2 center;
    public GameObject fullSquare;
  }

  List<squareInfo> createdSquares;

	// Use this for initialization
	void Start () {
    Vector2 playerPos = new Vector2(player.position.x,player.position.z);
    createdSquares = new List<squareInfo>();
    CreateSquare(playerPos);
    CreateSquare(playerPos + new Vector2(-squareDimension, 0));
    CreateSquare(playerPos + new Vector2(-squareDimension, -squareDimension));
    CreateSquare(playerPos + new Vector2(0, -squareDimension));
	}
	
	// Update is called once per frame
	void Update () {
    Vector2 playerPos = new Vector2(player.position.x,player.position.z);
	  if (createdSquares.Count > 4) 
    {
      // Remove most distant square
      //float biggestDist = 0;
      //int chosenID = -1;
      for (int i = 0; i<createdSquares.Count; ++i) {
        float dist = (playerPos-createdSquares[i].center).magnitude;
        if (dist > squareDimension*2) {

          Debug.Log("Would kill on: "+createdSquares[i].center);
          Destroy(createdSquares[i].fullSquare);
          createdSquares.RemoveAt(i);
          break;
        }
      }
    } //else 
    {
      // Create missing closest squares

      // Round vector values to the closest to squareDimension
      Vector2 approxPlayerPos = new Vector2( (Mathf.Round(playerPos.x/squareDimension))*squareDimension,
                                             (Mathf.Round(playerPos.y/squareDimension))*squareDimension );
      Debug.Log("Approx "+approxPlayerPos);
      // For a verification of which quadrants have been checked (relative to the approximated player position)
      bool first = false, second = false, third = false, fourth = false; 
      for (int i = 0; i<createdSquares.Count; ++i) {
        Vector2 tempApproxPlayerPos = approxPlayerPos;
        if (!first && tempApproxPlayerPos == createdSquares[i].origin) {
          first = true;
          Debug.Log("1st "+i);
          continue;
        }
        tempApproxPlayerPos.y -= squareDimension;
        if (!second && tempApproxPlayerPos == createdSquares[i].origin) {
          second = true;
          Debug.Log("2nd "+i);
          continue;
        }
        tempApproxPlayerPos.x -= squareDimension;
        if (!third && tempApproxPlayerPos == createdSquares[i].origin) {
          third = true;
          Debug.Log("3rd "+i);
          continue;
        }
        tempApproxPlayerPos.y += squareDimension;
        if (!fourth && tempApproxPlayerPos == createdSquares[i].origin) {
          fourth = true;
          Debug.Log("4th "+i);
          continue;
        }
      }

      if (!first) {
        CreateSquare(approxPlayerPos);
      }
      approxPlayerPos.y -= squareDimension;
      if (!second) {
        CreateSquare(approxPlayerPos);
      }
      approxPlayerPos.x -= squareDimension;
      if (!third) {
        CreateSquare(approxPlayerPos);
      }
      approxPlayerPos.y += squareDimension;
      if (!fourth) {
        CreateSquare(approxPlayerPos);
      }
    }
	}

  // Create the square by calling the creation of blocks
  void CreateSquare (Vector2 origin) {

    Transform floor = Instantiate(floor_prefab, new Vector3(origin.x+squareDimension*0.5f,0,origin.y+squareDimension*0.5f), Quaternion.identity) as Transform;
    floor.transform.localScale = new Vector3(squareDimension*0.1f, 1.0f, squareDimension*0.1f);

    List<Vector2> diagonalPointsOnSquare = new List<Vector2>();
    int count = 0;
    float i;
    for(i = 0; i<squareDimension-MIN_SIDE; ) {
      Vector2 new_d = new Vector2(origin.x+i,origin.y+i);
      diagonalPointsOnSquare.Add(new_d);
      ++count;
      i+=Random.Range(MIN_SIDE,MAX_SIDE);
    }
    {
      i = squareDimension;
      Vector2 new_d = new Vector2(origin.x+i,origin.y+i);
      diagonalPointsOnSquare.Add(new_d);
      ++count;
    }
    for(int j = 0; j<count-1; ++j) {
      {
        Vector2 position = diagonalPointsOnSquare[j];
        Vector2 position2 = diagonalPointsOnSquare[j+1];
        Vector2 position1 = new Vector2(position.x, position2.y);
        Vector2 position3 = new Vector2(position2.x, position.y);
        CreateBlocks(floor, position, position, position1, position2, position3);
      }
      for(int k = j+1; k<count-1; ++k) {
        {
          Vector2 position = new Vector2(diagonalPointsOnSquare[j].x, diagonalPointsOnSquare[k].y);
          Vector2 position2 = new Vector2(diagonalPointsOnSquare[j+1].x, diagonalPointsOnSquare[k+1].y);
          Vector2 position1 = new Vector2(position.x, position2.y);
          Vector2 position3 = new Vector2(position2.x, position.y);
          CreateBlocks(floor, position, position, position1, position2, position3);
        }
        {
          Vector2 position = new Vector2(diagonalPointsOnSquare[k].x, diagonalPointsOnSquare[j].y);
          Vector2 position2 = new Vector2(diagonalPointsOnSquare[k+1].x, diagonalPointsOnSquare[j+1].y);
          Vector2 position1 = new Vector2(position.x, position2.y);
          Vector2 position3 = new Vector2(position2.x, position.y);
          CreateBlocks(floor, position, position, position1, position2, position3);
        }
      }
    }
    squareInfo sqInfo;
    sqInfo.origin = origin;
    sqInfo.center = new Vector2(origin.x+0.5f*squareDimension, origin.y+0.5f*squareDimension);
    sqInfo.fullSquare = floor.gameObject;
    createdSquares.Add(sqInfo);
  }

  void CreateBlocks (Transform fullSquare, Vector2 buildingPosition, Vector2 b0, Vector2 b1, Vector2 b2, Vector2 b3) {
    // TODO: Make it create blocks instead of buildings
    GameObject[] building = new GameObject[15];
    
    building [0] = new GameObject ();
    building[0].transform.Translate (buildingPosition.x, 0, buildingPosition.y);
    
    BoxMesh boxMesh = building[0].AddComponent("BoxMesh") as BoxMesh;
    
    boxMesh.p0 = b0;
    boxMesh.p1 = b1;
    boxMesh.p2 = b2;
    boxMesh.p3 = b3;
    
    // pick a texture randomly
    boxMesh.buildingTex = "building" + Random.Range (1, 3);
    
    //building[0].AddComponent ("BoxMesh");
    
    // Pile boxes in a random high
    int numberOfFloors = Random.Range(5, 15);
    for(int i = 1; i < numberOfFloors; i++) {
      building[i] = Object.Instantiate(building[0]) as GameObject;
      building[i].transform.Translate (0, 4*i, 0);
      //building[i].AddComponent ("BoxMesh");
    }
    
    for(int i = 1; i < numberOfFloors; i++) {
      building[i].transform.parent = building[0].transform;
    }

    building[0].transform.parent = fullSquare.transform;
  }
}
