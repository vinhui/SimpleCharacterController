using UnityEngine;

public class CharacterControllerMotor : MonoBehaviour
{
    public CharacterController controller;
    public new Camera camera;

    [Header("Speed")]
    public float verticalMoveSpeed = .06f;

    public float horizontalMoveSpeed = .05f;
    public float verticalRotateSpeed = .075f;
    public float horizontalRotateSpeed = .1f;

    [Header("Sprinting")]
    [Tooltip("Multiplier for input when sprinting")]
    public float sprintMultiplier = 2f;

    public float maxSprintTime = 2f;

    [Header("Air Movement")]
    [Tooltip("Multiplier for input when in air")]
    public float airControlMultiplier = .25f;

    public float jumpForce = .2f;

    [Tooltip("Meters per second")]
    public float gravity = 9.8f;

    private bool _isSprinting;

    public bool IsSprinting
    {
        get => _isSprinting;
        set
        {
            if (IsGrounded)
                _isSprinting = value;
            else if (!value)
                _isSprinting = value;
        }
    }

    public bool CanSprint { get; private set; }
    public bool IsGrounded => controller.isGrounded;

    private Vector3 _moveDir;
    public Vector3 MoveDir => _moveDir;
    private float _stamina;
    public float Stamina => _stamina;

    private void Update()
    {
        if (!IsGrounded)
            _moveDir.y -= gravity * Time.deltaTime * Time.deltaTime;

        if (IsSprinting && !CanSprint)
            IsSprinting = false;

        if (IsSprinting)
        {
            _stamina -= Time.deltaTime;
            if (_stamina < 0)
            {
                CanSprint = false;
                _stamina = 0;
            }
        }
        else
        {
            CanSprint = true;
            _stamina += Time.deltaTime;
            if (_stamina > maxSprintTime)
                _stamina = maxSprintTime;
        }

        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            if (_moveDir.y > 0)
                _moveDir.y = 0;
        }

        controller.Move(transform.rotation * _moveDir);
        if (IsGrounded)
        {
            _moveDir.x *= .5f;
            _moveDir.z *= .5f;
        }
        else
        {
            _moveDir.x *= .8f;
            _moveDir.z *= .8f;
        }

        camera.fieldOfView = IsSprinting ? 70 : 60;
    }

    public void DoMove(float horizontalInput, float verticalInput)
    {
        var hSpeed = horizontalInput * horizontalMoveSpeed;
        var vSpeed = verticalInput * verticalMoveSpeed;
        var dir = new Vector3(hSpeed, 0f, vSpeed);

        if (IsSprinting && IsGrounded)
            dir *= sprintMultiplier;
        else if (!IsGrounded)
            dir *= airControlMultiplier;
        _moveDir += dir;
    }

    public void StopMove()
    {
        _moveDir = new Vector3(0, _moveDir.y, 0);
    }

    public void DoRotate(float horizontalInput, float verticalInput)
    {
        if (camera != null)
        {
            Transform camTrans = camera.transform;
            camTrans.Rotate(verticalInput * verticalRotateSpeed, 0, 0, Space.Self);
            Vector3 rot = camTrans.localEulerAngles;
            rot.x = ClampAngle(rot.x, -90, 90);
            rot.y = 0;
            rot.z = 0;
            camera.transform.localRotation = Quaternion.Euler(rot);
        }

        transform.rotation *= Quaternion.Euler(0f, horizontalInput * horizontalRotateSpeed, 0f);
    }

    public void DoJump()
    {
        if (IsGrounded)
            _moveDir.y = jumpForce;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }
}
