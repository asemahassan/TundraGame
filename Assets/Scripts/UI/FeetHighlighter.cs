/*Highlight feet when player stands on it
*/
using UnityEngine;
using Floor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

namespace UI
{
    public class FeetHighlighter : MonoBehaviour
    {

        public Button leftFootBtn = null;
        public Button rightFootBtn = null;

        public FloorControllerCSharp FloorCon;

        // Update is called once per frame
        void Update()
        {

#if UNITY_EDITOR
            //to check if the entername dialog panel is not open
            if (Input.GetKeyUp(KeyCode.L))
            {
                leftFootBtn.Select();
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                rightFootBtn.Select();
            }

#endif
            #region ChecksOnSpecificPartOfFlower
            /*We are starting first row item from bottom left (1x1), Item labeling as "FloorFlower_Rows#_Cols#"*/
            if (FloorCon == null)
                FloorCon = GameObject.Find("_GM and Singleton Scripts").GetComponent<FloorControllerCSharp>();

            if (FloorCon.FloorInput.x == 1)
            {
                switch ((int)FloorCon.FloorInput.y)
                {
                    case 2:
                        if (FloorCon.FloorInput.t1 >= FloorCon.StepValue || FloorCon.FloorInput.t2 >= FloorCon.StepValue
                                 || FloorCon.FloorInput.t3 >= FloorCon.StepValue || FloorCon.FloorInput.t4 >= FloorCon.StepValue)
                        {
                            leftFootBtn.Select(); //just highlighting foot
                        }

                        break;
                    case 3:
                        if (FloorCon.FloorInput.t5 >= FloorCon.StepValue || FloorCon.FloorInput.t6 >= FloorCon.StepValue
                            || FloorCon.FloorInput.t7 >= FloorCon.StepValue || FloorCon.FloorInput.t8 >= FloorCon.StepValue)
                        {
                            rightFootBtn.Select(); //just highlighting foot
                        }
                        break;
                }
            }
            #endregion
        }
    }
}