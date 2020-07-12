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

    void Start() {
        Setup();
        SpawnTetromino();
    }

    void Update() {
        if(Time.time - lastTimeFall >= 1f/(60*gravity)) {
            currentTetromino.Drop();
            lastTimeFall = Time.time;
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
    }

    public static bool ValidPosition(
        TetrominoBase tetromino
    ) {

        foreach(Block block in tetromino.blocks) {
            if(
                block.matrixPosition.y < instance.dimensions.y &&
                (
                    block.matrixPosition.x < 0 ||
                    block.matrixPosition.x >= instance.dimensions.x ||
                    block.matrixPosition.y < 0 ||
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

    public void SpawnTetromino() {
        GameObject tetromino = bag[0];
        bag.RemoveAt(0);

        Instantiate(
            tetromino,
            new Vector3(dimensions.x/2 - 1, dimensions.y),
            Quaternion.identity
        );

        if(bag.Count == 0)
            FillBag();
    }

    public void ClearRows() {
        int fallHeight = 0;
        List<Block> cleared = new List<Block>();
        for(int y = 0; y < dimensions.y; ++y) {
            cleared.Clear();

            for(int x = 0; x < dimensions.x; ++x) {
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

            if(cleared.Count == dimensions.x) {
                cleared.ForEach(block => Destroy(block.gameObject));
                fallHeight++;
            }
        }
    }
}
