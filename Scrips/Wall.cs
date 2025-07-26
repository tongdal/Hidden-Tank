using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))] // 2D ���� ���, ������ Collider Ÿ�� ���� ����
public class Wall : MonoBehaviour
{
    [Header("���� ����")]
    public float minHeight;
    public float maxHeight;

    private float _originalWidth;
    private float _depth;

    TilemapRenderer tmRenderer;

    private void Awake()
    {
        tmRenderer = GetComponent<TilemapRenderer>();
    }

    public void WallEnable()
    {
        tmRenderer.enabled = true;
    }
    public void WallDisable()
    {
        tmRenderer.enabled = false;
    }
    public void RandomMakeWall(StageConfig cfg)
    {
        // ���� X,Z(����) ��ġ ����
        _originalWidth = transform.position.x;
        _depth = transform.position.z;

        switch (cfg.wallHeight) {
            case (SizeOption.Small):
                minHeight = -6f;
                maxHeight = -3.5f;
                break;
            case (SizeOption.Medium):
                minHeight = -3.5f;
                maxHeight = -1.5f;
                break;
            case (SizeOption.Large):
                minHeight = -1f;
                maxHeight = 1f;
                break;
        }
        // ���� ���� ����
        float randomHeight = Random.Range(minHeight, maxHeight);

        // ��ġ ���� (X, Y, Z)
        transform.position = new Vector3(_originalWidth, randomHeight, _depth);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody != null) {
            StopAllCoroutines();
            WallEnable();
            StartCoroutine(wallHideAfterDelay());
        }
    }
    IEnumerator wallHideAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        WallDisable();
    }
}
