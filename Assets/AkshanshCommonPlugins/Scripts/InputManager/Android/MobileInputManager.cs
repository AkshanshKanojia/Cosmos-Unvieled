using UnityEngine;

namespace AkshanshKanojia.Inputs.Mobile
{
    public class MobileInputManager : MonoBehaviour
    {
        //handles all inputs related to touch
        #region PublicFields
        //events
        public delegate void OnTapped(TouchData _data);
        public event OnTapped HasTapped;
        public delegate void OnTouchMove(TouchData _data);
        public event OnTouchMove HasMoved;
        public delegate void OnTouchHeld(TouchData _data);
        public event OnTouchHeld HasHeld;
        public delegate void OnTouchEnd(TouchData _data);
        public event OnTouchEnd HasEnded;

        //eventDataHolder
        [System.Serializable]
        public class TouchData
        {
            public int TouchIndex;
            public int FingerId;
            public Vector3 TouchPosition;
        }

        [HideInInspector] public float mouseDragSenstivity = 2f;

        public bool supportCrossPlatformTesting = true;
        #endregion

        #region SerializedFields
        [SerializeField] bool supportMultiTouch = false;
        #endregion

        #region PrivateFields
        bool isOnPc = false;
        Vector3 tempTouchPos;
        #endregion

        private void Start()
        {
            if (supportCrossPlatformTesting)
            {
#if UNITY_EDITOR
                isOnPc = true;
#elif PLATFORM_ANDROID
                isOnPc = false;
#endif
            }
        }
        private void Update()
        {
            InputHandler();
        }

        //checks for different type of inputs and stores its data while invoking appropriate event
        void InputHandler()
        {
            if (isOnPc)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //tapped
                    TouchData _tempdata = new TouchData()
                    {
                        TouchPosition = Input.mousePosition,
                        TouchIndex = 0
                    };
                    tempTouchPos = Input.mousePosition;
                    HasTapped?.Invoke(_tempdata);
                }

                if (Input.GetMouseButton(0))
                {
                    TouchData _tempdata = new TouchData()
                    {
                        TouchPosition = Input.mousePosition,
                        TouchIndex = 0
                    };
                    if (Vector3.Distance(tempTouchPos, Input.mousePosition) > mouseDragSenstivity)
                    {
                        //Dragged
                        tempTouchPos = Input.mousePosition;
                        HasMoved?.Invoke(_tempdata);
                    }
                    //held
                    HasHeld?.Invoke(_tempdata);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    //released
                    TouchData _tempdata = new TouchData()
                    {
                        TouchPosition = Input.mousePosition,
                        TouchIndex = 0,
                        FingerId = -1
                    };
                    HasEnded?.Invoke(_tempdata);
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    if (supportMultiTouch)
                    {
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            ManageTouchEvents(i);
                        }
                    }
                    else
                    {
                        //input events
                        ManageTouchEvents(0);
                    }
                }
            }
        }

        private void ManageTouchEvents(int _tempIndex)
        {
            TouchData _tempdata = new TouchData()
            {
                TouchPosition = Input.GetTouch(_tempIndex).position,
                TouchIndex = _tempIndex,
                FingerId = Input.GetTouch(_tempIndex).fingerId
            };
            if (Input.GetTouch(_tempIndex).phase == TouchPhase.Began)
            {
                HasTapped?.Invoke(_tempdata);
                //tapped
            }
            if (Input.GetTouch(_tempIndex).phase == TouchPhase.Stationary)
            {
                HasHeld?.Invoke(_tempdata);
                //held
            }
            if (Input.GetTouch(_tempIndex).phase == TouchPhase.Moved)
            {
                HasMoved?.Invoke(_tempdata);
                //dragged
            }
            if (Input.GetTouch(_tempIndex).phase == TouchPhase.Ended)
            {
                HasEnded?.Invoke(_tempdata);
                //stopped
            }
        }
    }
}
