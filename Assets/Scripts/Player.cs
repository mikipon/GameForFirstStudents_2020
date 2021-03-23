using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int hp = 100;//プレイヤーhp
    public static int target = 0;//攻撃する敵の番号
    public int power;
    Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        this.manager = this.GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.Deth();
    }
    /// <summary>
    /// targetに攻撃（ダメージ計算）
    /// </summary>
    public void Attack()
    {
        Enemy enemy = this.manager.currentEnemies[target].GetComponent<Enemy>();
        enemy.hp -= this.power;
        print(this.manager.currentEnemies[target].name + "のHP: " + enemy.hp);
    }
    void Deth()//hpが0以下で死亡、ゲームオーバー
    {
        if (hp <= 0)
        {
            print("ゲームオーバー");//ゲーム停止処理未実装
        }
    }
}
