using AkshanshKanojia.Inputs.Mobile;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AkshanshKanojia.Inputs.Button
{
    public class ButtonInputManager : MobileInputs, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] enum AvailableObjectTypes { UIObject, Object2d, Object3d }
        [SerializeField] AvailableObjectTypes CurtType;
        [SerializeField] LayerMask RaycastLayer;

        //events
        public delegate void OnButtonTapped(GameObject _obj);
        public delegate void OnButtonHeld(GameObject _obj);
        public delegate void OnButtonTapEnd(GameObject _obj);
        public event OnButtonTapped OnTap;
        public event OnButtonHeld OnHeld;
        public event OnButtonTapEnd OnLeft;

        bool isTapped = false,isHeld = false;

        private void OnDisable()
        {
            isTapped = false;
            isHeld = false;
        }
        #region Inputs

        private void Update()
        {
            if(isHeld)
            {
                OnHeld?.Invoke(gameObject);
            }
        }
        public override void OnTapEnd(MobileInputManager.TouchData _data)
        {
            switch (CurtType)
            {
                case AvailableObjectTypes.Object2d:
                case AvailableObjectTypes.Object3d:
                    if (!isTapped)
                        return;
                    isTapped = false;
                    OnLeft?.Invoke(gameObject);
                    break;
                default:
                    break;
            }
        }

        public override void OnTapMove(MobileInputManager.TouchData _data)
        {
        }

        public override void OnTapped(MobileInputManager.TouchData _data)
        {
            switch (CurtType)
            {
                case AvailableObjectTypes.Object2d:
                    RaycastHit2D _hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(_data.TouchPosition),Mathf.Infinity,RaycastLayer);
                    if (_hit)
                    {
                        if (_hit.collider == GetComponent<Collider2D>())
                        {
                            isTapped = true;
                            OnTap?.Invoke(gameObject);
                        }
                    }
                    break;
                case AvailableObjectTypes.Object3d:
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(_data.TouchPosition), out RaycastHit _tempHit,Mathf.Infinity,RaycastLayer))
                    {
                        if (_tempHit.collider == GetComponent<Collider>())
                        {
                            isTapped = true;
                            OnTap?.Invoke(gameObject);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public override void OnTapStay(MobileInputManager.TouchData _data)
        {
            switch (CurtType)
            {
                case AvailableObjectTypes.Object2d:
                    if (!isTapped)
                        return;
                    RaycastHit2D _hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(_data.TouchPosition),Mathf.Infinity,RaycastLayer);
                    if (_hit)
                    {
                        if (_hit.collider == GetComponent<Collider2D>())
                        {
                            OnHeld?.Invoke(gameObject);
                        }
                        else
                        {
                            isTapped = false;
                        }
                    }
                    else
                    {
                        isTapped = false;
                    }
                    break;
                case AvailableObjectTypes.Object3d:
                    if (!isTapped)
                        return;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(_data.TouchPosition), out RaycastHit _tempHit,Mathf.Infinity,RaycastLayer))
                    {
                        if (_tempHit.collider == GetComponent<Collider>())
                        {
                            OnHeld?.Invoke(gameObject);
                        }
                        else
                        {
                            isTapped = false;
                        }
                    }
                    else
                    {
                        isTapped = false;
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (CurtType == AvailableObjectTypes.UIObject)
            {
                OnLeft?.Invoke(gameObject);
                isHeld = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (CurtType == AvailableObjectTypes.UIObject)
            {
                isHeld = true;
                OnTap?.Invoke(gameObject);
            }
        }
        #endregion
    }
}
