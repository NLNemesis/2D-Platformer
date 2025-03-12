using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [Header("Action")]
    public KeyCode Interaction;
    public KeyCode Inventory;
    public KeyCode Attack;
    public KeyCode Slide;
    public KeyCode Dash;

    [Header("Movement")]
    public KeyCode PosInputX;
    public KeyCode NegInputX;
    public KeyCode PosInputY;
    public KeyCode NegInputY;
}
