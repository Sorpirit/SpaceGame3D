using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(Rigidbody))]
public class MovmentController : MonoBehaviour
{
    private Rigidbody myRb;
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float shipAcc;
    [SerializeField] private float MaxTurnSpeed;
    [SerializeField] private float turnAcc;

    [SerializeField] private float shipDec;
    [SerializeField] private float turnDec;

    [SerializeField] private float stopMult;

    private float horizInput;
    private float vertInput;

    private float rollInput;
    //private bool isEnginesWorking;
    [SerializeField] private bool isAutoControlingSpeed;
    [SerializeField] private bool isAutoControlingRotation;

    [SerializeField] private bool isInBoostMode;

    private Vector3 mouseOrign;
    [SerializeField] private float snes;

    [SerializeField] private TMP_Text text;

    public enum TurnControllerType{ Mouse, KeyBoard}
    public enum Speeds{
        BackFast = -2,
        Back = -1,
        Stay = 0,

        ForwardLow = 1,
        Forward = 2,
        ForwardFast = 3,

        ForwardSuperFast = 4
    }

    [SerializeField] private TurnControllerType controllerType;
    private Speeds mySpeed = Speeds.Stay;

    private void Awake() {
        
        myRb = GetComponent<Rigidbody>();

    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;        
    }

    private void Update() {

        //horizInput = -1 * Input.GetAxis("Horizontal");
       // vertInput = -1 * Input.GetAxis("Vertical");
        
        
        //isEnginesWorking = Input.GetKey(KeyCode.Space);
        isInBoostMode = Input.GetKey(KeyCode.LeftShift);
        isAutoControlingSpeed = mySpeed == Speeds.Stay;

        if(controllerType == TurnControllerType.KeyBoard){
            horizInput = -1 * Input.GetAxis("Horizontal");
            vertInput = -1 * Input.GetAxis("Vertical");
        }else if( controllerType == TurnControllerType.Mouse){

           
            
            horizInput = snes * Input.GetAxis("Mouse X");
            vertInput = -1 * snes * Input.GetAxis("Mouse Y");
            rollInput = -1 * Input.GetAxis("Horizontal");

            
            horizInput = Mathf.Abs(horizInput) > .1f ? horizInput : 0;
            vertInput = Mathf.Abs(vertInput) > .1f ? vertInput : 0;

        }

        if(Input.GetKeyDown(KeyCode.W) && mySpeed != Speeds.ForwardSuperFast){
            mySpeed = (Speeds)  (int) ++mySpeed;
            text.text = SpeedsToString(mySpeed);
        }else if(Input.GetKeyDown(KeyCode.S) && mySpeed != Speeds.BackFast){
            mySpeed = (Speeds)  (int) --mySpeed;
            text.text = SpeedsToString(mySpeed);
        }


        isAutoControlingRotation = Mathf.Abs(horizInput) < 0.01f && Mathf.Abs(vertInput) < 0.01f && Mathf.Abs(rollInput) < 0.01f;

    }

    private void FixedUpdate() {

        Vector3 rotHor = Vector3.zero;
        Vector3 roll = Vector3.zero;

        if(controllerType == TurnControllerType.KeyBoard){
            rotHor =  transform.forward * horizInput;
        }else if( controllerType == TurnControllerType.Mouse){
            rotHor = transform.up * horizInput;
            roll = transform.forward * rollInput;
        }

        Vector3 rotVer =  transform.right * vertInput;
        //transform.localRotation = Quaternion.Euler(rotHor + rotVer + transform.localRotation.eulerAngles);
        Vector3 rot = rotVer * turnAcc * Time.deltaTime;
        Vector3 rot2 = rotHor * turnAcc * Time.deltaTime;
        Vector3 rot3 = roll * turnAcc * Time.deltaTime;
        
        myRb.angularVelocity += rot + rot2 + rot3;
        myRb.angularVelocity = Vector3.ClampMagnitude(myRb.angularVelocity,MaxTurnSpeed);
        //Debug.Log(roll + "  " + myRb.);
        if(mySpeed != Speeds.Stay) {
            
            Vector3 move = transform.forward * shipAcc * Time.deltaTime;
            myRb.velocity += move * (int) mySpeed;
            myRb.velocity = Vector3.ClampMagnitude(myRb.velocity,MaxSpeed);
            if(isInBoostMode) myRb.velocity *= 5;
        }

        if(isAutoControlingRotation && myRb.angularVelocity != Vector3.zero){
            myRb.angularVelocity +=  myRb.angularVelocity.normalized * -1 * turnDec * Time.deltaTime;

            if(myRb.angularVelocity.magnitude <= .5f) 
                myRb.angularVelocity = Vector3.zero;
        }
        if(isAutoControlingSpeed && myRb.velocity != Vector3.zero){
            myRb.velocity +=  myRb.velocity.normalized * -1 * shipDec * Time.deltaTime;

            if(myRb.velocity.magnitude <= .5f) 
                myRb.velocity = Vector3.zero;
        }
        

    }


    private string SpeedsToString(Speeds s){
        switch (s){
            case Speeds.BackFast:
            return "BF";
            case Speeds.Back:
            return "B";
            case Speeds.Stay:
            return "S";
            case Speeds.ForwardLow:
            return "FL";
            case Speeds.Forward:
            return "F";
            case Speeds.ForwardFast:
            return "FF";
            case Speeds.ForwardSuperFast:
            return "FSF";
            default:
            return "";
        }
    }
    
}
