using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;


public enum TetraminoLetter {
    I,
    O,
    L,
    T,
    J,
    S,
    Z
}

public class TetraminoBase : MonoBehaviour {

    public TetraminoLetter letter;
    public int currentOrientation;
    public Vector2Int[] offsets = new Vector2Int[4];

    public List<Block> blocks = new List<Block>();

    Vector2Int matrixPosition {
        get {
            return new Vector2Int(
                (int) transform.position.x,
                (int) transform.position.y
            );
        }
    }

    void Start() {
        blocks.AddRange(transform.GetComponentsInChildren<Block>());
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.X)) {
            Rotate(1);
        } else if(Input.GetKeyDown(KeyCode.Z)) {
            Rotate(-1);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            Move(matrixPosition + Vector2Int.right);
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move(matrixPosition + Vector2Int.left);
        }
    }

    void Rotate(int sign) {
        int nextOrientation = Miscellaneous.Mod(currentOrientation + sign, 4);
            
        Vector2Int offset = (
            offsets[currentOrientation] - offsets[nextOrientation]
        );
        
        transform.localEulerAngles = new Vector3(
            0, 0, (int) transform.localEulerAngles.z - (90 * sign)
        );

        SetPosition(matrixPosition + offset);

        currentOrientation = nextOrientation;
    }

    void SetPosition(Vector2Int position) {
        transform.position = new Vector3(position.x, position.y);
    }

    void Move(Vector2Int position) {
        Vector2Int lastPosition = matrixPosition;
        SetPosition(position);
        
        if(!PlayfieldController.ValidPosition(this))
            SetPosition(lastPosition);
    }
}
