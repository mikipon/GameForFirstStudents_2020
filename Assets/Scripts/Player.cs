using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Status
{
    public static int target = 0;//攻撃する敵の番号
    public GameObject[] charList;//Unityから代入（GameOver,GameClear文字）
    public GameObject charTable;//文字をのせる台の当たり判定を操作
    public GameObject targettingMark;//Unityから代入
    Manager manager;

    public AudioClip sound;
    AudioSource audioSource;

    bool gameover = true;
    // Start is called before the first frame update
    void Start()
    {
        this.manager = GameObject.Find("Manager").GetComponent<Manager>();
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
        if (enemy.hp <= 0) this.manager.currentEnemies[enemy.number] = null;//破壊する枠を置き換える(List配列の収納番号がずれないように)
        else print(this.manager.currentEnemies[target].name + "のHP: " + enemy.hp);
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
                Instantiate(this.charList[0]);
                this.charTable.GetComponent<BoxCollider2D>().enabled = true;//直前にオンにしないとステッカーをクリックできない時がある
                this.gameover = false;
            }
        }
        else//プレイヤーが死んでないのにcharTableオブジェクトの当たり判定がオンになっていたらオフにする
        {
            if (this.charTable.GetComponent<BoxCollider2D>().enabled) this.charTable.GetComponent<BoxCollider2D>().enabled = false;
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
}
