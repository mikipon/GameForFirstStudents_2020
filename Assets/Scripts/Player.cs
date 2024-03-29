﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Status
{
    public static int target;//攻撃する敵の番号
    public GameObject targettingMark;//Unityから代入
    Manager manager;
    StickerManager stManager;

    public AudioClip sound;
    AudioSource audioSource;

    bool gameover = true;
    // Start is called before the first frame update
    void Start()
    {
        this.manager = GameObject.Find("Manager").GetComponent<Manager>();
        this.stManager = GameObject.Find("Manager").GetComponent<StickerManager>();
        this.audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        this.Targetting();
        this.Deth();
    }
    /// <summary>
    /// targetに攻撃（ダメージ計算）
    /// </summary>
    public void Attack()
    {
        this.audioSource.volume = 0.5f;
        this.audioSource.PlayOneShot(this.sound);

        Enemy enemy = this.manager.currentEnemies[target].GetComponent<Enemy>();
        enemy.hp -= this.power;

        this.StartCoroutine(this.AttackEffect(this.manager.currentEnemies[target]));
        Manager.isStickerClick = false;//攻撃エフェクトが出ている間はステッカーが削除されないのでその時にクリックされないように(音がでる)
        //print(this.manager.currentEnemies[target].name + "のHP: " + enemy.hp);

    }
    void Deth()//hpが0以下で死亡、ゲームオーバー
    {
        if (this.hp <= 0)
        {
            this.hp = 0;
            Manager.action = false;
            if (this.gameover)
            {
                this.StartCoroutine(this.manager.GameOver());
                this.gameover = false;
            }
        }
    }
    void Targetting()//ターゲットマークの調整
    {
        if (Manager.action)
        {
            this.targettingMark.SetActive(true);
            if(this.manager.currentEnemies[target]) this.targettingMark.transform.position = this.manager.currentEnemies[target].transform.position;
        }
        else
        {
            this.targettingMark.SetActive(false);
        }
    }
    IEnumerator AttackEffect(GameObject enemy)//エネミー被弾時のモーション
    {
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
        Vector3 temp = enemy.transform.position;

        for (int i = 0; i < 10; i++)
        {
            enemy.transform.position = temp + new Vector3(0.05f, 0);

            yield return new WaitForSeconds(0.02f);
            enemy.transform.position = temp - new Vector3(0.05f, 0);
            yield return new WaitForSeconds(0.02f);
        }
        enemy.transform.position = temp;
            
        enemy.GetComponent<SpriteRenderer>().color = Color.white;

        //ステッカーのリセットorデリート
        if (manager.CheckLivingEnemy() != -1)
        {
            this.stManager.ResetSticker();//敵が全滅してたらリセットしない(エネミー側がやってくれるDeffautTargetting)
        }
        else
        {
            this.StartCoroutine(this.stManager.DeletSticker());//ステッカーの破壊だけ行い、配置はしない
        }
    }
}
