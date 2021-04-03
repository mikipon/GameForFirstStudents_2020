using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextController : MonoBehaviour//オブジェクトの生成と同時にtextContentsを代入することでテキストを流せる
{
    public string[] textContents;//文を収納
    Text text;
    public int textNum,displayCharNum;//何文目//表示されている文字数
    float progressTime;//一文字が出るのにかかる時間
    /// <summary>
    /// イベント等でテキストを止めたい時用
    /// </summary>
    public static bool isNextText = true;
    bool isEvent;

    Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        progressTime = 0;
        displayCharNum = 0;//文のうち何文字表示しているか
        text = this.GetComponent<Text>();
        textNum = 0;
        text.text = textContents[textNum].Substring(0, displayCharNum);
        this.manager = GameObject.Find("Manager").GetComponent<Manager>();
        isEvent = true;
    }

    // Update is called once per frame
    void Update()
    {
        ProgressText();
    }
    //textを表示
    void ProgressText()
    {
        if(progressTime<1)progressTime += Time.deltaTime;
        if (displayCharNum < textContents[textNum].Length)
        {
            if (progressTime > 0.1f)//0.1秒で次の文字表示
            {
                progressTime = 0;
                displayCharNum++;
                text.text = textContents[textNum].Substring(0, displayCharNum);
            }
        }
        else
        {
            if (this.isEvent) this.TextEvent();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if(displayCharNum < textContents[textNum].Length)//ボタンを押したら文を最後まで一気に表示
            {
                displayCharNum = textContents[textNum].Length;
                text.text = textContents[textNum].Substring(0, displayCharNum);
            }
            else if(textNum < textContents.Length - 1)//文字が全部表示されてボタンを押したら次の文が表示される
            {
                if(isNextText) this.NextText();
            }
            else
            {
                this.Finish();
            }
        }
    }
    public void NextText()
    {
        displayCharNum = 0;
        progressTime = 0;
        textNum++;
        text.text = textContents[textNum].Substring(0, displayCharNum);

        this.isEvent = true;
    }
    void Finish()//テキスト終わりに起こすアクション
    {
        if (this.textNum >= this.textContents.Length - 1 && displayCharNum == textContents[textNum].Length)
        {
            switch (Manager.fase)
            {
                case 0:
                    Destroy(transform.parent.parent.gameObject);
                    Manager.fase++;
                    break;
                default:
                    SceneManager.LoadScene("Title");
                    break;
            }
        }
    }
    void TextEvent()//基本的な会話以外のイベント
    {
        if(Manager.fase == 8)
        {
            if(this.textNum == 0)
            {
                this.StartCoroutine(this.manager.GameClear(this.GetComponent<TextController>()));
            }
        }
        this.isEvent = false;
    }
}
