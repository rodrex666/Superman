
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR;
public class Flying : MonoBehaviour
{
    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private Transform pointDirection;

    private List<InputDevice> devicesRight = new List<InputDevice>();
    private InputDevice deviceControllerRight;
    private List<InputDevice> devicesLeft = new List<InputDevice>();
    private InputDevice deviceControllerLeft;
    private bool isFlyingWithControllerRight;
    private bool isFlyingWithControllerLeft;
    private HandsCollider _controlHandCollider;
    Vector3 movementFloor = new Vector3(1, 0, 1);

    [SerializeField]
    private float speed = 0.3f;
    Rigidbody rb;
    CapsuleCollider foot;


    void GetDevice()
    {
        InputDeviceCharacteristics _controllerRight = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(_controllerRight, devicesRight);
        deviceControllerRight = devicesRight.FirstOrDefault();
        InputDeviceCharacteristics _controllerLeft = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(_controllerLeft, devicesLeft);
        deviceControllerLeft = devicesLeft.FirstOrDefault();
        _controlHandCollider = pointDirection.GetComponentInParent<HandsCollider>();
        //Debug.Log(deviceControllerRight);
    }
    bool ControlAltitude(Vector3 _vektor)
    {
        if (_vektor.y < 0)
            return false;
        return true;
    }
    private void OnEnable()
    {
        if (!deviceControllerRight.isValid || !deviceControllerLeft.isValid)
        {
            GetDevice();

            //Debug.Log(_controlHandCollider.enterHand);
        }
        foot = GetComponent<CapsuleCollider>();

    }

    private void checkCollisionWithTerrain(Vector3 direction)
    {
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        if (terrainHeight >= transform.position.y + direction.y)
        {
            transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!deviceControllerRight.isValid || !deviceControllerLeft.isValid)
        {
            GetDevice();
        }
        isFlyingWithControllerRight = false;
        isFlyingWithControllerLeft = false;
        //Capturing Grab and Trigger value
        bool grabButton = false;
        float triggerValue = 0.0f;
        InputFeatureUsage<bool> usageR = CommonUsages.gripButton;
        InputFeatureUsage<float> usagetrigger = CommonUsages.trigger;
        //Grab Right Hand and Valid it
        if (deviceControllerRight.TryGetFeatureValue(usageR, out grabButton) && grabButton && !isFlyingWithControllerRight)
        {
            isFlyingWithControllerRight = !isFlyingWithControllerRight;
            isFlyingWithControllerLeft = false;
        }
        //Grab Left Hand and Valid it
        if (deviceControllerLeft.TryGetFeatureValue(usageR, out grabButton) && grabButton && !isFlyingWithControllerLeft)
        {
            isFlyingWithControllerLeft = !isFlyingWithControllerLeft;
            isFlyingWithControllerRight = false;
        }
        //Control Grab and fly
        if (isFlyingWithControllerRight && deviceControllerRight.TryGetFeatureValue(usagetrigger, out triggerValue) && _controlHandCollider.enterHand
            || isFlyingWithControllerLeft && deviceControllerLeft.TryGetFeatureValue(usagetrigger, out triggerValue) && _controlHandCollider.enterHand)
        {
            Vector3 leftDir = leftHand.position - head.position;
            Vector3 rightDir = rightHand.position - head.position;

            Vector3 dir = leftDir + rightDir;
            if (Terrain.activeTerrain.SampleHeight(transform.position) != 0)
            {
                checkCollisionWithTerrain(dir);
            }
           
            {
                if (!ControlAltitude(dir + foot.transform.position))
                {

                    transform.position += Vector3.Scale((dir * triggerValue) * speed, movementFloor);
                }
                else
                {
                    
                    transform.position += (dir * triggerValue) * speed;
                }
            }
        }
    }
}
