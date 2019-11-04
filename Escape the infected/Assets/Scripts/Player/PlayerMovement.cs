using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private PhotonView pv;
    public float speed = 4.0f;

    public float rotationSpeed = 100;
    private CharacterController _charCont;



    [SerializeField]
    private float m_WalkSpeed = 6.0f;
    private float m_RunSpeed = 4.0f;
    public bool m_LimitDiagonalSpeed = true;
    private bool m_ToggleRun = false;
    public PhotonView photonView;
    private float m_JumpSpeed = 10.0f;
    private float m_Gravity = 20.0f;
    private float m_FallingThreshold = 10.0f;
    private bool m_SlideWhenOverSlopeLimit = false;
    private bool m_SlideOnTaggedObjects = false;
    private float m_SlideSpeed = 12.0f;
    private bool m_AirControl = true;
    private float m_AntiBumpFactor = .75f;
    private int m_AntiBunnyHopFactor = 1;
    private Vector3 m_MoveDirection = Vector3.zero;
    private bool m_Grounded = false;
    private CharacterController m_Controller;
    private Transform m_Transform;
    private float m_Speed;
    private RaycastHit m_Hit;
    private float m_FallStartLevel;
    private bool m_Falling;
    private float m_SlideLimit;
    private float m_RayDistance;
    private Vector3 m_ContactPoint;
    private bool m_PlayerControl = false;
    private int m_JumpTimer;


    private void Update()
    {
        if (m_ToggleRun && m_Grounded && Input.GetButtonDown("Run"))
        {
            m_Speed = (m_Speed == m_WalkSpeed ? m_RunSpeed : m_WalkSpeed);
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        m_ContactPoint = hit.point;
    }

    private void OnFell(float fallDistance)
    {
        print("Ouch! Fell " + fallDistance + " units!");
    }


    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Controller = GetComponent<CharacterController>();

        m_Speed = m_WalkSpeed;
        m_RayDistance = m_Controller.height * .5f + m_Controller.radius;
        m_SlideLimit = m_Controller.slopeLimit - .1f;
        m_JumpTimer = m_AntiBunnyHopFactor;
        pv = GetComponent<PhotonView>();
        _charCont = GetComponent<CharacterController>();
        v = GetComponent<PhotonView>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        Init(transform, cam.transform);
        if (!v.IsMine)
        {
            cam.enabled = false;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (pv.IsMine)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");
            float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && m_LimitDiagonalSpeed) ? .7071f : 1.0f;

            if (m_Grounded)
            {
                bool sliding = false;
                if (Physics.Raycast(m_Transform.position, -Vector3.up, out m_Hit, m_RayDistance))
                {
                    if (Vector3.Angle(m_Hit.normal, Vector3.up) > m_SlideLimit)
                    {
                        sliding = true;
                    }
                }

                else
                {
                    Physics.Raycast(m_ContactPoint + Vector3.up, -Vector3.up, out m_Hit);
                    if (Vector3.Angle(m_Hit.normal, Vector3.up) > m_SlideLimit)
                    {
                        sliding = true;
                    }
                }

                if (m_Falling)
                {
                    m_Falling = false;
                    if (m_Transform.position.y < m_FallStartLevel - m_FallingThreshold)
                    {
                        OnFell(m_FallStartLevel - m_Transform.position.y);
                    }
                }

                if (!m_ToggleRun)
                {
                    m_Speed = Input.GetKey(KeyCode.LeftShift) ? m_RunSpeed : m_WalkSpeed;
                }

                if ((sliding && m_SlideWhenOverSlopeLimit) || (m_SlideOnTaggedObjects && m_Hit.collider.tag == "Slide"))
                {
                    Vector3 hitNormal = m_Hit.normal;
                    m_MoveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                    Vector3.OrthoNormalize(ref hitNormal, ref m_MoveDirection);
                    m_MoveDirection *= m_SlideSpeed;
                    m_PlayerControl = false;
                }

                else
                {
                    m_MoveDirection = new Vector3(inputX * inputModifyFactor, -m_AntiBumpFactor, inputY * inputModifyFactor);
                    m_MoveDirection = m_Transform.TransformDirection(m_MoveDirection) * m_Speed;
                    m_PlayerControl = true;
                }

                if (!Input.GetButton("Jump"))
                {
                    m_JumpTimer++;
                }
                else if (m_JumpTimer >= m_AntiBunnyHopFactor)
                {
                    m_MoveDirection.y = m_JumpSpeed;
                    m_JumpTimer = 0;
                }
            }
            else
            {
                if (!m_Falling)
                {
                    m_Falling = true;
                    m_FallStartLevel = m_Transform.position.y;
                }

                if (m_AirControl && m_PlayerControl)
                {
                    m_MoveDirection.x = inputX * m_Speed * inputModifyFactor;
                    m_MoveDirection.z = inputY * m_Speed * inputModifyFactor;
                    m_MoveDirection = m_Transform.TransformDirection(m_MoveDirection);
                }
            }

            m_MoveDirection.y -= m_Gravity * Time.deltaTime;

            m_Grounded = (m_Controller.Move(m_MoveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
        }
        if (pv.IsMine)
        {
            RotateView();
        }
    }



    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;
    PhotonView v;
    
    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    public Camera cam;

    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;

    public void Init(Transform character, Transform camera)
    {
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;
    }


    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {

        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    private void RotateView()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        float oldYRotation = transform.eulerAngles.y;

        LookRotation(transform, cam.transform);

    }


    void BasicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0, mouseX, 0));
    }

}
