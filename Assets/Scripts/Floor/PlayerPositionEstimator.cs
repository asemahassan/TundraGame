using UnityEngine;
using System.Collections;
using Floor;

public class PlayerPositionEstimator : MonoBehaviour {

    public FloorControllerCSharp FloorCon;

	// Use this for initialization
	void Start ()
    {
        if (FloorCon == null) FloorCon = GameObject.Find("_GM and Singleton Scripts").GetComponent<FloorControllerCSharp>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        int xRow = (int)FloorCon.FloorInput.x;
        int yCol = (int)FloorCon.FloorInput.y;

        //Update flower textures 
        string nameOfFlower = "FloorFlower_" + xRow.ToString() + "_" + yCol.ToString();
        Debug.Log("FlowerName:" + nameOfFlower);
        GameObject flowerObj = GameObject.Find(nameOfFlower);

        for (int i = 1; i <= 8; ++i)
        {
            if (FloorCon.IsPetalStepped(i))
            {
                RectTransform petal = flowerObj.transform.FindChild("T" + i.ToString()).GetComponent<RectTransform>();

                Vector3 localPos = this.GetComponent<RectTransform>().localPosition;
                float lerpX = Mathf.Lerp(localPos.x, petal.localPosition.x, 0.03f);
                float lerpY = Mathf.Lerp(localPos.x, petal.localPosition.x, 0.03f);

                this.GetComponent<RectTransform>().localPosition = new Vector3(lerpX, lerpY, 0f);
            }
        }
    }
}
