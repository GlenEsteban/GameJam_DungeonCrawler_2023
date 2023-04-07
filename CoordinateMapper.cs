using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordinateMapper : MonoBehaviour
{
    [SerializeField] float coordinateMapOffset = 10f;
    TextMeshPro label;
    Color labelColor;
    Vector2Int coordinates = new Vector2Int();

    public void SetLabelColor(Color color) {
        labelColor = color;
    }
    void Awake() {
        label = GetComponent<TextMeshPro>();
        labelColor = Color.white;
        DisplayCoordinates();
    }

    void Update() {
        if(!Application.isPlaying) {
            DisplayCoordinates();
        } 
        else {
            this.GetComponent<TextMeshPro>().enabled = false;
            this.GetComponent<CoordinateMapper>().enabled = false;
        }
        UpdateObjectName();
    }

    void DisplayCoordinates() {   
        // Find coordinates and set label text
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);
        label.text = coordinates.y + "," + coordinates.x;

        // Position coordinate map in 2d view
        this.transform.position = new Vector3 (coordinates.x * UnityEditor.EditorSnapSettings.move.x, coordinates.y * UnityEditor.EditorSnapSettings.move.y + coordinateMapOffset, 0);

        // Set color of label
        label.color = labelColor;
        label.alpha = 1;
    }

    void UpdateObjectName() {
        transform.parent.name = coordinates.y + "," + coordinates.x;
    }
}
