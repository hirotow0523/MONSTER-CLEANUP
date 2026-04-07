using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;

    AudioSource audioSource;
    public AudioClip DecideSE;
    public AudioClip GameEndSE;
    public AudioClip CountDownSE;

    void Awake()
    {
        // 既に存在する場合は自分を削除
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // シーンを跨いでも消えない
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    // 決定/クリック音
    public void PlayDecide()
    {
        if (DecideSE != null)
        {
            audioSource.PlayOneShot(DecideSE);
        }
    }
    
    // ゲーム終了音
    public void PlayGameEnd()
    {
        if (GameEndSE != null)
        {
            audioSource.PlayOneShot(GameEndSE);
        }
    }

    // カウントダウン音
    public void PlayCountDown()
    {
        if (CountDownSE != null)
        {
            audioSource.PlayOneShot(CountDownSE);
        }
    }
}