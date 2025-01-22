using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 0;
    private float movementX;
    private float movementY; 
    private Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
     rb = GetComponent <Rigidbody>();    
    }
   void OnMove (InputValue movementValue)
   {
    Vector2 movementVector = movementValue.Get<Vector2>(); 
    movementX = movementVector.x; 
    movementY = movementVector.y; 
    movement = new Vector3 (movementX, 0.0f, movementY);
   }
    private void FixedUpdate() 
   {
   //rb.AddForce(movementVector); 
   rb.AddForce(movement * speed);
   }
}
