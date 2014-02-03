using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquareGen : MonoBehaviour {

  const int N_TEXTURES = 3;//56;

  const int MIN_BLOCK_SIDE = 100;
  const int MAX_BLOCK_SIDE = 200;
  const int MIN_FLOORS = 5;
  const int MAX_FLOORS = 15;
  const int MAX_BUILDINGS_PER_BLOCK_SIDE = 3;

  const float STREET_WIDTH = 13.5f; // 27m total
  const float PAVIMENT_WIDTH = 3f; // 3m each side

  float squareDimension = 700.0f;

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

          //Debug.Log("Kill on: "+createdSquares[i].center);
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
      //Debug.Log("Approx "+approxPlayerPos);
      // For a verification of which quadrants have been checked (relative to the approximated player position)
      bool first = false, second = false, third = false, fourth = false; 
      for (int i = 0; i<createdSquares.Count; ++i) {
        Vector2 tempApproxPlayerPos = approxPlayerPos;
        if (!first && tempApproxPlayerPos == createdSquares[i].origin) {
          first = true;
          //Debug.Log("1st "+i);
          continue;
        }
        tempApproxPlayerPos.y -= squareDimension;
        if (!second && tempApproxPlayerPos == createdSquares[i].origin) {
          second = true;
          //Debug.Log("2nd "+i);
          continue;
        }
        tempApproxPlayerPos.x -= squareDimension;
        if (!third && tempApproxPlayerPos == createdSquares[i].origin) {
          third = true;
          //Debug.Log("3rd "+i);
          continue;
        }
        tempApproxPlayerPos.y += squareDimension;
        if (!fourth && tempApproxPlayerPos == createdSquares[i].origin) {
          fourth = true;
          //Debug.Log("4th "+i);
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
    for(i = 0; i<squareDimension-MIN_BLOCK_SIDE; ) {
      Vector2 new_d = new Vector2(origin.x+i,origin.y+i);
      diagonalPointsOnSquare.Add(new_d);
      ++count;
      i+=Random.Range(MIN_BLOCK_SIDE,MAX_BLOCK_SIDE);
    }
    {
      i = squareDimension;
      Vector2 new_d = new Vector2(origin.x+i,origin.y+i);
      diagonalPointsOnSquare.Add(new_d);
      ++count;
    }
    for(int j = 0; j<count-1; ++j) {
      {
        // bottom left
        Vector2 position = diagonalPointsOnSquare[j];
        // top right
        Vector2 position2 = diagonalPointsOnSquare[j+1];
        // top left
        Vector2 position1 = new Vector2(position.x, position2.y);
        // bottom right
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

  void CreateBlocks (Transform fullSquare, Vector2 blockPosition, Vector2 b0, Vector2 b1, Vector2 b2, Vector2 b3) {
    GameObject block = new GameObject();

    block.transform.parent = fullSquare.transform;
    block.transform.Translate (blockPosition.x, 0, blockPosition.y);

    int n_buildings_x = Random.Range(1, MAX_BUILDINGS_PER_BLOCK_SIDE+1);
    int n_buildings_y = Random.Range(1, MAX_BUILDINGS_PER_BLOCK_SIDE+1);
    float blockWidth, blockLength;
    float buildingMeanWidth, buildingMeanLength;

    // Reserve some space for the streets surrounding the buildings
    // bottom left
    b0 += new Vector2(STREET_WIDTH,STREET_WIDTH);
    // top left
    b1 += new Vector2(STREET_WIDTH,-STREET_WIDTH);
    // top right
    b2 += new Vector2(-STREET_WIDTH,-STREET_WIDTH);
    // bottom right
    b3 += new Vector2(-STREET_WIDTH,STREET_WIDTH);


    blockWidth = (b0.x - b2.x);
    blockLength = (b0.y - b2.y);

    buildingMeanWidth = blockWidth/n_buildings_x;
    buildingMeanLength = blockLength/n_buildings_y;

    Vector2[] buildingPoints = new Vector2[(n_buildings_x+1)*(n_buildings_y+1)];

    for (int x = 0; x <= n_buildings_x; x++) {
      float x_pos = x * (buildingMeanWidth);
      // TODO: Randomize points without losing the right position
      //if (x != 0 && x != n_buildings_x) x_pos += Random.Range(-buildingMeanWidth/2, buildingMeanWidth/2);
      
      for (int y = 0; y <= n_buildings_y; y++) {
        float y_pos = y * (buildingMeanLength);
        // TODO: Randomize points without losing the right position
        //if (y != 0 && y != n_buildings_y) y_pos += Random.Range(-buildingMeanLength/2, buildingMeanLength/2);
        
        buildingPoints[x*(n_buildings_y+1) + y] = new Vector2(x_pos, y_pos);
      }
    }

    for (int i = 0; i < n_buildings_x; i++) {
      for (int j = 0; j < n_buildings_y; j++) {
        int buildingNumber = i*n_buildings_y + j;
        int bottomLeft = buildingNumber+i;
        int topLeft = bottomLeft+1;
        int bottomRight = bottomLeft+(n_buildings_y+1);
        int topRight = bottomRight+1;

        Vector2 buildingPosition = buildingPoints[bottomLeft];//new Vector2(i*buildingMeanWidth, j*buildingMeanLength);

        if ((n_buildings_x < 3 || n_buildings_y < 3) ||     (buildingPoints[bottomLeft].x == 0.0f || buildingPoints[bottomLeft].y == 0.0f ) ||
                                                            (buildingPoints[topLeft].x == 0.0f || buildingPoints[topLeft].y == blockLength ) ||
                                                            (buildingPoints[topRight].x == blockWidth || buildingPoints[topRight].y == blockLength ) ||
                                                            (buildingPoints[bottomRight].x == blockWidth || buildingPoints[bottomRight].y == 0.0f )     )
          CreateBuildings(block.transform, buildingPosition, buildingPoints[bottomLeft], buildingPoints[topLeft], buildingPoints[topRight], buildingPoints[bottomRight]);
        else {
          // TODO: Place something else (ex: tree, playground, field, fountain)
        }

      }
    }
    block.transform.position = Vector3.zero;

  }

  void CreateBuildings (Transform fullBlock, Vector2 buildingPosition, Vector2 b0, Vector2 b1, Vector2 b2, Vector2 b3) {
    // TODO: Make it create blocks instead of buildings
    GameObject[] building = new GameObject[MAX_FLOORS];
    
    GameObject buildingPos = new GameObject ();
    building[0] = new GameObject ();
    building[0].transform.parent = buildingPos.transform;
    buildingPos.transform.parent = fullBlock.transform;
    BoxMesh boxMesh = building[0].AddComponent("BoxMesh") as BoxMesh;
    
    boxMesh.p0 = b0;
    boxMesh.p1 = b1;
    boxMesh.p2 = b2;
    boxMesh.p3 = b3;
    
    // pick a texture randomly
    boxMesh.buildingTex = "building" + Random.Range (1, N_TEXTURES);
    
    // Pile boxes in a random height
    int numberOfFloors = Random.Range(MIN_FLOORS, MAX_FLOORS);
    for(int i = 1; i < numberOfFloors; i++) {
      building[i] = Object.Instantiate(building[0]) as GameObject;
      building[i].transform.parent = buildingPos.transform;
      building[i].transform.Translate (0, 4*i, 0);
    }


    buildingPos.transform.Translate (buildingPosition.x, 0, buildingPosition.y);
  }
}
