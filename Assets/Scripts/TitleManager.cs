using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    // Click To Startの処理
    public Image clickText;
    public float blinkSpeed = 2f;

    // フェードアウト
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    bool isStarting = false;
    float fadeAlpha = 0f;

    void Update()
    {
        // Click To Startを点滅
        if (!isStarting)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
            Color c = clickText.color;
            c.a = alpha;
            clickText.color = c;
        }

        // フェード処理
        if (isStarting)
        {
            fadeAlpha += Time.deltaTime * fadeSpeed;

            Color c = fadeImage.color;
            c.a = fadeAlpha;
            fadeImage.color = c;
            clickText.color = c;

            if (fadeAlpha >= 1f)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    // ゲームスタート
    public void StartGame()
    {
        if (isStarting) return;

        isStarting = true;
        SEManager.Instance.PlayDecide();
    }
}