using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PlayerManager.Script
{
    public class PlayerAnimation : MonoBehaviour,PlayerInput.IPlayerActions
    {

        [SerializeField] private Animator _animator;
        // Start is called before the first frame update
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        public void OnMove(InputAction.CallbackContext context)
        {

        }

        public void OnJump(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnRoatate(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnRoatateRight(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnRotateLeft(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnRotationTarget(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
