using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    public Vector2Int spawnCoordinates;
    Grid grid;
    PlayerInputActions playerInputActions;
    public GameObject currentTile;
    public GameObject nextTile;
    public Vector2Int GetSpawnCoord(){
        return spawnCoordinates;
    }
    public GameObject GetCurrentTile(){
        return currentTile;
    }
    void Awake(){
        grid = FindObjectOfType<Grid>();
        playerInputActions = new PlayerInputActions();

        // Enable input actions and subscribe to the Move action
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += Move;
        playerInputActions.Player.LookRight.performed += LookRight;
        playerInputActions.Player.LookLeft.performed += LookLeft;

        StartCoroutine(SpawnPlayer()); //guards against missing reference while tiles are generated
    }

    void Update(){

    }
    public void Move(InputAction.CallbackContext context){
        Vector2 moveDirection = context.ReadValue<Vector2>();
        Vector3 currentDirection = this.transform.forward;

        FindNextTile(moveDirection, currentDirection);
        if (nextTile != null) {
            MoveToTile(nextTile.transform.parent.gameObject);
        }
    }
    public void LookRight(InputAction.CallbackContext context)
    {
        transform.LookAt(transform.position + transform.right);
    }

    public void LookLeft(InputAction.CallbackContext context)
    {
        transform.LookAt(transform.position - transform.right);
    }

    void FindNextTile(Vector2 moveDirection, Vector3 currentDirection){
        if (currentTile.GetComponentInChildren<Tile>() == null) {return;} // guards against null

        if (moveDirection.y == 1) {
            if (currentDirection == Vector3.forward) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetNorthTile();
            }
            else if (currentDirection == Vector3.back) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetSouthTile();
            }
            else if (currentDirection == Vector3.left) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetWestTile();
            }
            else if (currentDirection == Vector3.right) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetEastTile();
            }
        }
        else if (moveDirection.y == -1) {
            if (currentDirection == Vector3.forward) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetSouthTile();
            }
            else if (currentDirection == Vector3.back) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetNorthTile();
            }
            else if (currentDirection == Vector3.left) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetEastTile();
            }
            else if (currentDirection == Vector3.right) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetWestTile();
            }
        }
        else if (moveDirection.x == -1) {
            if (currentDirection == Vector3.forward) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetWestTile();
            }
            else if (currentDirection == Vector3.back) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetEastTile();
            }
            else if (currentDirection == Vector3.left) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetSouthTile();
            }
            else if (currentDirection == Vector3.right) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetNorthTile();
            }
        }
        else if (moveDirection.x == 1)
        {
            if (currentDirection == Vector3.forward) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetEastTile();
            }
            else if (currentDirection == Vector3.back) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetWestTile();
            }
            else if (currentDirection == Vector3.left) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetNorthTile();
            }
            else if (currentDirection == Vector3.right) {
                nextTile = currentTile.GetComponentInChildren<Tile>().GetSouthTile();
            }
        }
    }

    void MoveToTile (GameObject tile){
        if (tile.GetComponentInChildren<Tile>().GetIsWall()) {return;}
        this.transform.position = tile.transform.position;
        currentTile = tile;
    }

    IEnumerator SpawnPlayer(){
        yield return new WaitForEndOfFrame();
        GameObject spawnTile = grid.GetTile(spawnCoordinates.x, spawnCoordinates.y).transform.parent.gameObject;
        this.transform.position = spawnTile.transform.position;
        currentTile = spawnTile;
    }
}
