using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Tile : MonoBehaviour
{
    [SerializeField] bool isWall;
    [SerializeField] bool isEnemy;
    [SerializeField] Vector2Int coordinates;
    GameObject northTile;
    GameObject southTile;
    GameObject westTile;
    GameObject eastTile;
    Transform mesh;
    Grid grid;
    bool isUpdated;

    public GameObject GetNorthTile() {
        return northTile;
    }
    public GameObject GetSouthTile() {
        return southTile;
    }
    public GameObject GetWestTile() {
        return westTile;
    }
    public GameObject GetEastTile() {
        return eastTile;
    }
    public void SetIsWall(bool state) {
        isWall = state;
    }
    public void SetIsEnemy(bool state) {
        isEnemy = state;
    }
    public bool IsWall() {
        return isWall;
    }
    public bool IsEnemy(){
        return isEnemy;
    }
    public void SetCoordinates(int x, int y) {
        coordinates.x = x;
        coordinates.y = y;
    }
    void Awake() {
        mesh = this.transform.parent.GetChild(0);
        grid = FindObjectOfType<Grid>();
        isWall = false;
        isUpdated = false;

        StartCoroutine(LinkDirectionTiles());
        StartCoroutine(UpdateTileType());
        RandomizeTile(this.gameObject);
    }

    void Update(){
        StartCoroutine(UpdateTileType());
    }

    IEnumerator LinkDirectionTiles() {
        yield return new WaitForEndOfFrame(); // Find linked tiles after tile generation
        northTile = grid.GetTile(coordinates.x, coordinates.y + 1);
        southTile = grid.GetTile(coordinates.x, coordinates.y - 1);
        westTile = grid.GetTile(coordinates.x - 1, coordinates.y);
        eastTile = grid.GetTile(coordinates.x + 1, coordinates.y);
    }

    IEnumerator UpdateTileType(){
        yield return new WaitForEndOfFrame();
        if(!isUpdated) {            
            if (isWall) {
                grid.AddToWalls(coordinates);
                GameObject wallTileMesh = grid.GetWallTilePrefab();
                Quaternion rotation = Quaternion.Euler(transform.rotation.x, Random.Range(0,3) * 90, transform.rotation.y);
                Instantiate(wallTileMesh, this.transform.parent.position, rotation , mesh);

                // Update coordinate color on coordinate map
                transform.parent.GetComponentInChildren<CoordinateMapper>().SetLabelColor(Color.blue);

                // Finish updating tile
                isUpdated = true;
            }
            else if (isEnemy) {
                grid.AddToEnemies(coordinates);
                grid = FindObjectOfType<Grid>();
                GameObject enemy = grid.GetEnemyTilePrefab();
                GameObject player = FindObjectOfType<PlayerController>().gameObject;
                Instantiate(enemy, this.transform.parent.position, Quaternion.identity, transform);
                
                // Update coordinate color on coordinate map
                transform.parent.GetComponentInChildren<CoordinateMapper>().SetLabelColor(Color.red);

                // Finish updating tile
                isUpdated = true;
            }
        }
    }

    void RandomizeTile(GameObject tile) {
        if (tile != null) {
            //Randomize height
            tile.transform.position = new Vector3(tile.transform.position.x, Random.Range(-.1f, .1f), tile.transform.position.z);

            //Randomize rotation
            float randomRotationY = Random.Range(0,3) * 90;
            mesh.transform.rotation = Quaternion.Euler(transform.rotation.x, randomRotationY, transform.rotation.z);
        }
    }
}
