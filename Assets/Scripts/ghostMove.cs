using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostMove : MonoBehaviour
{
    // 储存路径点的transform
    public Transform[] wayPoints;
    public float speed = 0.2f;
    // 路径点的索引
    private int index = 0;
    private Rigidbody2D rg2d;
    private Animator ghostAnimator;

    private void Awake()
    {
        rg2d = GetComponent<Rigidbody2D>();
        ghostAnimator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (transform.position != wayPoints[index].position)
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index].position, speed);
            rg2d.MovePosition(temp);
        }
		else
		{
			index = (index + 1) % wayPoints.Length;
		}
        Vector2 dirVec = (wayPoints[index].position - transform.position).normalized;
        ghostAnimator.SetFloat("dirX",dirVec.x);
        ghostAnimator.SetFloat("dirY",dirVec.y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Pacman")
        {
            Destroy(other.gameObject);
        }
    }
}
