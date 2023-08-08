using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    //initialises variables
    public GameObject ball;
    public Rigidbody ballRb;
    public float speed;
    public Transform indicator;
    public bool follow = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ballRb.velocity.magnitude == 0.0f) //tests if the ball's velocity is equal to 0.0f which determines whether the shot indicator is moveable or not..
        {
            if (Input.GetKey(KeyCode.W)) //tests if the "W" key has been input.
            {
                transform.RotateAround(ball.transform.position, Vector3.left, speed * Time.deltaTime); //rotates the shot indicator vertically around the ball's position.
            }
            if (Input.GetKey(KeyCode.S)) //tests if the "S" key has been input.
            {
                transform.RotateAround(ball.transform.position, -Vector3.left, speed * Time.deltaTime); //rotates the shot indicator vertically around the ball's position.
            }
            if (indicator.rotation.x != 0.0f)
            {
                if (Input.GetKey(KeyCode.A)) //tests if the "A" key has been input.
                {
                    transform.RotateAround(ball.transform.position, -Vector3.down, speed * Time.deltaTime); //rotates the shot indicator horizontally around the ball's position.
                }
                if (Input.GetKey(KeyCode.D)) //tests if the "D" key has been input.
                {
                    transform.RotateAround(ball.transform.position, Vector3.down, speed * Time.deltaTime); //rotates the shot indicator horizontally around the ball's position.
                }
            }
        }
        if (ballRb.velocity.magnitude > 0.0f) //tests if the ball's velocity is greater than 0.0f.
        {
            indicator.rotation = Quaternion.identity; //resets the rotation of the shot indicator by assigning Quaternion identity to the indicator's rotation.
            Vector3 spawnPoint = new Vector3(ball.transform.position.x, ball.transform.position.y + 2.8f, ball.transform.position.z); //initialises new Vector3 ball position parameters that adjust its y-axis to a Vector3 variable.
            indicator.position = spawnPoint; //the recently initialised Vector3 variable is assigned to the indicators position.
        }
    }
}
