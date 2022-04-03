using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsCollider : MonoBehaviour
{
    private int _counter;
    public bool enterHand;
    
    private void Start()
    {
        enterHand = true;
        _counter = 0;
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hands")
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            _counter++;
            if (_counter == 2)
                enterHand = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hands")
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.white;
            _counter--;
            if (_counter != 2)
                enterHand = false;
        }
    }*/
}
