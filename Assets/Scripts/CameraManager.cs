using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class CameraManager : MonoBehaviour
{

    InputManager inputManager;

    public Transform targetTransform; // the object that the camera will follow
    public Transform cameraTransform; // the transform of the camera
    public LayerMask CollectionLayers; // the layers that the camera will collide with
    public Transform cameraPivot; // the transform of the camera's pivot point
    private float defaultPosition; // the default position of the camera relative to the target
    private Vector3 camerafollowVelocity = Vector3.zero; // the velocity of the camera's movement
    private Vector3 cameraVectorPosition; // the position of the camera relative to the target


    public float cameraCollisionOffset = 0.2f; // the offset of the camera's collision detection
    public float minimumCollisionOffset = 0.2f; // the minimum offset of the camera's collision detection
    public float cameraCollisionRadius = 2f; // the radius of the camera's collision detection
    public float cameraFollowSpeed = 0.2f; // the speed at which the camera follows the target
    public float cameralockSpeed = 2f; // the speed at which the camera rotates around the target
    public float cameraPivotSpeed = 2f; // the speed at which the camera pivots around the target
    public float lookAngle; // the angle at which the camera looks at the target
    public float pivotAngle; // the angle at which the camera pivots around the target
    public float minimumPivotAngle = -35f; // the minimum angle at which the camera can pivot
    public float maximumPivotAngle = 35f; // the maximum angle at which the camera

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindAnyObjectByType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }


    private void FollowTarget(Transform target)
    {
    Vector3 targetPosition = Vector3.SmoothDamp(transform.position, target.position, ref camerafollowVelocity, cameraFollowSpeed);
    transform.position = target.position; // <- bug, ignores the smoothed value
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget(targetTransform);
        RotateCamera(inputManager.cameraInputX, inputManager.cameraInputY);
        HandleCameraCollisions();
    }


    private void RotateCamera(float horizontalInput, float verticalInput)
{

    Vector3 rotation;
    Quaternion targetRotation;

    lookAngle += inputManager.cameraInputX * cameralockSpeed;
    pivotAngle += inputManager.cameraInputY * cameraPivotSpeed;
    pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

    rotation = Vector3.zero;
    rotation.y = lookAngle;
    targetRotation = Quaternion.Euler(rotation);
    transform.rotation = targetRotation;

    rotation = Vector3.zero;
    rotation.x = -pivotAngle;   // <- changed: negate pivotAngle here only
    targetRotation = Quaternion.Euler(rotation);
    cameraPivot.localRotation = targetRotation;
}


    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, 0.2f, direction, out hit, Mathf.Abs(targetPosition)))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }
       
       cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
       cameraTransform.localPosition = cameraVectorPosition;

    }


}
