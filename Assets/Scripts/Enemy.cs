using JetBrains.Annotations;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 3;
    private enum Direction { left, right }
    public Animator animator;
    public Transform Player;
    private bool PlayerInRange = false;
    public float AttackRange = 10f;
    public float WalkSpeed = 1.5f;
    public float chaseSpeed = 2.5f;
    public float retrieveDistance = 2.5f;


    public Transform detectPoint;
    public float distance;
    public LayerMask detectLayer;
    //private bool facingLeft = true;
    private Direction currentDirection;
    public Transform attackPoint;
    public float attackRadius;
    public LayerMask attackLayer;





    // Update is called once per frame
    void Update()
    {
        if (maxHealth <= 0) 
        {
            Die();
        }
        if (Vector2.Distance(transform.position, Player.position) <= AttackRange)
        {
            PlayerInRange = true;
        }
        else
        {
            PlayerInRange = false;
        }
        if (PlayerInRange)
        {
            if (transform.position.x < Player.position.x && currentDirection == Direction.left)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                currentDirection = Direction.right;
            }
            else if (transform.position.x > Player.position.x && currentDirection == Direction.right)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                currentDirection = Direction.left;
            }


            if (Vector2.Distance(transform.position, Player.position) > retrieveDistance)
            {
                animator.SetBool("Attack", false);
                transform.position = Vector2.MoveTowards(transform.position, Player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack", true);
            }

        }
        else
        {
            transform.Translate(Vector2.left * WalkSpeed * Time.deltaTime);

            RaycastHit2D hit = Physics2D.Raycast(detectPoint.position, Vector2.down,
                               distance, detectLayer);

            if (hit == false)
            {
                if (currentDirection == Direction.left)

                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    currentDirection = Direction.right;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    currentDirection = Direction.left;
                }
            }
        }
    }


    public void Attack()
    {
        Collider2D collisioninfo = Physics2D.OverlapCircle(attackPoint.position,
                                   attackRadius, attackLayer);
        if (collisioninfo)
        {
            Debug.Log(collisioninfo.gameObject.name + "takes damage");
        }
    }

    public void EnemyTakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
    }

    void Die() 
    {
        Destroy(this.gameObject);
    }



    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position, Vector2.down * distance);

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }



    }

}
