using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class TileMap : MonoBehaviour {
    [Header("Actions")]
    [SerializeField] bool saveTileMap;

    [Header("Tile Map Configurations")]
    [SerializeField] Vector2Int tileMapDimensions;
    [SerializeField] float coordinateMapOffset = -120;

    [Header("Cached Tile Generation References")]
    [SerializeField] GameObject tilePrefab;

    // Accessors and Mutators
    public GameObject GetTile(int x, int y) {
        // check if coordinates exist in tile map
        if (x >= tileMapDimensions.x || x < 0 || y >= tileMapDimensions.y || y < 0) { return null;} // guards against non-existing tiles
        
        // Get child based on coordinates
        int tileChildIndex = (x * tileMapDimensions.y) + y;
        GameObject tile = this.transform.GetChild(tileChildIndex).gameObject;
        return tile;
    }
    public float GetCoordinateMapOffset(){
        return coordinateMapOffset;
    }

    void Start() {
        // If tile map already exists or if dimensions are invalid, return
        if (transform.childCount > 0) { return; };
        if (tileMapDimensions.x <= 0 && tileMapDimensions.y <= 0) { return; }

        GenerateTileMap();
    }

    void Update() {
        RefreshTileMap();
    }

    void GenerateTileMap() {
        for (int i = 0; i < tileMapDimensions.x; i++) {
            for (int j = 0; j < tileMapDimensions.y; j++)
            {
                // Generate empty tiles
                Vector2 coordinates = new Vector2 (i, j);
                Instantiate(tilePrefab, transform.position, Quaternion.identity, this.transform); // position updated via Coordinate script

                // Assign coordinates to tile
                GameObject tile = GetTile(i, j);
                tile.GetComponent<Tile>().SetCoordinates(i, j);
            }
        }
    }

    void RefreshTileMap() {
        //check for refresh or change in dimensions 
        if (saveTileMap) { 
            // clear grid before generating new tile map
            while (transform.childCount > 0) { 
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            GenerateTileMap();

            // Finish updating
            saveTileMap = !saveTileMap;
        }
    }
}
