using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveActionX;
    InputAction moveActionY;

    [SerializeField]
    int speed = 5;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveActionX = playerInput.FindAction("MoveX");
        moveActionY = playerInput.FindAction("MoveY");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Move () {
        
    }
}
