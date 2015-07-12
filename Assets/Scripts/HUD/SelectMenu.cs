using UnityEngine;
using System.Collections;

public class SelectMenu : MonoBehaviour {

    public Transform[] cursorPositions;
    public Transform cursor;

    public CombatController combatController;

    public int cursorRow { get; private set; }
    public int cursorCol { get; private set; }

    private bool healSelected = false;
    private bool hasMovedLeft = false;
    private bool hasMovedRight = false;
    private bool hasMovedUp = false;
    private bool hasMovedDown = false;

    void OnEnable() {
        cursorRow = 0;
        cursorCol = 0;
        updateCursorPosition();
    }

    void Update() {
        if (!healSelected) {
            if (Input.GetAxisRaw("Horizontal") < 0 && !hasMovedLeft) {
                cursorCol = Mathf.Max(0, cursorCol - 1);
                hasMovedLeft = true;
                updateCursorPosition();
            }
            else if (Input.GetAxisRaw("Horizontal") > 0 && !hasMovedRight) {
                cursorCol = Mathf.Min(1, cursorCol + 1);
                hasMovedRight = true;
                updateCursorPosition();
            }
            else if (Input.GetAxisRaw("Horizontal") == 0) {
                hasMovedLeft = false;
                hasMovedRight = false;
            }

            if (Input.GetAxisRaw("Vertical") < 0 && !hasMovedDown) {
                cursorRow = (cursorRow + 1) % 3;
                hasMovedDown = true;
                updateCursorPosition();
            }
            else if (Input.GetAxisRaw("Vertical") > 0 && !hasMovedUp) {
                cursorRow = (cursorRow + 2) % 3;
                hasMovedUp = true;
                updateCursorPosition();
            }
            else if (Input.GetAxisRaw("Vertical") == 0) {
                hasMovedDown = false;
                hasMovedUp = false;
            }
        }
        else {
            if ((Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Vertical") < 0) && !hasMovedDown) {
                cursorRow = (cursorRow + 1) % 3;
                hasMovedDown = true;
                updateCursorPosition();
            }
            else if ((Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Vertical") > 0) && !hasMovedUp) {
                cursorRow = (cursorRow + 2) % 3;
                hasMovedUp = true;
                updateCursorPosition();
            }
            else if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) {
                hasMovedDown = false;
                hasMovedUp = false;
            }
        }
        
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Z)) {
#else
        if (Input.GetKeyDown(KeyCode.Joystick1Button0)) {
#endif
            if (cursorRow == 2 && cursorCol == 1) {
                if (canSelectHeal()) {
                    healSelected = true;
                    cursorCol = 2;
                    updateCursorPosition();
                }
                else {
                    Debug.Log("Didn't make a valid selection");
                }
            }
            else {
                if (combatController.makeSelectionFromHUD(cursorRow, cursorCol)) {
                    Debug.Log(cursorRow + " " + cursorCol);
                    combatController.confirmSelection();
                    gameObject.SetActive(false);
                    cursorRow = 0;
                    cursorCol = 0;
                    healSelected = false;
                }
                else {
                    Debug.Log("Didn't make a valid selection");
                }
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.X)) {
#else
        if (Input.GetKeyDown(KeyCode.Joystick1Button2)) {
#endif
            if (healSelected) {
                healSelected = false;
                cursorRow = 2;
                cursorCol = 1;
                updateCursorPosition();
            }
        }
    }

    private bool canSelectHeal() {
        return combatController.cadenceCombat.hasEnoughMp();
    }

    private void updateCursorPosition() {
        Transform cursorPosition;
        if (!healSelected) {
            cursorPosition = cursorPositions[cursorCol * 3 + cursorRow % 3];
        }
        else {
            cursorPosition = cursorPositions[6 + cursorRow % 3];
        }

        cursor.position = cursorPosition.position;
    }
}
