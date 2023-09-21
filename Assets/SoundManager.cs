using UnityEngine;
using System.Collections.Generic;

// SoundManager: 게임의 모든 사운드 관리를 위한 싱글톤 클래스
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // 싱글톤 인스턴스

    // 게임 내에서 사용될 오디오 클립들
    public AudioClip backgroundMusicClip;
    public AudioClip titleMusicClip;
    public List<AudioClip> soundEffects;

    // 볼륨 설정. 외부에서 읽기는 가능하나 변경은 SetMasterVolume을 통해서만 가능
    public float BGMmasterVolume { get; private set; } = 1f;
    public float SoundmasterVolume { get; private set; } = 1f;

    // AudioSource 컴포넌트를 재사용하기 위한 필드
    private AudioSource audioSource;

    // 사운드 이펙트 이름과 클립을 매핑하기 위한 딕셔너리
    private Dictionary<string, AudioClip> soundEffectDictionary;

    [System.Serializable]
    public class SoundEffect
    {
        public string name; // 사운드 이펙트 이름
        public AudioClip clip; // 사운드 이펙트 클립
    }

    // Awake: 초기 싱글톤 설정과 사운드 이펙트 딕셔너리 초기화
    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSource 컴포넌트 초기화
        audioSource = GetComponent<AudioSource>();

        // 사운드 이펙트 딕셔너리 초기화
        soundEffectDictionary = new Dictionary<string, AudioClip>();
        for (int i = 0; i < soundEffects.Count; i++)
        {
            soundEffectDictionary.Add(soundEffects[i].name, soundEffects[i]);
        }
    }

    // 배경 음악 재생
    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusicClip);
    }

    // 타이틀 음악 재생
    public void PlayTitleMusic()
    {
        PlayMusic(titleMusicClip);
    }

    // 주어진 오디오 클립 재생
    private void PlayMusic(AudioClip clip)
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 재사용된 AudioSource 설정
        audioSource.clip = clip;
        audioSource.volume = BGMmasterVolume;
        audioSource.Play();
    }

    // 사운드 이펙트를 재생하고, 그에 대한 SoundEffectPlayer 인스턴스를 반환
    public SoundEffectPlayer PlaySoundEffect(string name)
    {
        if (soundEffectDictionary.ContainsKey(name))
        {
            GameObject soundEffectObject = new GameObject($"SoundEffect_{name}");
            SoundEffectPlayer player = soundEffectObject.AddComponent<SoundEffectPlayer>();
            player.PlayEffect(soundEffectDictionary[name], SoundmasterVolume);

            return player;
        }
        else
        {
            Debug.LogWarning("Sound effect not found: " + name);
            return null;
        }
    }

    // 마스터 볼륨 설정
    public void SetBGMMasterVolume(float volume)
    {
        BGMmasterVolume = volume;
        audioSource.volume = BGMmasterVolume;
    }
    public void SetSoundMasterVolume(float volume)
    {
        SoundmasterVolume = volume;
    }
}