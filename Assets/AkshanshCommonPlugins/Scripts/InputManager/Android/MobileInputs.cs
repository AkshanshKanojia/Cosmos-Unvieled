using UnityEngine;
namespace AkshanshKanojia.Inputs.Mobile
{
    public abstract class MobileInputs:MonoBehaviour
    {
        public virtual void Start()
        {
            MobileInputManager mang = FindObjectOfType<MobileInputManager>();
            if(mang)
            {
                mang.HasTapped += OnTapped;
                mang.HasMoved += OnTapMove;
                mang.HasHeld += OnTapStay;
                mang.HasEnded += OnTapEnd;
            }
            else
            {
                Debug.LogError("Can not find MobileInputManager class!");
            }
        }
        public abstract void OnTapped(MobileInputManager.TouchData _data);
        public abstract void OnTapMove(MobileInputManager.TouchData _data);
        public abstract void OnTapStay(MobileInputManager.TouchData _data);
        public abstract void OnTapEnd(MobileInputManager.TouchData _data);
    }
}
