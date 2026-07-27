using UnityEngine;
using System.Collections;

public class SelectableUnit : MonoBehaviour {

    public float selectionSize;
    public bool playerOwned = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void OnBeginSelection()
    {

    }

    public virtual void OnEndSelection()
    {

    }

    public virtual void OnBeginHover()
    {

    }

    public virtual void OnEndHover()
    {

    }
}
