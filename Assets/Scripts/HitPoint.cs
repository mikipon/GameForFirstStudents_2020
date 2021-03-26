using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HitPoint : MonoBehaviour
{
    Vector2 maxSize;
    float maxHp;
    Status status;
    // Start is called before the first frame update
    void Start()
    {
        this.maxSize = this.transform.GetChild(0).localScale;
        switch (this.gameObject.tag)
        {
            case "Player"://プレイヤー用なら、オブジェクトのタグを合わせてシーン上に配置する
                this.status = GameObject.Find("Manager").GetComponent<Player>();
                break;
            case "Enemy"://エネミー用なら、オブジェクトのタグを合わせてエネミーの子オブジェクトとしてシーン上に配置する
                this.status = this.transform.parent.GetComponent<Enemy>();
                print(this.transform.GetChild(0).name);
                break;
        }
        this.maxHp = this.status.hp;
    }

    // Update is called once per frame
    void Update()
    {
        this.AjustSize();
        this.AjustPosition();
    }
    void AjustSize()//hpに合わせてオブジェクトサイズを合わせる
    {
        this.transform.GetChild(0).localScale = new Vector2(this.maxSize.x * (this.status.hp / this.maxHp), this.maxSize.y);
    }
    void AjustPosition()//hpに合わせてオブジェクトの位置を合わせる
    {
        this.transform.GetChild(0).localPosition = new Vector2(-(this.maxSize.x - this.transform.GetChild(0).localScale.x) / 2.0f, 0);
    }
}
