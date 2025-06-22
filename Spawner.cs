using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;         // 생성할 적 탱크 프리팹
    public float minX = 5f;                // 생성 구간 시작 (플레이어 기준 오른쪽)
    float maxX;                    // 생성 구간 끝
    public float spawnY = 10f;             // 위에서 Ray를 쏠 높이
    public float rayDistance = 20f;        // 바닥까지의 최대 거리
    public LayerMask groundLayer;          // 땅 레이어

    public void SpawnEnemy()
    {
        // X 좌표의 Max
        maxX = GameManager.instance.border.transform.position.x - 1f ;
        Debug.Log($"탱크 생성 최대 거리: {maxX}");
        // 랜덤 설정
        float randomX = Random.Range(minX, maxX);

        // 위에서 아래로 Raycast로 지형 체크
        Vector2 spawnOrigin = new Vector2(randomX, spawnY);
        RaycastHit2D hit = Physics2D.Raycast(spawnOrigin, Vector2.down, rayDistance, groundLayer);

        if (hit.collider != null) {
            Vector2 spawnPosition = hit.point;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        }
        else {
            Debug.LogWarning("적 탱크 생성 실패: 땅을 찾지 못함");
        }
    }

    void OnDrawGizmosSelected()
    {
        // 디버그용 Ray 시각화
        Gizmos.color = Color.red;
        Vector2 from = new Vector2(minX, spawnY);
        Vector2 to = new Vector2(maxX, spawnY);
        Gizmos.DrawLine(from, to);
    }
}
