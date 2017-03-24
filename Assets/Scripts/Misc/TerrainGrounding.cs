using UnityEngine;
using System.Collections;

/// <summary>
/// this script moves any game object it is applied to VerticalOffset units above the terrain. And nothing more
/// </summary>
public class TerrainGrounding : MonoBehaviour {

    public float VerticalOffset = 0f;

    Terrain terrain;

    void Awake()
    {
        terrain = GameObject.Find("NewTerrain").GetComponent<Terrain>();
    }

	void LateUpdate()
    {
		Vector3 pos = transform.position;
		pos.y = terrain.SampleHeight(pos) + VerticalOffset;
		transform.position = pos;
    }
}
