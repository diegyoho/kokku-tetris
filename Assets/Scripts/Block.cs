using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    
    public Vector2Int matrixPosition {
        get {
            return new Vector2Int(
                (int) Mathf.Round(transform.position.x),
                (int) Mathf.Round(transform.position.y)
            );
        }
    }

    public void SetPosition(Vector2Int position) {
        transform.position = new Vector3(position.x, position.y);
    }

    public void Lock() {
        transform.SetParent(null);
        
        PlayfieldController.matrix[
            matrixPosition.x,
            matrixPosition.y
        ] = this;
    }
}
