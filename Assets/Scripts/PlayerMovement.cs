using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float start_threshold = 0.02f;
    [SerializeField]
    private float stop_threshold = 0.002f;
    [SerializeField] 
    private GameObject camera_rig;
    [SerializeField]
    private XRController left_hand;
    [SerializeField]
    private XRController right_hand;

    private Rigidbody rigid_body;
    private Vector3 last_difference;

    // Start is called before the first frame update
    void Start()
    {
        rigid_body = GetComponent<Rigidbody>();
        last_difference = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // difference between left and right hand
        Vector3 hand_difference = left_hand.transform.position - right_hand.transform.position;
        // difference between last and current left-right hand differences
        Vector3 difference_in_time = hand_difference - last_difference;

        // moves in camera direction
        if ((Mathf.Abs(difference_in_time[0]) > start_threshold) || (Mathf.Abs(difference_in_time[1]) > start_threshold) || (Mathf.Abs(difference_in_time[2]) > start_threshold))
        {
            rigid_body.transform.position += new Vector3(camera_rig.transform.forward.x, 0, camera_rig.transform.forward.z) * Time.deltaTime * speed;
        }
        else
        {
            // slows down 
            if ((Mathf.Abs(rigid_body.velocity.x) > stop_threshold || (Mathf.Abs(rigid_body.velocity.y) > stop_threshold) || (Mathf.Abs(rigid_body.velocity.z) > stop_threshold)))
            {
                rigid_body.velocity *= 0.5f;

            }
            // stops completely
            else rigid_body.velocity = new Vector3(0, 0, 0);
        }

        last_difference = hand_difference;
    }
}
