using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    /// <summary>
    /// ゲームの進行度
    /// </summary>
    public static int fase;
    public static bool action;//完全にフェードインするまで攻撃しないように
    /// <summary>
    /// ステッカーを選択できるかどうか
    /// </summary>
    public static bool isStickerClick;
    
    public GameObject[] enemyList;//Unity側で追加
    public GameObject storyText;//Unity側で追加Text3Line
    public Sprite[] BackgroundList;//Unity側で追加
    public GameObject[] charList;//Unityから代入（GameOver,GameClear文字）
    public GameObject charTable;//文字をのせる台の当たり判定を操作 UnityからCharTable

    public List<GameObject> currentEnemies = new List<GameObject>();//現ステージ残存エネミー
    StickerManager stManager;
    Player player;
    NextStageAnim blackAnim;
    SpriteRenderer spriteRenderer;
    public AudioClip[] sounds;
    AudioSource audioSource;
    string[][] sentences = new string[3][];
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; //FPSを60に設定 
        action = false;
        this.stManager = this.GetComponent<StickerManager>();
        this.player = GameObject.Find("Player").GetComponent<Player>();
        this.blackAnim = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<NextStageAnim>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.audioSource = this.GetComponent<AudioSource>();

        fase = 0;

        this.sentences[0] = new string[3];
        this.sentences[0][0] = "探しものをマウスでクリックして見つけよう！\n合っていれば攻撃できるぞ";
        this.sentences[0][1] = "敵の攻撃時間に注意しよう";
        this.sentences[0][2] = "出発だ";

        this.DisplayText(0);

        this.sentences[1] = new string[1];
        this.sentences[1][0] = "「ゲームオーバー」\nマウス左クリックでタイトルに戻る";

        this.sentences[2] = new string[2];//文増やすならTextControllerのイベントプログラムを変更する必要あり、Manager上のisNextText変数への代入も
        this.sentences[2][0] = "何とか無事に森を抜けられそうだ\nTo be continued?";
        this.sentences[2][1] = "「ゲームクリア」\nマウス左クリックでタイトルに戻る";
    }

    // Update is called once per frame
    void Update()
    {
        if(action) this.ObjectSelection();
        switch (fase)
        {
            case 0:
                //テキストが終わり次第、次のフェーズに進む
                break;
            case 1:
                blackAnim.isFadeIn = true;
                this.spriteRenderer.sprite = this.BackgroundList[0];
                this.CreateEnemy(this.enemyList[0], this.enemyList[1], this.enemyList[0]);
                //stManager.ResetSticker();フェードインしてからステッカーを配置に変更
                fase++;
                break;
            case 2://戦闘フェーズ
                
                break;
            case 3:
                blackAnim.isFadeIn = true;
                this.spriteRenderer.sprite = this.BackgroundList[1];
                this.CreateEnemy(this.enemyList[2], this.enemyList[3], this.enemyList[2]);
                fase++;
                break;
            case 4://戦闘フェーズ

                break;
            case 5:
                blackAnim.isFadeIn = true;
                this.spriteRenderer.sprite = this.BackgroundList[2];
                this.CreateEnemy(this.enemyList[3], this.enemyList[4], this.enemyList[2]);
                fase++;
                break;
            case 6://戦闘フェーズ

                break;
            case 7:
                //print("finish");
                this.DisplayText(2);
                TextController.isNextText = false;
                fase++;
                break;
            case 8:
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
        Vector2[] enemyPos = {new Vector2(-4, 1.5f), new Vector2(0, 1.5f), new Vector2(4, 1.5f)};//3体のエネミーの位置
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
                        if (hit.transform.GetComponent<Sticker>() && isStickerClick)
                        {
                            print("選択したスティッカー： " + hit.transform.GetComponent<Sticker>().stickeNumber);
                            if (this.stManager.CompareSticker(hit.transform.GetComponent<Sticker>().stickeNumber))
                            {
                                this.player.Attack();
                                StickerManager.theme = -2;
                                
                            }
                            else
                            {
                                this.audioSource.volume = 1;
                                this.StartCoroutine(this.IncorrectSound());
                            }
                        }
                        break;
                }
            }
            
        }
    }
    public void DefaultTargetting()//デフォルトやターゲットしていた敵が死んだ時は左から順々に狙う、敵がいなければfaseを進める
    {
        if (!CheckLivingEnemy())//敵が全滅してたら
        {
            Player.target = 0;
            //print("finish");

            blackAnim.isFadeOut = true;//フェードアウトしてからフェーズを進める
        }
    }
    /// <summary>
    /// 敵の残存をチェック
    /// </summary>
    /// <returns true> 生きてる敵がいる</returns　false>生きている敵はいない
    public bool CheckLivingEnemy()//
    {
        for (int i = 0; i < this.currentEnemies.Count; i++)
        {
            if (this.currentEnemies[i] != null)
            {
                Player.target = i;
                return true;
            }
        }
        return false;
    }
    IEnumerator IncorrectSound()
    {
        this.audioSource.clip = this.sounds[0];
        this.audioSource.time = 0.5f;
        this.audioSource.Play();
        yield return new WaitForSeconds(0.1f);
        this.audioSource.Stop();
    }
    void DisplayText(int index)//テキスト表示
    {
        GameObject obj = Instantiate(this.storyText);
        TextController cmp = obj.transform.GetChild(0).GetChild(0).GetComponent<TextController>();
        cmp.textContents = this.sentences[index];
    }
    public IEnumerator GameOver()
    {
        Instantiate(this.charList[0]);
        this.charTable.GetComponent<BoxCollider2D>().enabled = true;//直前にオンにしないとステッカーをクリックできない時がある
        yield return new WaitForSeconds(2);
        this.DisplayText(1);
    }
    public IEnumerator GameClear(TextController tc)
    {
        GameObject obj = Instantiate(this.charList[1]);
        SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
        spr.color = new Color(1, 1, 1, 0);//透明化
        for(int i = 1; i <= 20; i++)
        {
            spr.color = new Color(1, 1, 1, 1.0f / 20.0f * i);
            yield return new WaitForSeconds(0.1f);
        }
        TextController.isNextText = true;
        tc.NextText();//自動的に次のテキストを表示
    }
}
