using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordinateMapper : MonoBehaviour {
    // Cache references
    Vector2Int coordinates = new Vector2Int();
    Tile tile;
    TileMap tileMapGenerator;
    float coordinateMapOffset;
    TextMeshPro label;
    Color labelColor;

    // Accesors and Mutators
    public void SetLabelColor(Color color) {
        labelColor = color;
    }

    void Awake() {
        // Get coordinate map offset from tile generator
        tile = GetComponent<Tile>();
        tileMapGenerator = FindObjectOfType<TileMap>();
        coordinateMapOffset = tileMapGenerator.GetCoordinateMapOffset();

        // Reference the label and color
        label = GetComponent<TextMeshPro>();
        labelColor = Color.white;
    }

    void Update() {
        if (Application.isPlaying) { return; }
        DisplayCoordinates();
        UpdateObjectName();
    }

    void DisplayCoordinates() {   
        // Get coordinates and set label text
        coordinates.x = tile.GetCoordinates().x;
        coordinates.y = tile.GetCoordinates().y;
        label.text = coordinates.y + "," + coordinates.x;

        // Position coordinate map in 2d view
        this.transform.position = new Vector3 (coordinates.x * UnityEditor.EditorSnapSettings.move.x + coordinateMapOffset, coordinates.y * UnityEditor.EditorSnapSettings.move.y, 0);

        // Set color of label
        label.color = labelColor;
        label.alpha = 1;
    }

    void UpdateObjectName() {
        transform.name = coordinates.y + "," + coordinates.x;
    }
}
