using UnityEngine;
using Floor;
using System.Collections.Generic;
using System.Collections;

public class Pond : MonoBehaviour
{
	public FloorControllerCSharp FloorCon;
	public GameObject FloorMappingObj;
	public List<IcePlateBehaviour> IcePlateGOs;

	//TODO: All the class needs to be fixed with proper refined methods.
	public FishBehaviour Fish01;
	public FishBehaviour Fish02;
	public FishBehaviour Fish03;

	private float _stompCooldown;
	private float _stompCooldownMax = 1f;

	/// <summary>
	/// returns true only after cooldown period after stomp has passed
	/// </summary>
	private bool CooldownPassed ()
	{
		return _stompCooldown < 0f;
	}

	/// <summary>
	/// restart cooldown period after stomp
	/// </summary>
	private void resetCooldown ()
	{
		_stompCooldown = _stompCooldownMax;
	}

	private void SetIcePlateStomped (int icePlateIndex)
	{
		IcePlateGOs [icePlateIndex].Stomped ();
		resetCooldown ();
	}

    void OnEnable()
    {
        if (Floor.FloorControllerCSharp.startReadingData == false)
            StartCoroutine(ActivateFloor());
    }

    IEnumerator ActivateFloor()
    {
        yield return new WaitForSeconds(1f);
        //Enable flooor input
        Floor.FloorControllerCSharp.ResetData();
        Floor.FloorControllerCSharp.startReadingData = true;
    }

    // Use this for initialization
    void Start ()
	{
		resetCooldown ();
	}

