using System.Collections;
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
    Vector2[] positionList = {new Vector2(-8,-2), new Vector2(0,-2), new Vector2(8,-2)};//スティッカーを配置するときの位置
    List<int> distributedSticker = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }


    /// <summary>
    /// お題のシルエット生成
    /// </summary>
    void CreateTheme()
    {
        theme = Random.Range(0, this.stickerList.Length);
        GameObject obj = Instantiate(this.stickerObject, new Vector2(12,0), Quaternion.identity);//シルエット生成（位置はここで設定）
        this.distributedObj.Add(obj);
        obj.GetComponent<Sticker>().stickeNumber = -1;
        obj.GetComponent<SpriteRenderer>().sprite = this.stickerList[theme];
        obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);//画像を黒くする
    }
    /// <summary>
    /// ステッカーを配置
    /// </summary>
    public void DistributeSticker()
    {
        this.DeletSticker();
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
        print("お題： " + theme);
    }
    public void DeletSticker()//ゲーム上のステッカー全削除（お題含む）
    {
        for (int i = 0; i < this.distributedObj.Count; i++)
        {
            Destroy(this.distributedObj[i]);
        }
        this.distributedObj.Clear();
    }
    /// <summary>
    /// 選択画像がお題と一致するか
    /// </summary>
    /// <param name="s"></param>一致
    /// <returns></returns>不一致
    public bool CompareSticker(int s)
    {
        if (theme == s) return true;
        else return false;
    }
}
