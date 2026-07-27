using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour
{
    public float panSpeed = 100;
    public int zoomSpeed = 4000;
    public int minHeight = 35;
    public int maxHeight = 300;

    public float xBoundry = 0;
    public float zBoundry = 0;

#pragma warning disable 0169
    Vector3 mousePosition;
#pragma warning restore 0169

    public bool lockCamera = false;

    bool isPanning = false;
    Vector3 middleMouseStart;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;

        if (!lockCamera)
        {
            if (!isPanning && Input.GetKeyDown("mouse 2"))
            {
                isPanning = true;
                middleMouseStart = Input.mousePosition;
            }

            if (Input.GetKeyUp("mouse 2"))
            {
                isPanning = false;
            }

            int scrollSpeed = 80;

            //mouse panning in editor behaves weirdly
            if (!Application.isEditor)
            {
                if (Input.mousePosition.x <= 0)
                {
                    transform.Translate(Vector3.left * (Time.deltaTime * scrollSpeed));
                }
                else if (Input.mousePosition.x >= Screen.width)
                {
                    transform.Translate(Vector3.right * (Time.deltaTime * scrollSpeed));
                }

                if (Input.mousePosition.y <= 0)
                {
                    transform.Translate(Vector3.back * (Time.deltaTime * scrollSpeed));
                }
                else if (Input.mousePosition.y >= Screen.height)
                {
                    transform.Translate(Vector3.forward * (Time.deltaTime * scrollSpeed));
                }
            }

            if (isPanning)
            {
                float reductionAmount = 100f;
                transform.position = new Vector3(transform.position.x + -((middleMouseStart.x - Input.mousePosition.x) / reductionAmount), transform.position.y, transform.position.z + -((middleMouseStart.y - Input.mousePosition.y) / reductionAmount));
            }
            else
            {
                float zMovement = Input.GetAxis("Vertical") * panSpeed;
                float xMovement = Input.GetAxis("Horizontal") * panSpeed;
                float scrollWheel = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

                if (transform.position.y <= minHeight && scrollWheel < 0)
                {
                    scrollWheel = 0;
                }

                //Target transform position pushed in the direction of the current inputs
                Vector3 targetPosition = new Vector3(transform.position.x + xMovement, transform.position.y + scrollWheel, transform.position.z + zMovement);

                // Smoothly move the camera towards that target position
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);

                //correct being out of bounds vertically
                if (transform.position.y < minHeight)
                {
                    transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
                }
                if (transform.position.y > maxHeight)
                {
                    transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
                }
            }

            //limit the camera pan to inside the given terrain
            if (xBoundry > 0 && zBoundry > 0)
            {
                if (transform.position.x < -xBoundry)
                {
                    transform.position = new Vector3(-xBoundry, transform.position.y, transform.position.z);
                }
                if (transform.position.x > xBoundry)
                {
                    transform.position = new Vector3(xBoundry, transform.position.y, transform.position.z);
                }

                if (transform.position.z < -zBoundry)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, -zBoundry);
                }
                if (transform.position.z > zBoundry)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, zBoundry);
                }
            }
        }
    }
}