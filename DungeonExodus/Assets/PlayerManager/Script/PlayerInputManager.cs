using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.VFX;

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
        [SerializeField] private Animator _animator;
        [SerializeField] private Camera _camera;
        [SerializeField] private float wheelSpeed = 3000.0f;
        [SerializeField] private VisualEffect magic;
        [SerializeField] private GameObject target;
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
                Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 5f);
            }
            return target;
        
        }
        void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel") * 1000f * Time.deltaTime;
            //print("wheelSpeed : " + wheelSpeed + "mouseInput" + Input.GetAxis("Mouse ScrollWheel"));
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
               // print("jumping");
            }
            else
            {
                _fallingSpeed = Time.deltaTime * gravity;
            }
            var dir = transform.forward * _inputVector.y + transform.right * _inputVector.x;
            _characterController.Move(dir  * Time.deltaTime * _charactorMoveSpeed);
            _characterController.Move(new Vector3(0, _fallingSpeed, 0));
            
            
           var picked = Picking();
           if (picked == null)
           {
     
           }
           else if(picked.tag == "mirror")
           {
               magic.Play();
               if (scroll != 0)
               {
                   _animator.Play("Attack02Start 0");
                   picked.transform.Rotate(0, scroll, 0);
                   print(scroll);
               }
           }
           else if (picked.tag == "block")
           {
               magic.Play();
               var moveDir = transform.forward*Time.deltaTime*5f;
               moveDir.y = 0;
               if(Input.GetMouseButton(0)) picked.transform.Translate(moveDir);
           }
        }
        private void OnDisable()
        {
          _playerInput.Disable();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            _inputVector = context.ReadValue<Vector2>();
            _animator.Play("BattleRunForward");
        }
        

        public void OnJump(InputAction.CallbackContext context)
        {
            _isGrounded = false;
            _isJumping = true;
            _animator.Play("JumpStart");
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            target = Picking();
            var temp = context.ReadValue<Vector2>();
            if (target.tag == "mirror")
            {
                _animator.Play("Attack02Start 0");
                target.transform.Rotate(0, temp.x * Time.deltaTime * 100f, 0);
            }
        }

        // print(scroll);
    }
}