	// Update is called once per frame
	void Update ()
	{
		_stompCooldown -= Time.deltaTime;

#if UNITY_EDITOR
        // Control the Cracks with the A and D keys. Each pressing of the keys will reduce the HP of the ice by 1.
        if (Input.GetKeyUp (KeyCode.A)) {
			IcePlateGOs [0].Stomped ();
		}
        if (Input.GetKeyUp(KeyCode.D))
        {
            IcePlateGOs[1].Stomped();
        }
#endif
                   
        #region FindFlowerPart
        /*We are starting first row item from bottom left (1x1), Item labeling as "FloorFlower_Rows#_Cols#"*/
        if (FloorCon == null)
			FloorCon = GameObject.Find ("_GM and Singleton Scripts").GetComponent<FloorControllerCSharp> ();

        if (FloorCon != null)
        {
            if (Floor.FloorControllerCSharp.GetStatusOfReadingData())
            {
                if (FloorCon.FloorInput.x == 2)
                {
                    switch ((int)FloorCon.FloorInput.y)
                    {
                        case 2:
                            if (FloorCon.IsPetalStepped(3))
                            {
                                if (FloorCon.IsPetalStomped(3) && CooldownPassed())
                                {
                                    SetIcePlateStomped(0);
                                }
                                if (IcePlateGOs[0].IsBroken && Fish03.ReturnNearestNodeID() == 6)
                                    Fish03.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(4))
                            {
                                if (FloorCon.IsPetalStomped(4) && CooldownPassed())
                                {
                                    SetIcePlateStomped(0);
                                }
                                if (IcePlateGOs[0].IsBroken && Fish01.ReturnNearestNodeID() == 3)
                                    Fish01.Stomped();
                                if (IcePlateGOs[0].IsBroken && Fish03.ReturnNearestNodeID() == 7)
                                    Fish03.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(5))
                            {
                                if (FloorCon.IsPetalStomped(5) && CooldownPassed())
                                {
                                    SetIcePlateStomped(0);
                                }
                                if (IcePlateGOs[0].IsBroken && Fish01.ReturnNearestNodeID() == 2)
                                    Fish01.Stomped();
                                if (IcePlateGOs[0].IsBroken && Fish03.ReturnNearestNodeID() == 8)
                                    Fish03.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(6))
                            {
                                if (FloorCon.IsPetalStomped(6) && CooldownPassed())
                                {
                                    SetIcePlateStomped(0);
                                }
                                if (IcePlateGOs[0].IsBroken && Fish01.ReturnNearestNodeID() == 1)
                                    Fish01.Stomped();
                                if (IcePlateGOs[0].IsBroken && Fish03.ReturnNearestNodeID() == 9)
                                    Fish03.Stomped();
                            }
                            break;
                        case 3:
                            if (FloorCon.IsPetalStepped(3))
                            {
                                if (FloorCon.IsPetalStomped(3) && CooldownPassed())
                                {
                                    SetIcePlateStomped(1);
                                }
                                if (IcePlateGOs[1].IsBroken && Fish02.ReturnNearestNodeID() == 1)
                                    Fish02.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(4))
                            {
                                if (FloorCon.IsPetalStomped(4) && CooldownPassed())
                                {
                                    SetIcePlateStomped(1);
                                }
                                if (IcePlateGOs[1].IsBroken && Fish02.ReturnNearestNodeID() == 2)
                                    Fish02.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(5))
                            {
                                if (FloorCon.IsPetalStomped(5) && CooldownPassed())
                                {
                                    SetIcePlateStomped(1);
                                }
                                if (IcePlateGOs[1].IsBroken && Fish02.ReturnNearestNodeID() == 3)
                                    Fish02.Stomped();
                                if (IcePlateGOs[1].IsBroken && Fish03.ReturnNearestNodeID() == 4)
                                    Fish03.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(6))
                            {
                                if (FloorCon.IsPetalStomped(6) && CooldownPassed())
                                {
                                    SetIcePlateStomped(1);
                                }
                                if (IcePlateGOs[1].IsBroken && Fish03.ReturnNearestNodeID() == 5)
                                    Fish03.Stomped();
                            }
                            break;
                    }
                }
                //X = 1
                else
                {
                    switch ((int)FloorCon.FloorInput.y)
                    {
                        case 2:
                            if (FloorCon.IsPetalStepped(1))
                            {
                                if (FloorCon.IsPetalStomped(1) && CooldownPassed())
                                {
                                    SetIcePlateStomped(0);
                                }
                                if (IcePlateGOs[0].IsBroken && Fish02.ReturnNearestNodeID() == 7)
                                    Fish02.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(2))
                            {
                                if (FloorCon.IsPetalStomped(2) && CooldownPassed())
                                {
                                    SetIcePlateStomped(0);
                                }
                                if (IcePlateGOs[0].IsBroken && Fish02.ReturnNearestNodeID() == 6)
                                    Fish02.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(7))
                            {
                                if (FloorCon.IsPetalStomped(7) && CooldownPassed())
                                {
                                    SetIcePlateStomped(0);
                                }
                                if (IcePlateGOs[0].IsBroken && Fish02.ReturnNearestNodeID() == 9)
                                    Fish02.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(8))
                            {
                                if (FloorCon.IsPetalStomped(8) && CooldownPassed())
                                {
                                    SetIcePlateStomped(0);
                                }
                                if (IcePlateGOs[0].IsBroken && Fish02.ReturnNearestNodeID() == 8)
                                    Fish02.Stomped();
                            }
                            break;
                        case 3:
                            if (FloorCon.IsPetalStepped(1))
                            {
                                if (FloorCon.IsPetalStomped(1) && CooldownPassed())
                                {
                                    SetIcePlateStomped(1);
                                }
                                if (IcePlateGOs[1].IsBroken && Fish01.ReturnNearestNodeID() == 6)
                                    Fish01.Stomped();
                                if (IcePlateGOs[1].IsBroken && Fish03.ReturnNearestNodeID() == 2)
                                    Fish03.Stomped();

                            }
                            if (FloorCon.IsPetalStepped(2))
                            {
                                if (FloorCon.IsPetalStomped(2) && CooldownPassed())
                                {
                                    SetIcePlateStomped(1);
                                }
                                if (IcePlateGOs[1].IsBroken && Fish01.ReturnNearestNodeID() == 7)
                                    Fish01.Stomped();
                                if (IcePlateGOs[1].IsBroken && Fish03.ReturnNearestNodeID() == 1)
                                    Fish03.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(7))
                            {
                                if (FloorCon.IsPetalStomped(7) && CooldownPassed())
                                {
                                    SetIcePlateStomped(1);
                                }
                                if (IcePlateGOs[1].IsBroken && Fish01.ReturnNearestNodeID() == 4)
                                    Fish01.Stomped();
                                if (IcePlateGOs[1].IsBroken && Fish02.ReturnNearestNodeID() == 5)
                                    Fish02.Stomped();
                            }
                            if (FloorCon.IsPetalStepped(8))
                            {
                                if (FloorCon.IsPetalStomped(8) && CooldownPassed())
                                {
                                    SetIcePlateStomped(1);
                                }
                                if (IcePlateGOs[1].IsBroken && Fish01.ReturnNearestNodeID() == 5)
                                    Fish01.Stomped();
                                if (IcePlateGOs[1].IsBroken && Fish02.ReturnNearestNodeID() == 4)
                                    Fish02.Stomped();
                                if (IcePlateGOs[1].IsBroken && Fish03.ReturnNearestNodeID() == 3)
                                    Fish03.Stomped();
                            }
                            break;
                    }
                }
            }
        }
		#endregion
	}
}
