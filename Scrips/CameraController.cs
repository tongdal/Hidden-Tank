using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // ���� ���� ���� Ÿ��
    public static float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10); // Z���� -10���� ����
    public float fixedYPosition = 0f; // Y�� ���� ��


    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, fixedYPosition + offset.y, offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
