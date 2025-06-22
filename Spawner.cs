using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;         // ������ �� ��ũ ������
    public float minX = 5f;                // ���� ���� ���� (�÷��̾� ���� ������)
    float maxX;                    // ���� ���� ��
    public float spawnY = 10f;             // ������ Ray�� �� ����
    public float rayDistance = 20f;        // �ٴڱ����� �ִ� �Ÿ�
    public LayerMask groundLayer;          // �� ���̾�

    public void SpawnEnemy()
    {
        // X ��ǥ�� Max
        maxX = GameManager.instance.border.transform.position.x - 1f ;
        Debug.Log($"��ũ ���� �ִ� �Ÿ�: {maxX}");
        // ���� ����
        float randomX = Random.Range(minX, maxX);

        // ������ �Ʒ��� Raycast�� ���� üũ
        Vector2 spawnOrigin = new Vector2(randomX, spawnY);
        RaycastHit2D hit = Physics2D.Raycast(spawnOrigin, Vector2.down, rayDistance, groundLayer);

        if (hit.collider != null) {
            Vector2 spawnPosition = hit.point;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        }
        else {
            Debug.LogWarning("�� ��ũ ���� ����: ���� ã�� ����");
        }
    }

    void OnDrawGizmosSelected()
    {
        // ����׿� Ray �ð�ȭ
        Gizmos.color = Color.red;
        Vector2 from = new Vector2(minX, spawnY);
        Vector2 to = new Vector2(maxX, spawnY);
        Gizmos.DrawLine(from, to);
    }
}
