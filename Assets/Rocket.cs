
using System;
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
    [SerializeField] float fwThrust = 250f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    public enum State { Alive, Dying, Transcending }
    public State state = State.Alive;
    public bool isInvulnerable = false;

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
    {   if (state == State.Alive) {
            RespondToThrustInput();
            RespondToRotateInput();
            //todo only if debug on
        }
        if (Debug.isDebugBuild) {
            RespondToDebugKeys();
        }
        
    }

    private void RespondToDebugKeys() {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            isInvulnerable = !isInvulnerable;
            print(isInvulnerable);
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
                StartSuccessSequence();
                break;

            /*case "Fuel":
                print("Fuel");
                Destroy(collision.gameObject);
                break;*/

            default:
                if (isInvulnerable) {
                    break;
                }
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        deathParticles.Play();
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadFistScene", 1f);
    }

    private void StartSuccessSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        state = State.Transcending;
        Invoke("LoadNextScene", 1f);
    }

    private void LoadNextScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex > SceneManager.sceneCountInBuildSettings - 1) {
            LoadFirstScene();
        }
        else {
            SceneManager.LoadScene(nextSceneIndex);
        }


    }

    private void LoadFirstScene() {
        SceneManager.LoadScene(0);
    }

    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {

        float thrustSpeed = fwThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustSpeed);
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust(float thrustSpeed)
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustSpeed);
        if (!audioSource.isPlaying)
        { 
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }

    }
}
