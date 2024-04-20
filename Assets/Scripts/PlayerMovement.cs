using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float directionX = 0;
    [SerializeField] float directionZ = 0;
    [SerializeField] Vector3 _moveDirection = Vector3.zero;

    public float moveSpeed; // Default
    public float jumpSpeed; // Jump
    public float sprintMultiplier = 2f; //Sprint speed multiplier

    CharacterController characterController;

    public bool isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        // Player is Check Grounded or Not
        isGrounded = characterController.isGrounded;

        // Ensure the player stays grounded
        if (isGrounded && _moveDirection.y < 0)
        {
            _moveDirection.y = -2f;
        }
        
        // X Axis (A=-1 To D=1)
        directionX = Input.GetAxis("Horizontal");
        // Z Axis (W=1 To S=-1)
        directionZ = Input.GetAxis("Vertical");

        // X-Axiz and Y-Axiz Value Normalized, Bcoz When you press (W+A Or W+D Or S+A Or S+D) player moves at the same speed in all directions
        Vector3 move = (transform.right * directionX + transform.forward * directionZ).normalized;

        // Here Y is not Normalized, only X-Axiz and Y-Axiz
        _moveDirection = new Vector3(move.x, _moveDirection.y, move.z);

        // Player Jump (Press Space)
        if (isGrounded && Input.GetButton("Jump"))
        {
            _moveDirection.y = jumpSpeed;
        }

        // When Left-Shift Press Sprint Run Active
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintMultiplier : moveSpeed;

        // Assign Gravity Value 9.81f
        _moveDirection.y += Physics.gravity.y * Time.deltaTime;

        // Assign Character Movement
        characterController.Move(_moveDirection * currentSpeed * Time.deltaTime);

        if (_moveDirection.y <= -15f)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("First Person Movement");
        }
    }
}
