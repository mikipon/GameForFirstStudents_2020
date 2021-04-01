﻿//-9 9
//当たり判定で移行、上まで到達したら移行

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public Sprite[] stickers;
    public GameObject obj;
    public bool bol        = true;

    //ロード時間
    public int loadingTime = 5;

    //クリック回数用
    int click = 0;

    void Start()
    {
        Application.targetFrameRate = 60; //FPSを60に設定 
        //シード値変更
        Random.InitState(System.DateTime.Now.Millisecond);

    }

    /// <summary>
    /// 時間の制御によるステッカーの生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator Corou1()
    {

        Debug.Log("Start");
        int cleateTotal = 100;

        //ステッカーの生成するメソッド
        for (int i=0; i< cleateTotal; i++)
        {
            int xRam   = Random.Range(-9, 10);//生成位置Xをランダム
            int stiRam = Random.Range(0, stickers.Length);//生成するステッカーをランダム

            Instantiate(obj, new Vector2(xRam, 8), Quaternion.identity)
                .GetComponent<SpriteRenderer>().sprite = stickers[stiRam];

            yield return new WaitForSeconds((float)loadingTime / cleateTotal);
            
        }
        //ゲームシーンに切り替え
        SceneManager.LoadScene("Game");

    }

    /// <summary>
    /// スタートボタンイベント
    /// </summary>
    public void OnClick()
    {

        click ++;
        if (click == 1)
            StartCoroutine("Corou1");//コルーチン呼び出し

    }
    
}