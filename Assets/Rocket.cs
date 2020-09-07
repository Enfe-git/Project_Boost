
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    /* TO DO
     *  - Fix lightning
     *  - Add more levels
     *  - Add main menu
     */
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float fwThrust = 100f;

    public enum State { Alive, Dying, Transcending }
    public State state = State.Alive;

    //Finding the Rigidbody
    Rigidbody rigidBody;
    AudioSource audioSource;
    static int currentScene = 0;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
        //todo stop sound on death
    {   if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //print("OK");
                break;

            case "Finish":
                state = State.Transcending;
                currentScene++;
                Invoke("LoadNextScene", 1f);
                break;

            /*case "Fuel":
                print("Fuel");
                Destroy(collision.gameObject);
                break;*/

            default:
                state = State.Dying;
                Invoke("LoadNextScene", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {
        if (currentScene > 2)
        {
            currentScene = 0;
        }
        SceneManager.LoadScene(currentScene);
        
        

    }

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
