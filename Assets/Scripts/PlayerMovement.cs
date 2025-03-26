using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int maxHealth = 10;
    public float movement;
    public float speed;
    public float jumpheight;

    private bool facingRight = true;
    private bool isGround = true;

    public Rigidbody2D rb;
    public Animator animator;

    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");

        if (movement < 0f && facingRight == true)
        {
            transform.eulerAngles = new Vector3(0f, -180, 0f);
            facingRight = false;
        }

        else if (movement > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        };

        if (Input.GetKey(KeyCode.W) && isGround == true)
        {
            Jump();
            animator.SetBool("Jump", true);
            isGround = false;
        }

        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("Walk", 1f);

        }
        else if (movement < .1f)
        {
            animator.SetFloat("Walk", 0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            int randomIndex = Random.Range(0, 4);
            if (randomIndex == 0)
            {
                animator.SetTrigger("Attack1");
            }
            else if (randomIndex == 1)
            {
                animator.SetTrigger("Attack2");
            }
            else
            {
                animator.SetTrigger("Attack3");
            }
        }
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.deltaTime * speed;
    }

    void Jump()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpheight;
        rb.linearVelocity = velocity;
    }

    public void PlayerAttack()
    {
        Collider2D hitinfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if (hitinfo)
        {
            if (hitinfo.GetComponent<Enemy>()!=null)
            {
                hitinfo.GetComponent<Enemy>().EnemyTakeDamage(1);
            }
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            {
                return;
            }
       
        }

    }

    void Die() 
    {
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);  
    }

}
