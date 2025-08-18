using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;  // 싱글톤 인스턴스
    public AudioSource audioSource;

    void Awake()
    {
        // Scene이 바뀌어도 오브젝트가 파괴되지 않도록 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 이미 있으면 중복 제거
            return;
        }
    }

    void Start()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true; // 자동 반복
            audioSource.Play();      // 자동 재생
        }
    }

    // BGM 멈추기
    public void StopBGM()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
    // 볼륨 조절 (0~1)
    public void SetVolume(float value)
    {
        audioSource.volume = Mathf.Clamp01(value);
    }
}
