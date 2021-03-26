﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Status
{
    public float attackTime;
    public int number;//ターゲット管理用Listの位置番号
    float attackTimer = 0;
    Manager manager;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        this.manager = GameObject.Find("Manager").GetComponent<Manager>();
        this.player = GameObject.Find("Manager").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        this.Attack();
        this.Deth();
        //print(this.name + "のHP： " + this.hp);
    }
    void Attack()//"attackTime"時間おきにPlayerに"power"の攻撃
    {
        this.attackTimer += Time.deltaTime;
        if (this.attackTimer >= attackTime)
        {
            this.player.hp -= this.power;
            this.attackTimer = 0;
            print("プレイヤーHP： " + this.player.hp);
        }
    }
    void Deth()//hpが0以下で破壊、自動的に他のエネミーへターゲットを変更する
    {
        if(this.hp <= 0)
        {
            this.hp = 0;
            Destroy(this.gameObject);
            this.manager.currentEnemies[number] = null;//破壊する枠を置き換える
            this.manager.DefaultTargetting();
            print(this.name + "死亡");
        }
    }
}
