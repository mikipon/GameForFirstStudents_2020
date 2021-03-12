using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float attackTime;
    public int power,hp;//攻撃力,体力
    public int listNum;//ターゲット管理用Listの位置番号
    float attackTimer = 0;
    Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        this.manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.Attack();
    }
    void Attack()//"attackTime"時間おきにPlayerに"power"の攻撃
    {
        this.attackTimer += Time.deltaTime;
        if (this.attackTimer >= attackTime)
        {
            Player.hp -= this.power;
            this.attackTimer = 0;
        }
    }
    void Deth()//hpが0以下で破壊、自動的に他のエネミーへターゲットを変更する
    {
        if(this.hp <= 0)
        {
            Destroy(this.gameObject);
            this.manager.DefaultTargetting();
        }
    }
}
