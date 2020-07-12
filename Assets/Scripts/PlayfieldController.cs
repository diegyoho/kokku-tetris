using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PlayfieldController :
    SingletonMonoBehaviour<PlayfieldController> {
        
    public Vector2Int dimensions;
    public float gravity;
    public static Block[, ] matrix;
    public static TetrominoBase currentTetromino;

    List<GameObject> bag = new List<GameObject>();

    float lastTimeFall;
    float lastGravity;

    Coroutine das;

    void Start() {
        Setup();
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

    void Setup() {
        matrix = new Block[
            dimensions.x,
            dimensions.y
        ];

        transform.position = new Vector3(
            (dimensions.x / 2f) - .5f,
            (dimensions.y / 2f) - .5f,
            .5f
        );

        transform.localScale = new Vector3(
            dimensions.x, dimensions.y, 1
        );

        GetComponent<Renderer>().material.SetTextureScale(
            "_MainTex", dimensions
        );

        FillBag();

        StartCoroutine(DelaySpawn());
    }

    IEnumerator DelaySpawn() {
        yield return new WaitForSeconds(1f);
        SpawnTetromino();
        lastTimeFall = Time.time;
    }

    void HandleInput() {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.X)) {
            currentTetromino.WallKick(1);
        } else if(Input.GetKeyDown(KeyCode.Z)) {
            currentTetromino.WallKick(-1);
        }

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

        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            lastGravity = PlayfieldController.instance.gravity;
            gravity = 1/3f;
        } else if(Input.GetKeyUp(KeyCode.DownArrow)) {
            gravity = lastGravity;
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
        bag = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Tetrominoes"));
        bag.Shuffle();
    }

    public static void SpawnTetromino() {
        GameObject tetromino = instance.bag[0];
        instance.bag.RemoveAt(0);

        Instantiate(
            tetromino,
            new Vector3(
                instance.dimensions.x/2 - 1,
                instance.dimensions.y
            ),
            Quaternion.identity
        );

        if(instance.bag.Count == 0)
            instance.FillBag();
    }

    public static void ClearRows() {
        int fallHeight = 0;
        List<Block> cleared = new List<Block>();
        for(int y = 0; y < instance.dimensions.y; ++y) {
            cleared.Clear();

            for(int x = 0; x < instance.dimensions.x; ++x) {
                if(matrix[x, y]) {
                    cleared.Add(matrix[x, y]);
                    
                    if(fallHeight > 0) {
                        matrix[x, y - fallHeight] = matrix[x, y];
                        matrix[x, y - fallHeight].SetPosition(
                            new Vector2Int(x, y - fallHeight)
                        );

                        matrix[x, y] = null;
                    }
                }
            }

            if(cleared.Count == instance.dimensions.x) {
                cleared.ForEach(block => Destroy(block.gameObject));
                fallHeight++;
            }
        }
    }

    public static void GameOver() {
        GameplayUIController.ShowGameOver();
        currentTetromino = null;
    }
}
