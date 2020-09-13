
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
    {   if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
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
        Invoke("LoadNextScene", 1f);
    }

    private void StartSuccessSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        state = State.Transcending;
        currentScene++;
        Invoke("LoadNextScene", 1f);
    }

    private void LoadNextScene()
    {
        if (currentScene > 2)
        {
            currentScene = 0;
        }
        SceneManager.LoadScene(currentScene);



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
