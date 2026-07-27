using UnityEngine;

public class ObjectRotator : MonoBehaviour {

    public float xSpeed = 0;
    public float ySpeed = 0;
    public float zSpeed = 0;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime);
	}
}
