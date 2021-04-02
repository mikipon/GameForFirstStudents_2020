﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Status
{
    public float attackTime;
    public int number;//ターゲット管理用Listの位置番号
    float attackTimer = 0;
    float fallTimer = 0;
    Manager manager;
    Player player;
    Image panelImage; 
    // Start is called before the first frame update
    void Start()
    {
        this.manager = GameObject.Find("Manager").GetComponent<Manager>();
        this.player = GameObject.Find("Player").GetComponent<Player>();
        this.panelImage = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.action)
        {
            this.Attack();
            this.Deth();
        }
        //print(this.name + "のHP： " + this.hp);
    }
    void Attack()//"attackTime"時間おきにPlayerに"power"の攻撃
    {
        this.attackTimer += Time.deltaTime;
        if (this.attackTimer >= attackTime)
        {
            this.player.hp -= this.power;
            this.attackTimer = 0;
            this.StartCoroutine(this.AttackEffect());
            print("プレイヤーHP： " + this.player.hp);
        }
    }
    void Deth()//hpが0以下で破壊、自動的に他のエネミーへターゲットを変更する
    {
        if (this.hp <= 0)
        {
            this.manager.currentEnemies[this.number] = null;//破壊する枠を置き換える(List配列の収納番号がずれないように)
            this.hp = 0;

            beforeFalling();

            //1.5秒点滅した後に破壊
            this.fallTimer += Time.deltaTime;
            if (this.fallTimer >= 1.5)
            {
                
                Destroy(this.gameObject);
                this.fallTimer = 0;

                this.manager.DefaultTargetting();//他にターゲットするエネミーがいなければステッカーを削除して次のフェーズへ
            }
            //print(this.name + "死亡");
        }
    }

    /// <summary>
    ///敵が点滅する
    /// </summary>
    void beforeFalling()
    {
        float lev = Mathf.Abs(Mathf.Sin(Time.time * 20));
        Color temp = this.gameObject.GetComponent<SpriteRenderer>().color;//ダメージエフェクトが上書きされないように
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, lev);
    }
    IEnumerator AttackEffect()//プレイヤーにダメージを与えた時のエフェクト
    {
        this.panelImage.color = new Color(1, 0, 0, 0);
        for(float i = 0; i <= 0.5f; i+=0.05f)
        {
            this.panelImage.color = new Color(1, 0, 0, i);
            yield return new WaitForSeconds(0.025f);
        }
        for (float i = 0.5f; i >= 0; i -= 0.05f)
        {
            this.panelImage.color = new Color(1, 0, 0, i);
            yield return new WaitForSeconds(0.025f);
        }
        this.panelImage.color = new Color(1, 0, 0, 0);
    }
}
