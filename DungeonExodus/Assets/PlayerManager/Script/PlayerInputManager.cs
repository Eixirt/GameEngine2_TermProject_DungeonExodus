using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PlayerManager.Script
{
    public class PlayerInputManager : MonoBehaviour, PlayerInput.IPlayerActions
    {
        private PlayerInput _playerInput;
        private Vector2 _inputVector;
        private CharacterController _characterController;
        private const float gravity = -9.8f;
        private float _jumpVelocity = 30.0f;
        [SerializeField] private float _fallingSpeed;
        private bool _isGrounded;
        private bool _isJumping = false;
        [SerializeField] private float _charactorMoveSpeed = 8.0f;
        [SerializeField] private Transform groundCheckPosition;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckRadius = 0.5f;
        [SerializeField] private float _jumpAccel = 0.96f;
        private void Awake() 
        {
            _characterController = GetComponent<CharacterController>();
        }
        private void OnEnable()
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInput();
            }

            _playerInput.Player.Enable();
            _playerInput.Player.SetCallbacks(this);
            
        }

        void Update()
        {
            _isGrounded = Physics.CheckSphere(groundCheckPosition.position, groundCheckRadius, groundLayer );
            
            if (_isGrounded)
            {
                _fallingSpeed = 0f;
            }
            if (_isJumping)
            {
                if (_jumpVelocity <= 1.0f)
                {
                    _isJumping = false;
                    _jumpVelocity = 30.0f;
                }
                _jumpVelocity = _jumpVelocity * _jumpAccel;
                _fallingSpeed = Time.deltaTime * _jumpVelocity  + Time.deltaTime * gravity;
                print("jumping");
            }
            else
            {
                _fallingSpeed = Time.deltaTime * gravity;
            }
            var dir = transform.forward * _inputVector.y + transform.right * _inputVector.x;
            _characterController.Move(dir  * Time.deltaTime * _charactorMoveSpeed);



                
            _characterController.Move(new Vector3(0, _fallingSpeed, 0));
            //print(dir.x);    
            
        }
        private void OnDisable()
        {
          _playerInput.Disable();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>();
            print(_inputVector);
        }

        public void OnMouse(InputAction.CallbackContext context)
        {
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            _isGrounded = false;
            _isJumping = true;
            print("jump!");
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
        }
    }
}
