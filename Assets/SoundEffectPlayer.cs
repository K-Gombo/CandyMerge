using System.Collections;
using UnityEngine;

// SoundEffectPlayer: 개별 사운드 이펙트를 재생하기 위한 클래스
public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource audioSource; // 이 클래스에서 사용할 AudioSource

    // 주어진 클립과 볼륨으로 사운드 이펙트 재생
    public void PlayEffect(AudioClip clip, float volume)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("PlayEffect에서 audioSource가 없음");
            return;
        }
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        // 사운드 이펙트가 끝날 때까지 기다림
        StartCoroutine(WaitForSoundToEnd());
    }

    // 사운드 이펙트 중지 및 객체 제거
    public void StopEffect()
    {
        Debug.Log($"A : {audioSource != null} B : {audioSource.isPlaying}");
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("삭제!");
            Destroy(gameObject);
        }
    }

    // 사운드 이펙트가 끝나면 StopEffect 호출
    private IEnumerator WaitForSoundToEnd()
    {
        while (audioSource.isPlaying)
        {
            yield return null; // 다음 프레임까지 기다림
;        }
        StopEffect();
    }
}