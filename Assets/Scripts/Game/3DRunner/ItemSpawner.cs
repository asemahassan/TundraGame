/* ItemSpawner.cs
This class is controlling the spawning of items in 3D runner scene.
*/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace _3DRunner
{
	public class ItemSpawner : MonoBehaviour
	{
		public SpawnItemsType itemsType = SpawnItemsType.None;

		public float spawnRadius = 10f;
		public float MinDistanceBetweenItems = 20f;
		public float VerticalOffset = 2f;
		public float spawnDistance = 25.0f;
		public GameObject Player = null;
		public GameObject Plane = null;

		Terrain mTerrain;
		LinkedList<GameObject> itemsList;

		Vector3 boundsForPlane = Vector3.zero;

		const float spawnTime = 0.7f;
		float elapsedTime = spawnTime;

		void Awake ()
		{
			itemsList = new LinkedList<GameObject> ();
			mTerrain = GameObject.Find ("NewTerrain").GetComponent<Terrain> ();

			//get bounds for plane used for path
			boundsForPlane = Plane.GetComponent<MeshRenderer> ().bounds.size;
		}

		// Update is called once per frame
		void Update ()
		{
			elapsedTime -= Time.deltaTime;
			if (GameController.GameStates == GameState.Runner && elapsedTime <= 0f) {
				UpdateItems ();
				elapsedTime = spawnTime;
			}
		}

		bool DistanceCheck (Vector3 newBerryPosition, Vector3 otherPosition)
		{
			float distX = newBerryPosition.x - otherPosition.x;
			float distZ = newBerryPosition.z - otherPosition.z;
			float squaredDistance = distX * distX + distZ * distZ;

			return squaredDistance > MinDistanceBetweenItems * MinDistanceBetweenItems;
		}

		private Vector3 GetPositionInPlayerViewPort ()
		{
			if (Player == null)
				return Vector3.zero;

			Vector3 playerPos = Player.transform.position;
			Vector3 playerDirection = Player.transform.forward;
			Quaternion playerRotation = Player.transform.rotation;

			return playerPos + playerDirection * spawnDistance;
		}


		private bool UpdateItems ()
		{
			Vector3 _pos = GetPositionInPlayerViewPort ();

			_pos = new Vector3 (_pos.x + (UnityEngine.Random.Range (-boundsForPlane.x / 2, boundsForPlane.x / 2)),
				0,
				_pos.z + (UnityEngine.Random.Range (-boundsForPlane.z / 2, boundsForPlane.z / 2)));
			for (LinkedListNode<GameObject> it = itemsList.First; it != null;) {
				LinkedListNode<GameObject> next = it.Next;
				GameObject item = it.Value;

				if (item == null) {
					itemsList.Remove (it);
				} else if (!DistanceCheck (_pos, item.transform.position)) {
					return false;
				}

				it = next;
			}

			if (itemsType == SpawnItemsType.Berry)
				GenerateBerry (_pos);
			else if (itemsType == SpawnItemsType.Fish)
				GenerateFishPond (_pos);
			else if (itemsType == SpawnItemsType.Question)
				GenerateQuiz (_pos);
			return true;
		}

		/// <summary>
		/// generate a new berry prefab instance of random type at position _pos
		/// </summary>
		/// <param name="_pos"></param>
		private void GenerateBerry (Vector3 _pos)
		{
			_pos.y = mTerrain.SampleHeight (_pos) + VerticalOffset;

			//just adding an empty object to control scale of berries
			GameObject berryParentObject = new GameObject ();
			berryParentObject.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);

			GameObject _prefab = null;

			// apply random textures, always start with 1 as 0 index is for None.
			BerriesType _type;// = (BerriesType) UnityEngine.Random.Range (1, Enum.GetValues (typeof(BerriesType)).Length);
			float berryRatioBlackToRed = 3;

			if (UnityEngine.Random.value > berryRatioBlackToRed / (berryRatioBlackToRed + 1)) {
				_prefab = Resources.Load (AssetsPath._RedBerryPrefabPath) as GameObject;
				_type = BerriesType.Red;
			} else {
				_prefab = Resources.Load (AssetsPath._BlackBerryPrefabPath) as GameObject;
				_type = BerriesType.Black;
			}

			GameObject _berryObj = Instantiate (_prefab, _pos, Quaternion.identity) as GameObject;
			_berryObj.name = _type.ToString () + "Berry";

			_berryObj.transform.parent = berryParentObject.transform;
			berryParentObject.name = _type.ToString () + "Berry";

			//the type of berries need to be set here to update score according the type
			_berryObj.GetComponent<BerryController> ().Type = _type;

			_berryObj.tag = "Item";

			itemsList.AddLast (berryParentObject);
		}

		/// <summary>
		/// generate a new pond prefab instance of random type at position _pos
		/// </summary>
		/// <param name="_pos"></param>
		private void GenerateFishPond (Vector3 _pos)
		{
			_pos.y = mTerrain.SampleHeight (_pos) + VerticalOffset;

			//just adding an empty object to control scale of ponds
			GameObject _prefab = Resources.Load (AssetsPath._FishPondsPrefabPath) as GameObject;

			GameObject _pondObj = Instantiate (_prefab, _pos, Quaternion.identity) as GameObject;
			_pondObj.tag = "Item";
			itemsList.AddLast (_pondObj);
		}

		/// <summary>
		/// generate quiz prefab at random position which will pop up a question when triggered
		/// </summary>
		/// <param name="_pos"></param>
		private void GenerateQuiz (Vector3 _pos)
		{
			_pos.y = mTerrain.SampleHeight (_pos) + VerticalOffset;

			//just adding an empty object to control scale of ponds
			GameObject _prefab = Resources.Load (AssetsPath._QuestionMarkPrefabPath) as GameObject;

			GameObject _quizObj = Instantiate (_prefab, _pos, Quaternion.identity) as GameObject;
			_quizObj.tag = "Item";
			itemsList.AddLast (_quizObj);
		}
	}
}
