using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PlayfieldController :
    SingletonMonoBehaviour<PlayfieldController> {
    
    public Vector2Int dimensions;
    public float gravity;
    public int currentLevel = 1;

    [Header("Info Tetraminoes Positions")]
    public Transform nextPosition;
    public Transform holdPosition;
    public static Block[, ] matrix;
    public static bool canHold = true;

    List<TetrominoBase> bag = new List<TetrominoBase>();

    float lastTimeFall;
    float lastGravity;

    TetrominoBase currentTetromino;
    TetrominoBase nextTetromino;
    TetrominoBase holdTetromino;

    Coroutine das;

    int rowsRequired = 10;
    int _clearedRowsCount = 0;
    int clearedRowsCount {
        get { return _clearedRowsCount; }
        set {
            _clearedRowsCount = value;
            if(_clearedRowsCount >= rowsRequired) {
                LevelUp();
            }
        }
    }

    void Update() {
        if(currentTetromino) {
            HandleInput();
        
            if(Time.time - lastTimeFall >= Speed(gravity)) {
                currentTetromino.Drop();
                lastTimeFall = Time.time;
            }
        }
    }

    public static void Setup(int intialLevel = 1) {
        instance.currentLevel = intialLevel;

        matrix = new Block[
            instance.dimensions.x,
            instance.dimensions.y
        ];

        instance.transform.position = new Vector3(
            (instance.dimensions.x / 2f) - .5f,
            (instance.dimensions.y / 2f) - .5f,
            .5f
        );

        instance.transform.localScale = new Vector3(
            instance.dimensions.x, instance.dimensions.y, 1
        );

        instance.GetComponent<Renderer>().material.SetTextureScale(
            "_MainTex", instance.dimensions
        );

        instance.gravity = GameData.GetGravityForLevel(instance.currentLevel);
        instance.rowsRequired = 10 * instance.currentLevel;
        GameplayUIController.UpdateLevel();

        instance.FillBag();
        instance.SetNextTetramino();
    }

    void HandleInput() {
        // Rotation
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            currentTetromino.WallKick(1);
        } else if(Input.GetKeyDown(KeyCode.Z)) {
            currentTetromino.WallKick(-1);
        }

        // Move Horizontal
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            if(das != null) {
                StopCoroutine(das);
                das = null;
            }

            currentTetromino.Move(Vector2Int.right);
            das = StartCoroutine(DelayAutoShift(Vector2Int.right));
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(das != null) {
                StopCoroutine(das);
                das = null;
            }

            currentTetromino.Move(Vector2Int.left);
            das = StartCoroutine(DelayAutoShift(Vector2Int.left));
        }

        // Soft Drop
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            lastGravity = PlayfieldController.instance.gravity;
            gravity = 1/3f;
        } else if(Input.GetKeyUp(KeyCode.DownArrow)) {
            gravity = lastGravity;
        }

        // Hold
        if(canHold && Input.GetKeyDown(KeyCode.Space)) {
            Hold();
        }
    }

    IEnumerator DelayAutoShift(Vector2Int position) {
        yield return new WaitForSeconds(Speed(.05f));

        while(
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.LeftArrow)
        ) {
            if(Input.GetKey(KeyCode.RightArrow)) {
                currentTetromino.Move(Vector2Int.right);
            } else if(Input.GetKey(KeyCode.LeftArrow)) {
                currentTetromino.Move(Vector2Int.left);
            }

            yield return new WaitForSeconds(Speed(1f/3f));
        }
    }

    public static float Speed(float gravity) {
        return 1f/(60f * gravity);
    }

    public static bool ValidPosition(
        TetrominoBase tetromino
    ) {

        foreach(Block block in tetromino.blocks) {
            if(
                (
                    tetromino.isLanded &&
                    block.matrixPosition.y >= instance.dimensions.y
                ) ||
                block.matrixPosition.x < 0 ||
                block.matrixPosition.x >= instance.dimensions.x ||
                block.matrixPosition.y < 0 ||
                (
                    block.matrixPosition.y < instance.dimensions.y &&
                    matrix[
                        block.matrixPosition.x,
                        block.matrixPosition.y
                    ]
                )
            ) {
                return false;
            }
        }

        return true;
    }

    void FillBag() {
        bag = new List<TetrominoBase>(Resources.LoadAll<TetrominoBase>("Prefabs/Tetrominoes"));
        bag.Shuffle();
    }

    public static void StartSpawn() {
        instance.StartCoroutine(
            instance.DelaySpawn()
        );
    }

    IEnumerator DelaySpawn() {
        yield return new WaitForSeconds(1f);
        SpawnTetromino();
        lastTimeFall = Time.time;
    }

    public static void SpawnTetromino() {

        instance.nextTetromino.transform.SetParent(null);
        instance.nextTetromino.transform.position = new Vector3(
            instance.dimensions.x/2 - 1,
            instance.dimensions.y
        );
        
        instance.currentTetromino = instance.nextTetromino;

        instance.SetNextTetramino();
    }

    void SetNextTetramino() {
        nextTetromino = instance.bag[0];
        instance.bag.RemoveAt(0);

        nextTetromino = Instantiate(
            nextTetromino.gameObject,
            nextPosition.position -
            (Vector3) nextTetromino.centerOffset,
            Quaternion.identity,
            nextPosition
        ).GetComponent<TetrominoBase>();

        if(instance.bag.Count == 0)
            instance.FillBag();
    }

    void Hold() {
        canHold = false;
        TetrominoBase toHold = currentTetromino;
        
        currentTetromino.CancelLock();
        currentTetromino.transform.SetParent(holdPosition);
        currentTetromino.transform.position = (
            holdPosition.position - (Vector3) currentTetromino.centerOffset
        );

        if(holdTetromino) {
            holdTetromino.transform.SetParent(null);
            holdTetromino.transform.position = new Vector3(
                instance.dimensions.x/2 - 1,
                instance.dimensions.y
            );
            currentTetromino = holdTetromino;
        } else {
            SpawnTetromino();
        }

        holdTetromino = toHold;
    }

    void LevelUp() {
        currentLevel++;
        gravity = GameData.GetGravityForLevel(currentLevel);
        clearedRowsCount -= 10;
        rowsRequired = 10;
        GameplayUIController.UpdateLevel();
        SoundController.PlaySfx(GameData.GetAudioClip("Level Up"));
    }

    public static void ClearRows() {
        List<Block> cleared = new List<Block>();
        int rowsCleared = 0;

        for(int y = 0; y < instance.dimensions.y; ++y) {
            cleared.Clear();

            for(int x = 0; x < instance.dimensions.x; ++x) {
                if(matrix[x, y]) {
                    cleared.Add(matrix[x, y]);
                    
                    if(rowsCleared > 0) {
                        matrix[x, y - rowsCleared] = matrix[x, y];
                        matrix[x, y - rowsCleared].SetPosition(
                            new Vector2Int(x, y - rowsCleared)
                        );

                        matrix[x, y] = null;
                    }
                }
            }

            if(cleared.Count == instance.dimensions.x) {
                cleared.ForEach(block => Destroy(block.gameObject));
                rowsCleared++;
            }
        }

        if(rowsCleared > 0) {
            GameController.Score(rowsCleared);
            GameplayUIController.UpdateScore();
            SoundController.PlaySfx(GameData.GetAudioClip("Clear"));
            instance.clearedRowsCount += rowsCleared;
        }
    }

    public static void GameOver() {
        GameplayUIController.ShowGameOver();
        SoundController.PlaySfx(GameData.GetAudioClip("Top out"));
        instance.currentTetromino = null;
    }
}
