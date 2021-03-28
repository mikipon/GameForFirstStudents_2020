using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimer : MonoBehaviour
{
    Animator animator;
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
        this.enemy = this.transform.parent.GetComponent<Enemy>();
        this.animator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.action)
        {
            this.animator.speed = 60 / this.enemy.attackTime;
        }
    }
}
