using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] LayerMask blockLayer;
    public enum DIRECTION_TYPE {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    new Rigidbody2D rigidbody2D;
    float speed;

    Animator animator;

    [SerializeField] AudioClip getItemSE;
    [SerializeField] AudioClip jumpSE;
    [SerializeField] AudioClip stampSE;
    AudioSource audioSource;


    float jumpPower = 400;
    bool isDead = false;

    private void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update(){

        if (isDead) {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(x));

        if (x == 0) {
            direction = DIRECTION_TYPE.STOP;

        } else if (x > 0) {
            direction = DIRECTION_TYPE.RIGHT;

        } else if (x < 0) {
            direction = DIRECTION_TYPE.LEFT;
        }

        if (IsGround()) {
            if (Input.GetKeyDown("space")) {
                Jump();
            } else {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void FixedUpdate() {
        if (isDead) {
            return;
        }
        switch (direction) {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:
                Debug.Log("right");
                speed = 3;
                transform.localScale = new Vector3(1,1,1);
                break;
            case DIRECTION_TYPE.LEFT:
                Debug.Log("left");
                speed = -3;
                transform.localScale = new Vector3(-1,1,1);
                break;
        }
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }

    void Jump() {
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        audioSource.PlayOneShot(jumpSE);
        animator.SetBool("isJumping", true);
    }

    bool IsGround() {
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        Debug.DrawLine(leftStartPoint, endPoint);
        Debug.DrawLine(rightStartPoint, endPoint);

        return Physics2D.Linecast(leftStartPoint, endPoint, blockLayer) || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }

     void OnTriggerEnter2D(Collider2D collision) {
        if (isDead) {
            return;
        }
         if (collision.gameObject.tag == "Trap") {
            PlayerDeath();
         }
         
         if (collision.gameObject.tag == "Finish") {
             if (gameManager.score >= 8) {
                // スコアが一定以上ならば
                Debug.Log("クリア");
                gameManager.GameClear();
             }
         }
         if (collision.gameObject.tag == "Item") {
             audioSource.PlayOneShot(getItemSE);
             collision.gameObject.GetComponent<ItemManager>().GetItem();

         }
         if (collision.gameObject.tag == "Enemy") {
             EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
             if (this.transform.position.y + 0.2f > enemy.transform.position.y) {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                Jump();
                audioSource.PlayOneShot(stampSE);
                enemy.DestroyEnemy();
             } else {
                 PlayerDeath();
             }
         }
    }
    void PlayerDeath() {
        isDead = true;
        rigidbody2D.velocity = new Vector2(0, 0);
        rigidbody2D.AddForce(Vector2.up * jumpPower);
        animator.Play("PlayerDeathAnimation");
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        Destroy(boxCollider2D);
        this.GetComponent<Renderer>().sortingOrder = 100; // 自分で追加した。死亡時にプレイヤーを最前面にする
        gameManager.GameOver();
    }
}
