using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneCamera_detector : MonoBehaviour
{
    [SerializeField] public bool detected;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Character")
        {
            detected= true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Character")
        {

            detected = false;
        }
    }
}
