using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float fwThrust = 100f;

    //Finding the Rigidbody
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    // Check for keyboard input
    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        
        float rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }

    private void Thrust()
    {

        float thrustSpeed = fwThrust * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.Space))
        {
            audioSource.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}
