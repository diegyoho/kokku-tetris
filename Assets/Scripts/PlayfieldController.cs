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

    float lastTimeFall;

    void Start() {
        Setup();
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
}
