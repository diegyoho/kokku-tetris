using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[System.Serializable]
public class OffsetList {
    public Vector2Int[] offsets = new Vector2Int[4];
}

public enum TetrominoLetter {
    I,
    O,
    L,
    T,
    J,
    S,
    Z
}

public class TetrominoBase : MonoBehaviour {

    public TetrominoLetter letter;
    public int currentOrientation;
    public List<Block> blocks = new List<Block>();
    public Vector2Int[] basicOffsets = new Vector2Int[4];
    public List<OffsetList> offsetsList = new List<OffsetList>();

    Vector2Int matrixPosition {
        get {
            return new Vector2Int(
                (int) Mathf.Round(transform.position.x),
                (int) Mathf.Round(transform.position.y)
            );
        }
    }

    Coroutine landed;

    void Start() {
        blocks.AddRange(transform.GetComponentsInChildren<Block>());
        PlayfieldController.currentTetromino = this;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.X)) {
            WallKick(1);
        } else if(Input.GetKeyDown(KeyCode.Z)) {
            WallKick(-1);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            Move(Vector2Int.right);
        } else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move(Vector2Int.left);
        }
    }

    void SetPosition(Vector2Int position) {
        transform.position = new Vector3(position.x, position.y);
    }

    public void Move(Vector2Int position) {
        Vector2Int lastPosition = matrixPosition;
        SetPosition(matrixPosition + position);
        
        if(!PlayfieldController.ValidPosition(this))
            SetPosition(lastPosition);
        
        if(landed != null) {
            StopCoroutine(landed);
            landed = null;
        }
    }

    void Rotate(int sign, Vector2Int[] offsets) {
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

    void WallKick(int sign) {
        Rotate(sign, basicOffsets);

        if(!PlayfieldController.ValidPosition(this)) {
            Rotate(-sign, basicOffsets);

            foreach(OffsetList ol in offsetsList) {
                Rotate(sign, ol.offsets);

                if(PlayfieldController.ValidPosition(this))
                    break;

                Rotate(-sign, ol.offsets);
            }
        }

        if(landed != null) {
            StopCoroutine(landed);
            landed = null;
        }
    }

    public void Drop() {
        if(landed != null) return;

        Move(Vector2Int.down);
        
        Vector2Int lastPosition = matrixPosition;
        SetPosition(matrixPosition + Vector2Int.down);
        
        if(!PlayfieldController.ValidPosition(this)) {
            landed = StartCoroutine(LockDelay());
        }
        
        SetPosition(lastPosition);
    }

    IEnumerator LockDelay() {
        yield return new WaitForSeconds(.5f);

        blocks.ForEach(block => block.Lock());
        PlayfieldController.instance.SpawnTetromino();

        Destroy(gameObject);
    }
}
