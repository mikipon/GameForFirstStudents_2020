using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextStageAnim : MonoBehaviour
{

    public float fadeSpeed  = 0.02f;    //透明度が変わるスピード
    float red, green, blue, alfa;       //パネルの色と不透明度を管理

    public bool isFadeOut   = false;    //フェードアウト処理の開始や完了を管理する
    public bool isFadeIn = false;       //フェードイン処理の開始や完了を管理

    Image fadeImage;                    //透明度を変更するパネルのイメージ

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GetComponent<Image>();
        isFadeOut = true; //Managerで呼び出したい
        red   = fadeImage.color.r;
        green = fadeImage.color.g;
        blue  = fadeImage.color.b;
        alfa  = fadeImage.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isFadeIn) StartFadeIn();
        if (isFadeOut) StartFadeOut();
    }

    //void StartFadeIn()
    //{
    //    alfa -= fadeSpeed;              //不透明度を少しづつ下げる
    //    SetAlpha();                     //パネルに反映
    //    if(alfa <= 0)                   //完全に透明になったら抜ける
    //    {
    //        isFadeIn = false;
    //        fadeImage.enabled = false; //パネル表示オフ
    //    }
    //}

    void StartFadeOut()
    {
        this.isFadeIn = false;
        fadeImage.enabled = true;   //パネルの表示オン
        alfa += fadeSpeed;          //不透明度を上げる
        SetAlpha();                 //透明度を反映
        if(alfa >= 1)               //完全に不透明になったら抜ける
        {
            isFadeOut = false;
        }
    }

    void SetAlpha()
    {
        Debug.Log("透明度 : " + alfa);
        fadeImage.color = new Color(red, green, blue, alfa);
    }
}
