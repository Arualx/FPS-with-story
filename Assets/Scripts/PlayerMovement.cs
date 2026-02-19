using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] private Transform cameraPosition;

    [SerializeField] private CharacterController characterController;

    private Vector3 movement;
    private float gravitation = 1f;

    private float playerWalkingSpeed = 10f;
    private float playerRunningSpeed = 20f;
    private bool isRunning = false;

    [SerializeField] private GameObject jumpSplash;
    private float jumpStrenght = 5f;
    private bool isGrounded;
    private bool jump = false;
    private bool doubleJump = false;

    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject bullet;
    private Vector3 shootDirection;

    private float headSpeed;
    private float headStrenght = 0.1f;
    private float headStartPosition;
    private float headTimer = 0;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private float mouseSensitivity = 1.5f;

    public int health = 100;

    
    /*
    private InputActionMap actionMap;
    private InputAction walkAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction crouchAction;
    */



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        headStartPosition = cameraPosition.localPosition.y;

        instance = this;
    }

    private void Update()
    {
        Movement();
        Rotation();
        HeadMovement();
        Shooting();
    }

    private void Movement()
    {

        //moving
        float verticalVelocity = movement.y;

        Vector3 inputX = transform.right * Input.GetAxis("Horizontal");
        Vector3 inputZ = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontal = inputX + inputZ;

        if (horizontal.magnitude > 1)
        {
            horizontal.Normalize();
        }

        //speed
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            horizontal *= playerRunningSpeed;
            headSpeed = playerRunningSpeed;
            isRunning = true;
        }
        else
        {
            horizontal *= playerWalkingSpeed;
            headSpeed = playerWalkingSpeed;
            isRunning = false;
        }

        //gravitation and jumping
        if (isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = -2f;
            }

            if (Input.GetKeyDown(KeyCode.Space) && jump)
            {
                jump = false;
                doubleJump = true;
                verticalVelocity = jumpStrenght;
                Instantiate(jumpSplash, transform.position, Quaternion.identity);
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Space) && doubleJump)
            {
                doubleJump = false;
                verticalVelocity = jumpStrenght;
                Instantiate(jumpSplash, transform.position, Quaternion.identity);
            }
        }
        verticalVelocity += Physics.gravity.y * gravitation * Time.deltaTime;

        //combine velocities
        movement = horizontal;
        movement.y = verticalVelocity;

        //output
        characterController.Move(movement * Time.deltaTime);

        //check for ground
        isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            jump = true;
        }


    }

    private void Rotation()
    {
        //learn wtf is happening here later
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        yRotation += mouseX;
        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        cameraPosition.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HeadMovement()
    {
        //learn wtf is happening here later
        Vector3 inputXZ = new(movement.x, 0f, movement.z);
        if (inputXZ != Vector3.zero)
        {
            headTimer += Time.deltaTime * headSpeed;
            cameraPosition.transform.localPosition = new Vector3(cameraPosition.transform.localPosition.x, headStartPosition + Mathf.Sin(headTimer) * headStrenght, cameraPosition.transform.localPosition.z);
        } else
        {           
            headTimer = 0f;

            cameraPosition.transform.localPosition = new Vector3(cameraPosition.transform.localPosition.x, Mathf.Lerp(cameraPosition.transform.localPosition.y, headStartPosition, headSpeed * Time.deltaTime), cameraPosition.transform.localPosition.z);
        }
    }

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0) && !isRunning)
        {
            
            if (Physics.Raycast(cameraPosition.transform.position, cameraPosition.transform.forward, out RaycastHit target, 30f))
            {
                shootDirection = target.point;
            }
            else
            {
                shootDirection = cameraPosition.transform.position + cameraPosition.transform.forward * 30f;
            }

            shootDirection = (shootDirection - shootPosition.position).normalized;
            Instantiate(bullet, shootPosition.position, Quaternion.LookRotation(shootDirection));
        }
    }
    

    /*
    private void Walk(ContextCallback ctx)
    {

    }

    private void Sprint(ContextCallback ctx)
    {

    }

    private void Jump(ContextCallback ctx)
    {

    }

    private void Shoot(ContextCallback ctx)
    {

    }

    private void Crouch(ContextCallback ctx)
    {
    }
    */
    /*
    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }
    */



}
