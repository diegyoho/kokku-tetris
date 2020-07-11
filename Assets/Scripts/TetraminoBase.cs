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

    void Start() {
        
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            ComputeOrientation(1);
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            ComputeOrientation(-1);
        }
    }

    void ComputeOrientation(int sign) {
        int nextOrientation = Miscellaneous.Mod(currentOrientation + sign, 4);
            
        Vector2Int offset = (
            offsets[currentOrientation] - offsets[nextOrientation]
        );
        
        transform.position += new Vector3(offset.x, offset.y);
        transform.localEulerAngles = new Vector3(
            0, 0, (int) transform.localEulerAngles.z - (90 * sign)
        );

        currentOrientation = nextOrientation;
    }
}
