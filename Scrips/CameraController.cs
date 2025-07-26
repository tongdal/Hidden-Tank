using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 현재 추적 중인 타겟
    public static float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10); // Z축을 -10으로 설정
    public float fixedYPosition = 0f; // Y축 고정 값


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
