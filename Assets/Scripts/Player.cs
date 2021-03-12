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
        this.manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// targetに攻撃（ダメージ計算）
    /// </summary>
    void Attack()
    {
        Enemy enemy = this.manager.enemyList[target].GetComponent<Enemy>();
        enemy.hp -= this.power;
    }
}
