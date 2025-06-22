using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove: MonoBehaviour
{
    [SerializeField]
    float speed;
    float inputValue;

    public Transform firePoint;         // ������ �߻�Ǵ� ��ġ
    public Transform gun;               // ������ �ش��ϴ� Transform
    public float rotationSpeed = 50f;   // ������ ȸ�� �ӵ�
    public float maxAngle = 10f;        // ������ ���� ȸ���� �� �ִ� �ִ� ����
    public float minAngle = -80f;       // ������ �Ʒ��� ȸ���� �� �ִ� �ּ� ����

    public float minPower = 5f;          // 최소 발사력
    public float maxPower = 15f;         // 최대 발사력
    public float powerChangeSpeed = 40f; // 파워 증가/감소 속도

    public float currentPower;
    private bool increasing = true;
    private bool charging = false;
    public int curBulletStage;

    public bool isFire;
    
    // 각도 조정
    public float alignSpeed = 10f;      // 각도 변경시 속도
    public float rayLength = 1f;        //Ray 를 쏘는 거리
    public LayerMask groundLayer;

    private CameraController cameraController;

    public GameObject bombA;

    Rigidbody2D body;

    Transform trans;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        cameraController = FindObjectOfType<CameraController>();
    }
        void FixedUpdate()
    {
        body.linearVelocityX  = inputValue * speed;
        AlignToGround();
        GunAngle();
    }
    void Update()
    {
        gauge();
        Fire();
    }
    
    void GunAngle()
    {
        if (isFire || !GameManager.instance.isLive) return;
        // ���� ���� ����
        float rotationInput = 0;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rotationInput = -rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rotationInput = rotationSpeed * Time.deltaTime;
        }

        // ���� ������ �����ͼ� ���ο� ������ ���
        float currentAngle = gun.localEulerAngles.z;
        if (currentAngle > 180) currentAngle -= 360;  // ���� ��ȯ (���� ���� ����)

        // ���ο� ���� ��� �� ���� (���� ���� ����)
        float newAngle = Mathf.Clamp(currentAngle + rotationInput, minAngle, maxAngle);
        gun.localRotation = Quaternion.Euler(0, 0, newAngle);
    }
    
    void gauge()
    {
        if (isFire || !GameManager.instance.isLive) return;

        if (Input.GetKeyDown(KeyCode.Space)) {
            charging = true;
            currentPower = minPower;
            increasing = true;
        }

        if (Input.GetKey(KeyCode.Space) && charging) {
            // 파워 업/다운 반복
            if (increasing) {
                currentPower += powerChangeSpeed * Time.deltaTime;
                if (currentPower >= maxPower) {
                    currentPower = maxPower;
                    increasing = false;
                }
            }
            else {
                currentPower -= powerChangeSpeed * Time.deltaTime;
                if (currentPower <= minPower) {
                    currentPower = minPower;
                    increasing = true;
                }
            }

            // ⚠️ 여기에 UI 게이지 반영 가능 (ex. 슬라이더)
            //Debug.Log("Power: " + currentPower);
        }
    }

    void Fire()
    {
        if (!GameManager.instance.isLive) return;

        if (!isFire && Input.GetKeyUp(KeyCode.Space) && charging) {
            isFire = true;
            charging = false;
            curBulletStage++;
            if (GameManager.instance.curBullet > 0)
                GameManager.instance.curBullet--;
            else {
                GameManager.instance.curGold -= GameManager.instance.bulletCost;
            }


            Transform bomb = GameManager.instance.pool.GetObj(0).transform;
            bomb.position = firePoint.position;
            bomb.rotation = firePoint.rotation;
            //GameObject bomb = Instantiate(bombA, firePoint.position, firePoint.rotation);

            Rigidbody2D rigid = bomb.GetComponent<Rigidbody2D>();
            rigid.linearVelocity = -firePoint.right * currentPower;
            cameraController.SetTarget(bomb.transform);
            //rigid.AddForce(-firePoint.right * fireForce, ForceMode2D.Impulse);
        }
    }

    void OnMove(InputValue value)
    {
        if (!GameManager.instance.isLive) return;
        inputValue = value.Get<Vector2>().x;
    }
    void AlignToGround()
    {
        if (isFire || !GameManager.instance.isLive) return;
        // 중심 아래로 Ray 쏘기
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);

        if (hit.collider != null) {
            // 바닥의 normal 벡터(법선)를 기준으로 기울기를 구함.
            // Atan2(y, x) → 벡터의 방향 각도 구함
            // Rad2Deg → 라디안 값을 도 단위로 변환
            // 90f - → Unity 2D는 기본적으로 오브젝트가 위(90도)를 향하므로 법선 기준 회전 방향을 맞추기 위해 보정하는 값
            float angle = 90f - Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;

            // 부드럽게 회전
            // Y축 기준이 180도 바뀐 기준으며 방금 계산한 각도를 Z축 회전 값으로 설정 (2D 회전은 Z축만 필요함)
            Quaternion targetRotation = Quaternion.Euler(0f, 180f, angle);
            //현재 회전 → 목표 회전으로 부드럽게 회전시킴. Lerp를 쓰면 회전이 뚝뚝 끊기지 않고 부드럽게 변함.
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * alignSpeed);
        }
    }

    void OnDrawGizmos()
    {
        // 디버그용 Ray 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }

    //추가할 부분

}
