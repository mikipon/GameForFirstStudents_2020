using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour//オブジェクトの生成と同時にtextContentsを代入することでテキストを流せる
{
    public string[] textContents;//文を収納
    Text text;
    public int textNum,displayCharNum;//何文目//表示されている文字数
    float progressTime;//一文字が出るのにかかる時間
    // Start is called before the first frame update
    void Start()
    {
        progressTime = 0;
        displayCharNum = 0;//文のうち何文字表示しているか
        text = this.GetComponent<Text>();
        textNum = 0;
        text.text = textContents[textNum].Substring(0, displayCharNum);
    }

    // Update is called once per frame
    void Update()
    {
        ProgressText();
        this.Finish();
    }
    //textを表示
    void ProgressText()
    {
        if(progressTime<1)progressTime += Time.deltaTime;
        if (progressTime >0.1f)//0.1秒で次の文字表示
        {
            if (displayCharNum < textContents[textNum].Length)
            {
                progressTime = 0;
                displayCharNum++;
                text.text = textContents[textNum].Substring(0, displayCharNum);
            }
        }
        //ボタンを押したら文を最後まで一気に表示
        if (displayCharNum < textContents[textNum].Length && Input.GetKeyUp(KeyCode.Space))
        {
            displayCharNum = textContents[textNum].Length - 1;
            text.text = textContents[textNum].Substring(0, displayCharNum);
        }
        //文字が全部表示されてボタンを押したら次の文が表示される
        if (displayCharNum == textContents[textNum].Length && textNum < textContents.Length - 1 && Input.GetKeyUp(KeyCode.Space))
        {
            textNum++;
            displayCharNum = 0;
            progressTime = 0;
            text.text = textContents[textNum].Substring(0, displayCharNum);
        }
    }
    void Finish()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (this.textNum >= this.textContents.Length - 1)
            {
                Destroy(transform.parent.parent.gameObject);
                Manager.fase++;
            }
        }
    }
}
