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
    public static bool nextText;
    // Start is called before the first frame update
    void Start()
    {
        progressTime = 0;
        displayCharNum = 0;//文のうち何文字表示しているか
        text = this.GetComponent<Text>();
        textNum = 0;
        text.text = textContents[textNum].Substring(0, displayCharNum);
        nextText = false;
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
        
        if (displayCharNum < textContents[textNum].Length && Input.GetMouseButtonUp(0))
        {
            
            //後からやらないとボタン連打した時に正しく表示されない
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(displayCharNum < textContents[textNum].Length)//ボタンを押したら文を最後まで一気に表示
            {
                displayCharNum = textContents[textNum].Length;
                text.text = textContents[textNum].Substring(0, displayCharNum);
            }
            else if(textNum < textContents.Length - 1 && nextText)//文字が全部表示されてボタンを押したら次の文が表示される
            {
                displayCharNum = 0;
                progressTime = 0;
                textNum++;
                text.text = textContents[textNum].Substring(0, displayCharNum);
            }
        }
    }
    void Finish()//テキスト終わりに起こすアクション
    {
        if (Input.GetMouseButtonUp(0))
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
    }
}
