using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSystemManager : MonoBehaviour
{
    public int Score;

    public Text ScoreText; // 戦闘時のスコア
    public Text ResultScoreText; // ゲーム終了時のスコア
    public Text RankLabelText;
    public TMP_Text RankValueText;

    bool isShown = false;

    void Start()
    {
        Time.timeScale = 1;

        ResultScoreText.gameObject.SetActive(false);
        RankLabelText.gameObject.SetActive(false);
        RankValueText.gameObject.SetActive(false);
    }

    void Update()
    {
        ScoreText.text = Score.ToString();

        if (GameTimer.gameEnd && !isShown)
        {
            ShowResult();
            ScoreText.gameObject.SetActive(false);
        }
    }

    // ゲーム終了でランク表示
    void ShowResult()
    {
        isShown = true;

        ResultScoreText.gameObject.SetActive(true);
        RankLabelText.gameObject.SetActive(true);
        RankValueText.gameObject.SetActive(true);

        ResultScoreText.text = "Score : " + Score.ToString(); // スコア表示

        string rank = "C";

        if (Score >= 2000)
        {
            RankValueText.enableVertexGradient = true;

            rank = "S";
            RankValueText.colorGradient = new VertexGradient(
                new Color32(130, 0, 190, 255),  // 左上(濃い紫)
                new Color32(220, 140, 255, 255),  // 右上(ピンク)
                new Color32(220, 100, 255, 255), // 左下(濃いピンク)
                new Color32(130, 0, 155, 255)  // 右下(濃い紫)
            );
        }
        else if (Score >= 1500)
        {
            rank = "A";
            RankValueText.color = new Color32(255, 50, 50, 255); // 黄味の赤
        }
        else if (Score >= 1000)
        {
            rank = "B";
            RankValueText.color = new Color32(40, 200, 255, 255); // 水色
        }
        else
        {
            rank = "C";
            RankValueText.color = new Color32(50, 255, 50, 255); // 明るい黄緑
        }

        RankValueText.text = rank; // ランク表示
    }
}