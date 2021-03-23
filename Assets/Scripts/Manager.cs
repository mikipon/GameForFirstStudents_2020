using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    /// <summary>
    /// ゲームの進行度
    /// </summary>
    public static int fase;
    public GameObject[] enemyList;
    public List<GameObject> currentEnemies = new List<GameObject>();//現ステージ残存エネミー
    StickerManager stManager;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        this.stManager = this.GetComponent<StickerManager>();
        this.player = this.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        this.ObjectSelection();
        switch (fase)
        {
            case 0:
                this.CreateEnemy(this.enemyList[0], this.enemyList[1], this.enemyList[2]);
                stManager.DistributeSticker();
                fase++;
                break;
            case 1:
                break;
        }
    }
    /// <summary>
    /// 敵生成（1～3体用）
    /// </summary>
    /// <param name="1番目の敵（左）"></param>
    /// <param name="2番目の敵（中）"></param>
    /// <param name="3番目の敵（右）"></param>
    void CreateEnemy(GameObject enemy1,GameObject enemy2,GameObject enemy3)
    {
        this.currentEnemies.Clear();

        GameObject[] enemies = { enemy1, enemy2, enemy3 };
        Vector2[] enemyPos = {new Vector2(-10, 1.5f), new Vector2(0, 1.5f), new Vector2(10, 1.5f)};
        for(int i = 0;i < enemies.Length; i++)
        {
            if (enemies[i])
            {
                GameObject obj = Instantiate(enemies[i], enemyPos[i], Quaternion.identity);
                this.currentEnemies.Add(obj);
                obj.GetComponent<Enemy>().number = i;
            }
        }
    }
    void ObjectSelection()//クリックしたオブジェクトに関するアクション
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 8 | 9;//Enemyだけに衝突

            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, layerMask);

            if (hit)
            {
                switch (hit.transform.gameObject.layer)
                {
                    case 8:
                        if (hit.transform.GetComponent<Enemy>())
                        {
                            Player.target = hit.transform.GetComponent<Enemy>().number;//クリックしたエネミーをPlayerの攻撃対象とする
                            print("ターゲット： " + Player.target);
                        }
                        break;
                    case 9:
                        if (hit.transform.GetComponent<Sticker>())
                        {
                            print("選択したスティッカー： " + hit.transform.GetComponent<Sticker>().stickeNumber);
                            if (this.stManager.CompareSticker(hit.transform.GetComponent<Sticker>().stickeNumber))
                            {
                                this.player.Attack();
                                StickerManager.theme = -2;
                                this.stManager.DistributeSticker();
                            }
                        }
                        break;
                }
            }
            
        }
    }
    public void DefaultTargetting()//デフォルトやターゲットしていた敵が死んだ時は左から順々に狙う、敵がいなければfaseを進める
    {
        for(int i = 0;i < this.currentEnemies.Count; i++)
        {
            if (this.currentEnemies[i] != null)
            {
                Player.target = i;
                return;
            }
        }

        Player.target = 0;
        fase++;
        this.stManager.DeletSticker();
        print("finish");
    }
}
