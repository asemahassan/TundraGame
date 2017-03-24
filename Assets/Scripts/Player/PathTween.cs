using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _3DRunner;

public class PathTween : MonoBehaviour
{
    public GameObject endPoint;
    public float movingSpeed = 5.0f;
    public float lookTime = 0.2f;
    public float lookAhead = 0.001f;
    public iTween.EaseType easeType = iTween.EaseType.linear;
  //  public List<Transform> wayPointsCubes = new List<Transform>();
    Vector3[] wayPointsPos;

    iTweenPath myOwnPath;

    int currPathindex = 1;
    // Use this for initialization
    void Awake()
    {
        
    }

    void Start()
    {
        //Vector3 endPointPos = GameController.Instance.TownObject.transform.position; 
        //myOwnPath.nodes.Add(endPointPos);

        // set way points position according to path nodes
        myOwnPath = gameObject.AddComponent<iTweenPath>() as iTweenPath;
        myOwnPath.pathName = "MyOwnPath";
        myOwnPath.nodes.Clear();
        myOwnPath.nodes.Add(gameObject.transform.position);
        myOwnPath.nodes.Add(endPoint.transform.position);
        iTweenPath.paths.Add(myOwnPath.pathName.ToLower(), myOwnPath);



        wayPointsPos = myOwnPath.nodes.ToArray();
        MoveOnPath();
        iTween.Pause(gameObject);
    }

    public void MoveOnPath()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", wayPointsPos, "speed", movingSpeed, "islocal", false,
           "orienttopath", true, "looktime", lookTime, "lookahead", lookAhead, "easetype", easeType));
    }


    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, wayPointsPos[currPathindex]);
        //Debug.Log("Distance:" + distance);
        if (Input.GetKeyDown(KeyCode.M))
        {
            iTween.Resume(gameObject);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            iTween.Pause(gameObject);
        }

        if (distance < 0.1f)
        {
            iTween.Pause();
            Debug.Log("Itween paused here, player pos: " + this.transform.position);

            if (Input.GetKeyUp(KeyCode.X))
            {
                if (currPathindex <= wayPointsPos.Length)
                {
                    currPathindex++;
                    iTween.Resume();
                    Debug.Log("Itween resume here, player pos: " + this.transform.position);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        //Visual. Not used in movement
        if (wayPointsPos != null && wayPointsPos.Length > 0)
            iTween.DrawPath(wayPointsPos);
    }
}
