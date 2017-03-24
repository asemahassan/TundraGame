using UnityEngine;
using System.Collections;

public class DialogManager : SingletonBehaviour<DialogManager> {

    public Canvas DialogCanvas;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (DialogCanvas.worldCamera == null) {
            GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
            if(camObj!=null)
                 DialogCanvas.worldCamera = camObj.GetComponent<Camera>();
        }
           
	}
}
