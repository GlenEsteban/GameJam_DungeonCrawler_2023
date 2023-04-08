using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions; //need this namespace to get access to Interactions script


public class PlayerController : MonoBehaviour {
    [SerializeField] bool isLerping;
    [SerializeField] bool isSlerping;
    [SerializeField] float movementSpeed = .5f;
    [SerializeField] float rotationSpeed = .3f;
    [SerializeField] Battle shield;
    [SerializeField] Battle sword;
    [SerializeField] Vector2Int spawnCoordinates;
    Grid grid;
    PlayerInputActions playerInputActions;
    GameObject currentTile;
    GameObject nextTile;
    bool isMoving;
    bool isBattling;

    private Vector2 moveDirection;

    public Vector2Int GetSpawnCoord() {
        return spawnCoordinates;
    }
    public GameObject GetCurrentTile() {
        return currentTile;
    }
    public bool IsBattling() {
        return isBattling;
    }
    void OnEnable() {
        playerInputActions.Enable();
    }

    void OnDisable() {
        playerInputActions.Disable();    
    }
    void Awake() {
        grid = FindObjectOfType<Grid>(); 
        playerInputActions = new PlayerInputActions();

        // Enable input actions and subscribe to the input actions
        playerInputActions.Player.Move.performed += Move;
        playerInputActions.Player.Move.canceled += CancelMove;

        playerInputActions.Player.Defend.performed += Defend;

        playerInputActions.Player.Attack.performed += Attack;

        playerInputActions.Player.LookRight.performed += LookRight;

        playerInputActions.Player.LookLeft.performed += LookLeft;
        
        // Spawn player after grid generation
        StartCoroutine(SpawnPlayer());
    }

    private void Update() {
        if (isBattling) {return;}
        CheckForMovement();
    }

    public void Move(InputAction.CallbackContext context) {
        isMoving = true;
        moveDirection = context.ReadValue<Vector2>();
    }
    void CancelMove(InputAction.CallbackContext context) {
        isMoving = false;
    }

    void Defend(InputAction.CallbackContext context) {
        if (shield == null || !isBattling) { return; }
        shield.Defend();
    }
    void Attack(InputAction.CallbackContext context) {
        if (sword == null || !isBattling) { return; }
        sword.Attack();
    }
    public void LookRight(InputAction.CallbackContext context) {
        if (isSlerping || isBattling) { return; }
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
        StartCoroutine(SlerpRotation(rotation, rotationSpeed));
    }

    public void LookLeft(InputAction.CallbackContext context) {
        if (isSlerping || isBattling) { return; }
        Quaternion rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
        StartCoroutine(SlerpRotation(rotation, rotationSpeed));
    }

    void CheckForMovement() {
        if (isMoving && !isLerping){ 
            Vector3 currentDirection = this.transform.forward;
            FindNextTile(moveDirection, currentDirection);
            if (nextTile != null) {
                CheckTile(nextTile.transform.parent.gameObject);
            }
        }
    }

    void FindNextTile(Vector2 moveDirection, Vector3 currentDirection) {
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
        else if (moveDirection.x == 1) {
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

    void CheckTile (GameObject tile) {
        if (isLerping) {return;}
        if (tile.GetComponentInChildren<Tile>().IsWall()) {return;}
        if (tile.GetComponentInChildren<Tile>().IsEnemy()) {
            //enter battle state
            isBattling = true;
            
            // Encounter enemy
            Enemy enemy = tile.GetComponentInChildren<Enemy>();
            if (shield == null) { return; }
            shield.StartBattle();
            enemy.EncounterEnemy();

            // Slerp rotation to enemy
            Quaternion rotation = Quaternion.LookRotation(transform.position + (tile.transform.parent.transform.position - transform.position));
            StartCoroutine(SlerpRotation(rotation, rotationSpeed));

            // Play battle BGM
            BGMPlayer bGMPlayer = FindObjectOfType<BGMPlayer>();
            bGMPlayer.PlayBattleBGM();
            return;
        }

        StartCoroutine(LerpPosition(tile.transform.position, movementSpeed));
        currentTile = tile;
    }

    IEnumerator SpawnPlayer() {
        yield return new WaitForEndOfFrame();
        GameObject spawnTile = grid.GetTile(spawnCoordinates.x, spawnCoordinates.y).transform.parent.gameObject;
        this.transform.position = spawnTile.transform.position;
        currentTile = spawnTile;
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration) {
        isLerping = true;
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration) {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        isLerping = false;
    }

    IEnumerator SlerpRotation(Quaternion targetRotation, float duration) {
        isSlerping = true;
        float time = 0;
        Quaternion startPosition = transform.localRotation;
        while (time < duration) {
            transform.rotation = Quaternion.Slerp(startPosition, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        isSlerping = false;
    }
}
