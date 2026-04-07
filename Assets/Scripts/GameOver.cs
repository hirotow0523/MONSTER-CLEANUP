using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject Player;
    public GameObject GameOverCanvas;

    // フェードアウト
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    bool isGameReStart = false;
    bool isQuitGame = false;
    bool isSEPlayed = false;
    float fadeAlpha = 0f;

    void Update()
    {
        // プレイヤー死亡時の処理
        if (!Player && !isSEPlayed)
        {
            ShowGameOver();
            isSEPlayed = true;
        }

        // フェード処理でリスタート
        if (isGameReStart)
        {
            fadeAlpha += Time.deltaTime * fadeSpeed;

            Color c = fadeImage.color;
            c.a = fadeAlpha;
            fadeImage.color = c;

            if (fadeAlpha >= 1f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // フェード処理でタイトル画面へ戻る
        if (isQuitGame)
        {
            fadeAlpha += Time.deltaTime * fadeSpeed;

            Color c = fadeImage.color;
            c.a = fadeAlpha;
            fadeImage.color = c;

            if (fadeAlpha >= 1f)
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }

    // ゲームオーバー
    public void ShowGameOver()
    {
        SEManager.Instance.PlayGameEnd();
        GameOverCanvas.SetActive(true);
    }

    // ゲームをリスタート
    public void GameReStart()
    {
        if (isGameReStart) return;

        isGameReStart = true;
        SEManager.Instance.PlayDecide();
    }

    // タイトル画面に移動
    public void QuitGame()
    {
        if (isQuitGame) return;

        isQuitGame = true;
        SEManager.Instance.PlayDecide();
    }
}
