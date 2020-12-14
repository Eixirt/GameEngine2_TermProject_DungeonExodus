using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using Image = UnityEngine.UI.Image;

namespace Assets.PlayerManager.Script
{
    public class PlayerInputManager : MonoBehaviour, PlayerInput.IPlayerActions
    {
        private PlayerInput _playerInput;
        private Vector2 _inputVector;
        private Vector2 _inputRotateVector;
        public CharacterController _characterController;
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
        [SerializeField] private Animator _animator;
        [SerializeField] private Camera _camera;
        [SerializeField] private float wheelSpeed = 3000.0f;
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject magicEffect;
        [SerializeField] private Image _aim;

        public bool moveFlag = true;
        private void Awake() 
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
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
        public GameObject Picking()
        {
            RaycastHit hit;
            GameObject target = null;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction*5f,out hit))
            {
                target = hit.collider.gameObject;
                
                //hit.transform.GetComponent<MeshRenderer>().material.color = Color.red; // 색 
                //Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 5f);
            }
            return target;
        
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
                Jumping();
                _animator.SetBool("isJump", true);
            }
            else
            {
                _fallingSpeed = Time.deltaTime * gravity;
                _animator.SetBool("isJump", false);
            }
           MovePlayer(); 
           GameObject picked = Picking();
           if (picked == null)
           {
                
           }
           else if (picked.tag == "mirror")
           {
               RotateTarget(picked);
               _aim.color = Color.yellow;
           }
           else if (picked.tag == "block")
           {
               PushBlock(picked);
               _aim.color = Color.yellow;
           }
           else
           {
               _aim.color = Color.gray;
           }
        }
        

        private void MovePlayer()
        {
            if (moveFlag)
            {
                var dir = transform.forward * _inputVector.y + transform.right * _inputVector.x;
                _characterController.Move(dir * Time.deltaTime * _charactorMoveSpeed);
                _characterController.Move(new Vector3(0, _fallingSpeed, 0));
                if(dir.x == 0 && dir.y == 0)
                    _animator.SetBool("isMove", false);
            }
        }

        private void PushBlock(GameObject picked)
        {
            var moveDir = transform.forward * Time.deltaTime * 5f;
            moveDir.y = 0;

            if (Input.GetMouseButton(0))
            {
                picked.transform.Translate(moveDir);
                _animator.SetBool("isAttack", true);
                magicEffect.SetActive(true);
            }
            else
            {
                _animator.SetBool("isAttack", false);
                magicEffect.SetActive(false);
            }
            magicEffect.transform.LookAt(target.transform);
            magicEffect.transform.Rotate(new Vector3(0,-90,0));
        }

        private void Jumping()
        {
            if (_jumpVelocity <= 1.0f)
            {
                _isJumping = false;
                _jumpVelocity = 30.0f;
            }

            _jumpVelocity = _jumpVelocity * _jumpAccel;
            _fallingSpeed = Time.deltaTime * _jumpVelocity + Time.deltaTime * gravity;
        }

        private void OnDisable()
        {
          _playerInput.Disable();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>();
            _animator.SetBool("isMove", true);
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            _isGrounded = false;
            _isJumping = true;
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            _inputRotateVector = context.ReadValue<Vector2>();
        }

        private void RotateTarget(GameObject target)
        { 
          if (_inputRotateVector.x == 0 && _inputRotateVector.y == 0)
          {
              _animator.SetBool("isAttack", false);
              magicEffect.SetActive(false);
          }
          else
          {
              _animator.SetBool("isAttack", true);
              magicEffect.SetActive(true);
          }; 
          target.transform.Rotate(0, _inputRotateVector.x * Time.deltaTime * 100f, 0);
          
          // operation base offset rotate
          magicEffect.transform.LookAt(target.transform);
          magicEffect.transform.Rotate(new Vector3(0,-90,0));
        }
    }
}
