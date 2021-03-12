using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static int fase;//ゲーム進行度
    public List<GameObject> enemyList = new List<GameObject>();//現ステージ残存エネミー
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.PlayerTargetting();
        switch (fase)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }
    void PlayerTargetting()//クリックしたエネミーをPlayerの攻撃対象とする
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 8;//Enemyだけに衝突

            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, layerMask);

            if (hit.transform.GetComponent<Enemy>())
            {
                Player.target = hit.transform.GetComponent<Enemy>().listNum;
            }
        }
    }
    public void DefaultTargetting()//デフォルトやターゲットしていた敵が死んだ時は左から順々に狙う、敵がいなければfaseを進める
    {
        for(int i = 0;i < this.enemyList.Count; i++)
        {
            if (this.enemyList[i])
            {
                Player.target = i;
                return;
            }
        }

        Player.target = 0;
        fase++;
    }
}
