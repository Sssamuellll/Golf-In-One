using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; //assigning the cursor's visibility to false hiding the cursor.
        Cursor.lockState = CursorLockMode.Locked; //assigning the cursor lock mode to locked, to the cursor's lockedState.
    }
}
