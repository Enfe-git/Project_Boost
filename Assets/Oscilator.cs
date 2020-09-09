using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    //todo remove from inspector later
    [Range(0,1)] [SerializeField] float movementFactor; //0 for not moved, 1 for fully moved.

    Vector3 startingPos;


    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        //set movement factor
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
