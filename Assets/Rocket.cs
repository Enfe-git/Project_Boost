using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
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
        ProcessInput();
    }

    // Check for keyboard input
    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        } else if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);

        } else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(Vector3.right);

        } else if (Input.GetKey(KeyCode.S)) {
            transform.Rotate(Vector3.left);
        }
    }
}
