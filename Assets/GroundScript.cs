using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    //Vars and Objects
    public GameObject player;

    Rigidbody cubeRigidbody;
    private bool isStepped = false;

    // Start is called before the first frame update
    void Start()
    {
        cubeRigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            cubeRigidbody.isKinematic = false;
            cubeRigidbody.useGravity = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(203f/255, 152f/255, 158f/255, 1f);
            if (!isStepped)
            {
                player.GetComponent<PlayerController>().addPath();
            }

            isStepped = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
