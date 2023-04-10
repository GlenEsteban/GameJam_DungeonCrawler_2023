using System.Collections;
using UnityEngine;

public enum TileTerrain { None, Floor, Wall }
public enum TileEncounter { None, Enemy }

[ExecuteAlways]
public class Tile : MonoBehaviour {
    [Header("Actions")]
    [SerializeField] bool saveTile;

    [Header("Terrain")]
    [SerializeField] TileTerrain tileTerrain;
    [SerializeField] GameObject terrainObj;
    [Tooltip("The max deviation from the original position")]
    [SerializeField] float tileHeightDeviationApex = .1f;

    [Header("Encounters")]
    [SerializeField] TileEncounter tileEncounter;
    [SerializeField] GameObject encounterObj;

    [Header("Tile Coordinates and Links")]
    [SerializeField] Vector2Int coordinates;
    [SerializeField] public GameObject northTile;
    [SerializeField] public GameObject southTile;
    [SerializeField] public GameObject westTile;
    [SerializeField] public GameObject eastTile;

    // Cached References
    TileMap tileMapGenerator;
    CoordinateMapper coordinateMapper;

    // Accessors and Mutators
    public void SetSaveTile(bool state) {
        saveTile = state;
    }
    public TileTerrain GetTileTerrain() {
        return tileTerrain;
    }
    public void SetTileTerrain(TileTerrain tileType) {
        this.tileTerrain = tileType;
    }
    public TileEncounter GetTileEncounter() {
        return tileEncounter;
    }
    public void SetTileEncounter(TileEncounter tileEncounter) {
        this.tileEncounter = tileEncounter;
    }
    public Vector2Int GetCoordinates() {
        return coordinates;
    }
    public void SetCoordinates(int x, int y) {
        coordinates.x = x;
        coordinates.y = y;
    }
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
    
    void Awake() {
        // Cache references
        tileMapGenerator = FindObjectOfType<TileMap>();
        coordinateMapper = GetComponent<CoordinateMapper>();
    }

    void Start() {
        StartCoroutine(LinkDirectionTiles());
        StartCoroutine(UpdateTerrain());
        StartCoroutine(UpdateEncounters());
    }

    void Update() {
        if (!Application.isPlaying && saveTile) {
            // Clear existing terrain and encounter objects
            DestroyChildren();

            // Update and cache terrain and encounters after tile map generation
            StartCoroutine(UpdateTerrain());
            StartCoroutine(UpdateEncounters());
        }
    }

    IEnumerator LinkDirectionTiles() {
        // Find linked tiles after tile map generation
        yield return new WaitForEndOfFrame(); 
        northTile = tileMapGenerator.GetTile(coordinates.x, coordinates.y + 1);
        southTile = tileMapGenerator.GetTile(coordinates.x, coordinates.y - 1);
        westTile = tileMapGenerator.GetTile(coordinates.x - 1, coordinates.y);
        eastTile = tileMapGenerator.GetTile(coordinates.x + 1, coordinates.y);
    }

    IEnumerator UpdateTerrain() {
        // Wait until after tile map generation
        yield return new WaitForEndOfFrame(); 

        // Set coordinate color based on terrain type
        if (tileTerrain == TileTerrain.None) {
            coordinateMapper.SetLabelColor(Color.grey);
        }
        else if (tileTerrain == TileTerrain.Floor) {
            coordinateMapper.SetLabelColor(Color.white);
        }   
        else if (tileTerrain == TileTerrain.Wall) {
            coordinateMapper.SetLabelColor(Color.blue);
        }

        GenerateTerrainObject();
        
        // Finish updating
        saveTile = false; 
    }

    void GenerateTerrainObject() {
        if (terrainObj == null) { return; }

        // Generate terrain object based on coordinates and with random 
        float tileHeightDeviation = Random.Range(-tileHeightDeviationApex, tileHeightDeviationApex);
        Vector3 position = new Vector3(coordinates.x * UnityEditor.EditorSnapSettings.move.x, tileHeightDeviation, coordinates.y * UnityEditor.EditorSnapSettings.move.x);
        Quaternion rotation = Quaternion.Euler(transform.rotation.x, Random.Range(0,3) * 90, transform.rotation.z);
        Instantiate(terrainObj, position, rotation, this.transform);
    }
    
    IEnumerator UpdateEncounters() {
        // Wait unitl after tile map generation
        yield return new WaitForEndOfFrame(); 

        // Overide terrain color and set color based on encounter
        if (tileEncounter == TileEncounter.Enemy) {
            coordinateMapper.SetLabelColor(Color.red);
        }

        // Finish updating
        saveTile = false;
    }

    void DestroyChildren(){
        // Destroy existing terrain and encounter objects
        while (transform.childCount > 0) { 
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
