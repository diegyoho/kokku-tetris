using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    
    public Vector2Int matrixPosition {
        get {
            return new Vector2Int(
                (int) transform.position.x,
                (int) transform.position.y
            );
        }
    }
}
