using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostMove : MonoBehaviour
{
    // 储存路径点的transform
    public GameObject[] wayPointsGos;
    public float speed = 0.2f;
    public int firstWayIndex = 0;
    // 路径点的索引
    private int index = 0;
    private Rigidbody2D rg2d;
    private Animator ghostAnimator;
    private List<Vector3> wayPoints = new List<Vector3>();
    private Vector2 startPos;

    private void Awake()
    {
        rg2d = GetComponent<Rigidbody2D>();
        ghostAnimator = GetComponent<Animator>();
        startPos = transform.position;
        // 初始默认是用firstWayIndex的路径
        foreach(Transform t in wayPointsGos[firstWayIndex].transform) // 遍历该点下的子物体
        {
            wayPoints.Add(t.position);
        }
    }
    private void FixedUpdate()
    {
        if (transform.position != wayPoints[index])
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index], speed);
            rg2d.MovePosition(temp);
        }
		else
		{
			index = (index + 1) % wayPoints.Count;
            // 一条路径完成后
            if(index == 0)
            {
                GetRandomWay();
            }
		}
        Vector2 dirVec = (wayPoints[index] - transform.position).normalized;
        ghostAnimator.SetFloat("dirX",dirVec.x);
        ghostAnimator.SetFloat("dirY",dirVec.y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Pacman")
        {
            if(GameControl.Instance.isSuperPacman)
            {
                //TODO: 将幽灵的位置回到原点，重新巡逻
                transform.position = startPos;
                GetRandomWay();
            }
            else
            {
                Destroy(other.gameObject);
            }
            
        }
    }

    private void GetRandomWay()
    {
        // 清空wayPoints的List集合中的路径点 
        wayPoints.Clear();
        index = 0;
        // 得到随机路径
        GameObject randomWay = wayPointsGos[Random.Range(0,wayPointsGos.Length)];
        foreach(Transform t in randomWay.transform) // 遍历该点下的子物体
        {
            wayPoints.Add(t.position);
        }
    }
}
