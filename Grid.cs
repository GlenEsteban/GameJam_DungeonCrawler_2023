using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Grid : MonoBehaviour {
    [SerializeField] bool refresh;
    [SerializeField] GameObject floorTilePrefab;
    [SerializeField] GameObject wallTilePrefab;

    [SerializeField] Vector2Int gridDimensions;
    [SerializeField] List<Vector2Int> walls;
    public GameObject GetFloorTilePrefab() {
        return floorTilePrefab;
    }
    public GameObject GetWallTilePrefab() {
        return wallTilePrefab;
    }
    public GameObject GetTile(int x, int y) {
        if (x >= gridDimensions.x || x < 0 || y >= gridDimensions.y || y < 0) { return null;} // guards against non-existing tiles
        
        int tileChildIndex = (x * gridDimensions.y) + y;
        GameObject tile = this.transform.GetChild(tileChildIndex).gameObject.transform.GetChild(1).gameObject;
        return tile;
    }
    public void AddToWalls(Vector2Int coordinates){
        foreach (Vector2Int wall in walls){
            if (coordinates == wall) {return;}
        }
        walls.Add(coordinates);
    }
    public bool CheckIfWall(Vector2Int coordinates){
        foreach (Vector2Int wall in walls) {
            if (coordinates == wall) {
                return true;
            }
        }
        return false;
    }
    void Awake() {
        if (gridDimensions.x == 0 && gridDimensions.y == 0) { return; }
        GenerateGrid();
        StartCoroutine(ApplyTileTypes()); // Applies tile type after tile generation
    }

    void Update() {
        CheckGridChange();
    }
    void GenerateGrid() {
        for (int i = 0; i < gridDimensions.x; i++) {
            for (int j = 0; j < gridDimensions.y; j++)
            {
                // Instantiate tilePrefabs based on tile types
                Vector2 coordinates = new Vector2 (i, j);
                Vector3 position = new Vector3(i * UnityEditor.EditorSnapSettings.move.x, Random.Range(-.1f, .1f), j * UnityEditor.EditorSnapSettings.move.y);
                Instantiate(floorTilePrefab, position, Quaternion.identity, this.transform);

                // Assign coordinates to tile
                GameObject tile = GetTile(i, j);
                tile.GetComponent<Tile>().SetCoordinates(i, j);
            }
        }
    }

    IEnumerator ApplyTileTypes(){
        yield return new WaitForEndOfFrame();
        foreach (Vector2Int wallCoord in walls){
            GameObject wall = GetTile(wallCoord.x, wallCoord.y);
            if (wall != null){
                wall.GetComponent<Tile>().SetIsWall(true);
                wall.transform.parent.GetChild(1).GetComponent<CoordinateMapper>().SetLabelColor(Color.blue);
            }
        }
    }

    void CheckGridChange() {
        if (refresh || gridDimensions.x * gridDimensions.y != this.transform.childCount) { //check for refresh or change in dimensions 
            // clear grid before generating new grid
            while (transform.childCount > 0) { 
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            GenerateGrid();
            StartCoroutine(ApplyTileTypes());
            refresh = !refresh;
        }
    }
}
