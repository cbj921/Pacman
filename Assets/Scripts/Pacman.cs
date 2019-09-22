using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{

    public float moveSpeed = 0.3f;
    private Rigidbody2D rg2d;
    private Vector2 dest = Vector2.zero;
    private Animator animator;

    private void Awake()
    {
        rg2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dest = transform.position;
    }

    private void FixedUpdate()
    {
        Vector2 movePos = Vector2.MoveTowards(transform.position, dest, moveSpeed);
        rg2d.MovePosition(movePos);

        if (((Vector2)transform.position == dest))
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Valid(Vector2.up))
            {
                dest = (Vector2)transform.position + Vector2.up;
            }
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))&& Valid(Vector2.down))
            {
                dest = (Vector2)transform.position + Vector2.down;
            }
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))&& Valid(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
            }
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))&& Valid(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
            }
            // 设置状态机状态
            Vector2 dirVec = (dest - (Vector2)transform.position).normalized;
            animator.SetFloat("dirX",dirVec.x);
            animator.SetFloat("dirY",dirVec.y);
        }

    }

    // 检测目标点是否可达
    private bool Valid(Vector2 dir)
    {
        Vector2 pos = (Vector2)transform.position;
        // 射线检测，看看目标点到当前位置是否是pacman，如果不是就是墙的碰撞体
        RaycastHit2D hit = Physics2D.Linecast(pos + 1.1f*dir, pos);
        bool canArrive = (hit.collider == GetComponent<Collider2D>());
        //Debug.Log(canArrive);
        //Debug.Log(hit.collider);
        return canArrive;
    }
}
