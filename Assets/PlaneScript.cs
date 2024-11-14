using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaneScript : MonoBehaviour {

    Rigidbody rb;
    

    private void Start() {
        rb = GetComponent<Rigidbody>();
        
    }
    [SerializeField] float throttleIncrement = 1f;
    [SerializeField] float maxThrust = 200f;
    [SerializeField] float responsiveness = 3f;
    [SerializeField] float lift = 135f;

    [SerializeField] TextMeshProUGUI hud;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    

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
            throttle += throttleIncrement;
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            throttle -= throttleIncrement;
        }

        throttle = Mathf.Clamp(throttle, 0f, 100f);

    }

    private void Update() {
        HandleInputs();
        updateHud();
        
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
}





