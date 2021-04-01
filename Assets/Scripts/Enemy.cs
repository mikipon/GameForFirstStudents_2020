using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Status
{
    public float attackTime;
    public int number;//ターゲット管理用Listの位置番号
    float attackTimer = 0;
    float fallTimer = 0;
    float nullTimer = 0;
    Manager manager;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        this.manager = GameObject.Find("Manager").GetComponent<Manager>();
        this.player = GameObject.Find("Player").GetComponent<Player>();
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
            print("プレイヤーHP： " + this.player.hp);
        }
    }
    void Deth()//hpが0以下で破壊、自動的に他のエネミーへターゲットを変更する
    {
        if (this.hp <= 0)
        {
            this.hp = 0;

            beforeFalling();
            this.manager.currentEnemies[this.number] = null;//破壊する枠を置き換える(List配列の収納番号がずれないように)

            this.fallTimer += Time.deltaTime;
            if (this.fallTimer >= 1.5)
            {
                this.manager.DefaultTargetting();
                Destroy(this.gameObject);
                this.fallTimer = 0;
            }
        }
        //this.manager.currentEnemies[number] = null;//Playerクラスで実行　
        //プレイヤーが攻撃した段階でエネミーの生死が分からないとステッカーリセットが2重に行われる(Manager.cs:125)
        print(this.name + "死亡");
    }

    /// <summary>
    ///敵が点滅して消える 
    /// </summary>
    void beforeFalling()
    {
        float lev = Mathf.Abs(Mathf.Sin(Time.time * 20));
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, lev);
    }
}