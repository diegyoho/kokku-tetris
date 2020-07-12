using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PlayfieldController :
    SingletonMonoBehaviour<PlayfieldController> {
        
    public Vector2Int dimensions;
    public Block[, ] matrix;

    void Start() {
        Setup();
    }

    void Setup() {
        matrix = new Block[
            dimensions.x,
            dimensions.y
        ];

        transform.position = new Vector3(
            (dimensions.x / 2f) - .5f,
            (dimensions.y / 2f) - .5f
        );

        transform.localScale = new Vector3(
            dimensions.x, dimensions.y, 1
        );

        GetComponent<Renderer>().material.SetTextureScale(
            "_MainTex", dimensions
        );
    }

    public static bool ValidPosition(
        TetraminoBase tetramino
    ) {

        foreach(Block block in tetramino.blocks) {
            if(
                block.matrixPosition.x < 0 ||
                block.matrixPosition.x >= instance.dimensions.x ||
                block.matrixPosition.y < 0 ||
                (
                    block.matrixPosition.y < instance.dimensions.y &&
                    instance.matrix[
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
