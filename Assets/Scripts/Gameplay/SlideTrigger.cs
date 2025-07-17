using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideTrigger : MonoBehaviour
{
    public bool isAbleToSlide = false;
    public Vector3 contactPointVector;
    public Vector3 contactPointNormal;
    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.CompareTag("Wall"))
        {
            isAbleToSlide = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Wall"))
        {
            isAbleToSlide = false;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, contactPointVector * 1.4f, Color.blue);
        Debug.DrawRay(transform.position, contactPointNormal * 1.4f, Color.cyan);


    }
}
