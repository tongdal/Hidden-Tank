using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Animator anim;
    private Collider2D bombCollider;    // ��ź�� Collider2D
    private Rigidbody2D rb;             // ��ź�� Rigidbody2D

    void Awake()
    {
        anim = GetComponent<Animator>();
        // ��ź�� Collider2D�� ������
        bombCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            // Collider ��Ȱ��ȭ
            bombCollider.enabled = false;
            //
            Vector3 explosionPos = transform.position;
            GameManager.instance.CheckDistances(explosionPos);
            // �߷� ����
            //rb.gravityScale = 0;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            anim.SetTrigger("isExplosion");
            Invoke("Removed", 1f);
            
        }
        else if (collision.gameObject.tag == "Enemy") {
            bombCollider.enabled = false;
            Animator animEnemy = collision.GetComponentInChildren<Animator>(true); // true: 비활성화 포함
            if (animEnemy != null) {
                animEnemy.gameObject.SetActive(true);   // 오브젝트 켜기
                animEnemy.SetTrigger("doDestroy");      // 애니메이션 재생
            }
            
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            anim.SetTrigger("isExplosion");
            Invoke("Removed", 1f);
            Destroy(collision.gameObject, 2f);
            GameManager.instance.BulletWin();
        }
    }

    public void Removed()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
        CameraController.smoothSpeed = 0.25f; // 2 배 빠르게 복귀함.
        FindObjectOfType<CameraController>().SetTarget(FindObjectOfType<PlayerMove>().transform); // ī�޶� Ÿ���� �ٽ� �÷��̾�� ����        
        GameManager.instance.player.isFire = false;
        bombCollider.enabled = true;
        rb.isKinematic = false;
    }
}
