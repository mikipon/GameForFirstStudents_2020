﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerManager : MonoBehaviour
{
    /// <summary>
    /// お題（スティッカーの番号）
    /// </summary>
    public static int theme;
    public Sprite[] stickerList;//スティッカー画像一覧
    public GameObject stickerObject;//スティッカーの基にするオブジェクト
    List<GameObject> distributedObj = new List<GameObject>();
    Vector2[] positionList = {  new Vector2(-8.0f, -3.5f), new Vector2(-7.3f, -1.7f), new Vector2(-6.0f, -4.0f),
                                new Vector2(-5.5f, -2.5f), new Vector2(-4.5f, -1.5f), new Vector2(-4.0f, -3.5f),
                                new Vector2(-2.5f, -1.2f), new Vector2(-2.0f, -3.7f), new Vector2(-0.9f, -2.4f),
                                new Vector2( 0.5f, -3.9f), new Vector2( 0.8f, -1.6f), new Vector2( 2.0f, -3.0f),
                                new Vector2( 2.7f, -1.3f), new Vector2( 3.5f, -4.0f), new Vector2( 4.0f, -2.0f)};//スティッカーを配置するときの位置
    List<int> distributedSticker = new List<int>();
    bool isDistributeSticker = false;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }
    void Update()
    {
        if (this.isDistributeSticker && this.CheckDestroy())
        {
            this.DistributeSticker();
        }
    }

    /// <summary>
    /// お題のシルエット生成
    /// </summary>
    void CreateTheme()
    {
        theme = Random.Range(0, this.stickerList.Length);
        GameObject obj = Instantiate(this.stickerObject, new Vector2(7,-2), Quaternion.identity);//シルエット生成（位置はここで設定）
        this.distributedObj.Add(obj);
        obj.GetComponent<Sticker>().stickeNumber = -1;
        obj.GetComponent<SpriteRenderer>().sprite = this.stickerList[theme];
        obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);//画像を黒くする
        obj.transform.localScale = new Vector2(0.2f, 0.2f);
    }
    /// <summary>
    /// ステッカーを配置
    /// </summary>
    void DistributeSticker()
    {
        this.CreateTheme();
        this.distributedSticker.Clear();
        int t = Random.Range(0, this.positionList.Length);//いつお題のシルエットを生成するかを決定
        int s;
        for (int i = 0;i < this.positionList.Length; i++)
        {
            if(i == t)
            {
                s = theme;//お題の番号を代入
            }
            else
            {
                do
                {
                    s = Random.Range(0, this.stickerList.Length);//どのステッカーを生成するか決める
                } while (this.distributedSticker.Contains(s) || s == theme);//stickerList[s]が既に使用されていればsを再代入
            }
            
            this.distributedSticker.Add(s);
            GameObject obj = Instantiate(this.stickerObject, this.positionList[i], Quaternion.identity);
            this.distributedObj.Add(obj);
            obj.GetComponent<Sticker>().stickeNumber = s;
            obj.GetComponent<SpriteRenderer>().sprite = this.stickerList[s];
        }

        this.isDistributeSticker = false;//ステッカーの配置は一回だけにしたい
        print("お題： " + theme);
    }
    public void DeletSticker()//ゲーム上のステッカー全削除、落とすだけ（お題含む）
    {
        for (int i = 0; i < this.distributedObj.Count; i++)
        {
            this.distributedObj[i].GetComponent<Rigidbody2D>().gravityScale = 1;
            this.distributedObj[i].GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3, ForceMode2D.Impulse);
            this.distributedObj[i].GetComponent<Sticker>().stickeNumber = -2;
            //Destroy(this.distributedObj[i]);
        }
    }
    bool CheckDestroy()//ステッカーが全て破壊されたかチェックする。されていればListをクリアする。
    {
        for (int i = 0; i < this.distributedObj.Count; i++)
        {
            if (this.distributedObj[i] != null) return false;//削除が完了していなければステッカーを生成しない
        }
        this.distributedObj.Clear();//破壊してnullにするだけじゃなく完全にリセット
        return true;
    }
    public void ResetSticker()//ステッカーを再配置したい時に呼び出すメソッド
    {
        if(!this.CheckDestroy())this.DeletSticker();//ステッカーがない状態から配置するときはいらない
        this.isDistributeSticker = true;
    }
    /// <summary>
    /// 選択画像がお題と一致するか
    /// </summary>
    /// <param name="s"></param>
    /// <returns>一致</returns>不一致
    public bool CompareSticker(int s)
    {
        if (theme == s) return true;
        else return false;
    }
}
