using UnityEngine;
using System.Collections;
using System;

public class PathGenerator : MonoBehaviour
{
	// height of camera above ground
	public float heightOffset = -11f;

	// maximum the angle is allowed to change between 2 interpolated nodes
	public float maxAngleChange = 0.27f;

    public GameObject GameEnd;

	// amount the mean of the random values is shifted towards left (negative) or right (positive), resulting in a biased angle 
	// that makes the path describe mainly left turns or mainly right turns respectively
	public float randomOffsetNormal = -0.12f;
	public float randomOffsetMakeTurn = 0.5f;

	public int numPoints = 500;

	private float dist = 14;

	public float MinDist = 6;
	public float MaxDist = 18;

	ArrayList pathPoints;
	Terrain terrain;

	// 2D AABB of all path positions
	Vector2 minCorner;
	Vector2 maxCorner;

	// Use this for initialization
	void Awake ()
    {		 
        minCorner = new Vector2();
		maxCorner = new Vector2();

		pathPoints = new ArrayList();

		//GameObject goCenter = GameObject.CreatePrimitive (PrimitiveType.Cube);
		//goCenter.transform.position = new Vector3 (500, 0, 500);
		//goCenter.name = "centerObj";

		terrain = GameObject.Find ("NewTerrain").GetComponent<Terrain> ();
        Transform lastTransform = this.transform.GetChild(0);
        //Transform endPointTransform = this.transform.GetChild(1);

		Vector3 startDir = new Vector3 (-1, 0, 0);
		Vector3 moveDir = RandomMoveDir (startDir, 0.27f, randomOffsetNormal);

        int i = 1;
        for (; i< numPoints; ++i)
        {
			Transform nextTransform = NextPathPoint(lastTransform, moveDir);

			// name and color path points ascendingly
			nextTransform.name = "_Node_" + String.Format("{0:000}", i);
			Vector3 position = nextTransform.position;

			minCorner.Set( Mathf.Min(position.x, minCorner.x), Mathf.Min(position.z, minCorner.y) );
			maxCorner.Set( Mathf.Max(position.x, maxCorner.x), Mathf.Max(position.z, maxCorner.y) );

			bool inOuterZone = InOuterZone (position);

			nextTransform.gameObject.GetComponent<Renderer>().material.color = new Color (inOuterZone ? 1 : 0, inOuterZone ? 0 : 1, 0);

			lastTransform = nextTransform; // next random move direction
		
			// if path is in outerzone, it will soon leave the terrain, so make a turn until path heas back into the terrain
			if (inOuterZone) 
			{
				// check if move direction is towards or away from terrain center
				// TODO: dynamic terrain center qery
				Vector3 center = new Vector3 (500, 0, 500);
				Vector3 centerDir = center - position;
				centerDir.y = 0;
				centerDir.Normalize ();

				// if facing way from terrain, move back onto it
				if ( Vector3.Dot (moveDir.normalized, centerDir) < 0.7f)
				{
					moveDir = RandomMoveDir (moveDir, 0.27f, randomOffsetMakeTurn);
				} 
				// if already heading towards terrain, just keep moving in roughly the same direction (like we wold normally do)
				else 
				{
					moveDir = RandomMoveDir (moveDir, 0.27f, randomOffsetNormal);
				}
			}
			else
			{
				moveDir = RandomMoveDir (moveDir, 0.27f, randomOffsetNormal);
			}
        }

        while( Vector3.Distance(lastTransform.position, GameEnd.transform.position) > 30 )
        {
            ++i;

            Vector3 targetDir = (GameEnd.transform.position - lastTransform.position).normalized;

            // make a turn if path is not moving towards game end object (tent)
            if(Vector3.Dot(moveDir.normalized, targetDir) < 0.7f)
            {
                moveDir = RandomMoveDir(moveDir, 0.27f, randomOffsetMakeTurn);
            }
            // move roughly towards the tent because we are already headed in that direction
            else
            {
                moveDir = RandomMoveDir(moveDir, 0.27f, randomOffsetNormal);
            }

            Transform nextTransform = NextPathPoint(lastTransform, moveDir);
            nextTransform.name = "_Node_" + String.Format("{0:000}", i);
            nextTransform.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 0);
            lastTransform = nextTransform;
        }

		//SpawnGameEnd (lastTransform.position);

        // move every path point above terrain by sampling its height
        foreach(Transform t in transform)
        {
			Vector3 position = t.position;
			position.y = 0;
			position.y = terrain.SampleHeight(position) + heightOffset;
			t.localPosition = position;
        }
    }

	bool InOuterZone(Vector3 pos)
	{
		Vector3 posNormalized = pos - new Vector3 (500, 0, 500);
		float outerRegionWidth = 300;
		return Mathf.Abs(posNormalized.x) > outerRegionWidth || Mathf.Abs(posNormalized.z) > outerRegionWidth;
	}

	Transform NextPathPoint(Transform lastTransform, Vector3 moveDir)
	{
		Transform t = Instantiate(lastTransform);
		// tint the color of the placeholder object for the path point from red to yellow in order to see where on the path it lies approximately
		t.parent = this.transform;
		t.localPosition = lastTransform.localPosition;
		//dist += RandRange(-1, 1) * 2f;
		//dist = Mathf.Clamp( dist, MinDist, MaxDist );
		t.Translate(moveDir * dist);

		return t;
	}

	Vector3 RandomMoveDir(Vector3 lastDir, float maxChange, float randomOffset)
	{
		float random = UnityEngine.Random.value * 2 - (1 + randomOffset);
		Vector3 newDir = Quaternion.Euler (0, random * maxChange * 90f, 0) * lastDir;

		return newDir;
	}

	float RandRange(float min, float max)
	{
		return min + UnityEngine.Random.value * (max - min);
	}

	void SpawnGameEnd(Vector3 pos)
	{
		pos.y = terrain.SampleHeight(pos) + heightOffset;
		pos.y += 12.6f;

		GameObject _prefab = Resources.Load(AssetsPath._GameEndPrefabPath) as GameObject;
		GameObject _endGameObject = Instantiate (_prefab, pos, Quaternion.identity) as GameObject;
		_endGameObject.transform.parent = transform.parent;
		_endGameObject.name = "GameEnd";

		/*
		GetComponent<SplineController> ().OrientationMode = eOrientationMode.NODE;

		for(int i= 0; i< 100; ++i)
		{
			Transform lastTransform = this.transform.GetChild(0);
			Transform t = Instantiate(lastTransform);
			// tint the color of the placeholder object for the path point from red to yellow in order to see where on the path it lies approximately
			t.parent = this.transform;
			t.position = pos + new Vector3( 20, 20, 0 );
			t.name = "End_Node_" + String.Format("{0:000}", i);
			//dist += RandRange(-1, 1) * 2f;
			//dist = Mathf.Clamp( dist, MinDist, MaxDist );
			t.RotateAround( pos, new Vector3(0, 1, 0), i * 90 );
			t.LookAt(pos);
			t.up = new Vector3 (0, 1, 0);
		}
		*/
	}


}
