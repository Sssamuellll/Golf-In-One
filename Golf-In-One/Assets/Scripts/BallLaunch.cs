using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class BallLaunch : MonoBehaviour
{
    //initialising variables
    public float force = 10;
    public Rigidbody rb;
    public Transform resetPos;
    public Transform direction;
    public GameObject tele1;
    public GameObject tele2;
    bool teleporting = false;
    bool teleport;
    bool shoot = true;
    Vector3 lastPos;

    //Initialising Audio effects
    public AudioSource hitSound;
    public AudioSource antigravSound;
    public AudioSource teleportSound;
    public AudioSource randomSound;
    public AudioSource winSound;
    public AudioSource outOfBoundsSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //Retrieving the Rigidboby component
        lastPos = resetPos.position; // assigning a transform position to a Vector3 variable.
    }
    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude == 0.0f && shoot == false) //tests the ball's velocity to determine if the ball is currently moving and if shoot is false.
        {
            shoot = true; //shoot defines whether the ball is able to be shot.
        }
        if (rb.velocity.magnitude > 0.5f && shoot == true) //tests whether shoot is true and the ball's velocity is greater than 0.5f.
        {
            shoot = false; //when shoot is set to false the player is unable to shoot the ball while its moving.
        }
        if (Input.GetKeyDown(KeyCode.Q) && force > 10 && shoot == true) //tests if the "Q" key has been input and if shoot is equal to true.
        {
            force = force - 10; //decrements force float variable by 10.
            direction.transform.localScale += new Vector3(-0.1f, -0.1f, -0.1f); //assigns new Vector3 transform scale parameters that decrease the scale of the shot indicator.
        }
        if (Input.GetKeyDown(KeyCode.E) && force < 30 && shoot == true) //tests if the "E" key has been input and if shoot is equal to true.
        {
            force = force + 10; //increments force variable be 10.
            direction.transform.localScale += new Vector3(+0.1f, +0.1f, +0.1f); // assigns new Vector3 transform scale parameters that increase the scale of the shot indicator.
        }
        if (Input.GetKeyDown(KeyCode.Space) && shoot == true) //tests if the "Space" key has been input and if shoot is equal to true.
        {
            hitSound.Play(); //plays the sound connected the variable.
            lastPos = rb.transform.position; //assigns the ball's current transform position to a Vector3 variable.
            rb.AddForce(-direction.transform.up * force, ForceMode.Impulse); //applies an impulse force to the ball in the direction of the shot indicator which is multiplied by the force variable.
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Antigrav")) //tests if the triggered game object has the tag "Antigrav".
        {
            rb.useGravity = false; //changes the ball so it's no longer affected by gravity.
            Vector3 antigrav = new Vector3(rb.velocity.x, rb.velocity.y + 500, rb.velocity.z); //initialises a new Vector3 with adjustments the ball's velocity as parameters to a Vector3 variable.
            rb.AddForce(antigrav, ForceMode.Acceleration); //applies an acceleration force to the ball using the recently initialised Vector3 variable as the direction the force is applied.
            antigravSound.Play(); //plays a sound effect once the above if statement is true.
        }
        if (other.gameObject.CompareTag("Teleport") && teleporting == false) //tests if the triggered game object has the tag "Teleport" and if teleporting is equal to false.
        {
            teleportSound.Play(); //plays a sound effect once the above statement is true.
            teleporting = true; //this boolean variable defines when the ball is teleporting.
            if (other.gameObject.name == "Teleport") //tests if the game object triggered is named "Teleport".
            {
                rb.transform.position = tele2.transform.position; //assigns the transform position of the other teleporter to the transform position of the ball.
                teleport = true; //this boolean variable identifies which teleporter the ball is teleporting to.
            }
            if (other.gameObject.name == "Teleport 2") //tests if the triggered game object is named "Teleport 2".
            {
                rb.transform.position = tele1.transform.position;
                teleport = false; 
            }
        }
        if (other.gameObject.CompareTag("Random")) //tests if the triggered game object has the tag "Random".
        {
            randomSound.Play(); //plays a sound effect once the above statements is true.
            Vector3 newVel = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f)); //initialises a new Vector3 with parameters that use the "Random.Range()" fucntion to a Vector3 variable.
            rb.velocity = newVel; // assigns the recently initialised Vector 3 value to the ball's velocity.
            rb.AddForce(newVel, ForceMode.Impulse); //applies an impluse force to make up for the lost horizontal velocity speed once the velocity had been changed.
        }
        //The following returns the ball to its last position once triggering a game object with the tag "Barrier" as this game obeject is outside the bounds of the golf course.
        if (other.gameObject.CompareTag("Barrier")) //tests if the triggered game Object has the tag "Barrier".
        {
            outOfBoundsSound.Play(); //plays a sound effect once the above statement is true.
            rb.velocity = -rb.velocity - rb.velocity; //subtracts the ball's velocity by itself making it equilivent to zero.
            rb.transform.position = lastPos; //assigns a Vector3 variable that holds transform position parameters to the ball's transform position.
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Antigrav")) //tests if the ball's exiting the triggered object tagged "Antigrav".
        {
            rb.useGravity = true; //changes the ball so its affected by gravity.
        }
        if (other.gameObject.name == "Teleport" && teleport == false) //again tests if the triggered game object is named "Teleport" as well as if teleport is equal to false.
        {
            if(teleporting==true) //tests if the ball has teleported.
            {
                teleporting = false; //once the ball has teleported this is changed to false.
            }
        }
        if (other.gameObject.name == "Teleport 2" && teleport == true) //again tests if the triggered game object is named "Teleport 2" as well as if teleport is equal to true.
        {
            if (teleporting == true)
            {
                teleporting = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Edge")) //tests if the ball had collided with a game object thats tagged "Edge".
        {
            //when the ball collides with the boundaries of the golf course the ball would be rebounded.
            rb.velocity = -rb.velocity; //assigns a negative velocity equivelent to the ball's current velocity to the ball's current velocity.
        }
        if (collision.gameObject.CompareTag("Hole")) //tests if the ball has collided with a game object tagged "Hole".
        {
            winSound.Play(); //plays a sound effect once the above statment is true.
            rb.transform.position = resetPos.position; //assigns a transform position to the ball's transform position reseting the ball to its spawn origin.
            rb.velocity -= rb.velocity; //again subtracts the ball's velocity by itself making it equilivent to zero.
        }
    }
}
