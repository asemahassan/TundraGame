using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UI
{
    
    public class PointsController : MonoBehaviour {

        public void DestroyPointsPopUp()
        {
                Destroy(this.gameObject);
        }
    }

}