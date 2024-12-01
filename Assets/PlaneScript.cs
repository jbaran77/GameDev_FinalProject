using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class PlaneScript : MonoBehaviour {

    Rigidbody rb;
    AudioSource audioSource;
    

    private void Start() {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
    }

    [SerializeField] ParticleSystem engine1;

    [SerializeField] ParticleSystem engine2;
    [SerializeField] float throttleIncrement = 1f;
    [SerializeField] float maxThrust = 400f;
    [SerializeField] float responsiveness = 3f;
    [SerializeField] float lift = 135f;

    [SerializeField] TextMeshProUGUI hud;
    [SerializeField] TextMeshProUGUI altWarn;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    private float numHits = 0;

    

    private float responseModifier {
        get {
            return rb.mass / 10f * responsiveness;
        }
    }

    private void HandleInputs(){
        roll = Input.GetAxis("roll");
        pitch = Input.GetAxis("pitch");
        yaw = Input.GetAxis("yaw");


        if (Input.GetKey(KeyCode.Space)) {
            if(!audioSource.isPlaying){
                audioSource.Play();
            }
            engine1.Play();
            engine2.Play();

            throttle += throttleIncrement;
        } else  {
            audioSource.Stop();
            throttle -= throttleIncrement;
            engine1.Stop();
            engine2.Stop();
        }

        throttle = Mathf.Clamp(throttle, 0f, 100f);

    }

    private void Update() {
        HandleInputs();
        updateHud();
        CheckAltitude();
    }

    private void CheckAltitude() {


         if (transform.position.y > 100) {
            altWarn.text = "Too High";
           
            
         }
    }

    private void FixedUpdate() {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifier);

        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
        
    }

    private void updateHud() {
        hud.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed: " + (rb.velocity.magnitude *3.6f).ToString("F0") + " Km/h\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
    }

     private void OnCollisionEnter(Collision other) {
        switch (other.gameObject.tag) {
            case "Mountains":
            numHits ++;
            if(numHits == 3){
                ReloadLevel();
            }
                break;
            default:
                break;
        }
    }


    private void ReloadLevel() {
        Scene sceneManager = SceneManager.GetActiveScene();
        int currentIndex = sceneManager.buildIndex;
        SceneManager.LoadScene(currentIndex);

    }
}





