using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public void Awake()
    {
        if (Instance == null) //정적으로 자신을 체크함, null인진
        {
            Instance = this; //이후 자기 자신을 저장함.
            DontDestroyOnLoad(gameObject);//씬이 전환이 되어도 파괴되지 않고 유지됨.
        }
    }
    public AudioSource BGMaudiosource;
    public AudioSource[] EffectaudioSource;

    public AudioClip[] Song;
    public AudioClip[] Effect;


    //private void Update()
    //{
    //    if(GameManager.Instance.currentState == CurrentState.Ingame)
    //    {
    //       BGMaudiosource.Stop();
    //    }
    //}
    public void StopBGM()
    {
        BGMaudiosource.Stop();
    }
    public void StartBGM()
    {
        BGMaudiosource.Play();
    }
    private void Start()
    {
       // BGMaudiosource = GetComponent<AudioSource>();
        Song = Resources.LoadAll<AudioClip>("Mp3");
        Effect = Resources.LoadAll<AudioClip>("Effect");

    }
    public void EffectPlay(params string[] EffectNames) //유동적인 범위
    {
        for (int i = 0; i < EffectNames.Length; i++)
        {
            // Effect 배열에서 EffectNames[i]를 포함하는 오디오 클립을 찾습니다.
            AudioClip foundClip = System.Array.Find(Effect, clip => clip.name.Contains(EffectNames[i]));

            if (foundClip != null)
            {
                // 찾은 오디오 클립을 EffectaudioSource[i]에 할당하고 재생합니다.
                EffectaudioSource[i].clip = foundClip;
                EffectaudioSource[i].Play();
            }
            else
            {
                // 오디오 클립을 찾지 못한 경우에 대한 처리를 여기에 추가할 수 있습니다.
                Debug.LogWarning("오디오 클립을 찾을 수 없습니다: " + EffectNames[i]);
            }
        }
    }

    //public void EffectPlay(string EffectName)
    //{
    //    // Effect 배열에서 EffectName을 포함하는 오디오 클립을 찾습니다.
    //    AudioClip foundClip = System.Array.Find(Effect, clip => clip.name.Contains(EffectName));

    //    if (foundClip != null)
    //    {
    //        // 찾은 오디오 클립을 EffectaudioSource에 할당하고 재생합니다.
    //        EffectaudioSource[0].clip = foundClip;
    //        EffectaudioSource[0].Play();
    //    }
    //    else
    //    {
    //        // 오디오 클립을 찾지 못한 경우에 대한 처리를 여기에 추가할 수 있습니다.
    //        Debug.LogWarning("오디오 클립을 찾을 수 없습니다: " + EffectName);
    //    }
    //}


    public int songNumber;
    public void SongPreview()
    {
        string currentSongNameModified = GameManager.Instance.CurrentSongName.Replace("_", " ").ToLower(); // 언더스코어를 띄어쓰기로 변경하고 소문자로 변환
        for (songNumber = 0; songNumber < Song.Length; songNumber++)
        {
            if (Song[songNumber].name.ToLower().Replace("_", " ").Contains(currentSongNameModified)) // 오디오 클립의 이름을 변경하고 소문자로 변환하여 비교
            {
                Debug.Log($"오디오 클립 '{GameManager.Instance.CurrentSongName}'의 배열 순서: " + songNumber);
                BGMaudiosource.clip = Song[songNumber];
               // BGMaudiosource.time = GameManager.Instance.prelistening;
                BGMaudiosource.Play();
                break; // 오디오 클립을 찾았으므로 반복문 종료
            }

        }
        //BGMaudiosource.Play();
        // GameManager.Instance.CurrentSong
    }

    public bool SongStart;
   

    public void SongPlay()
    {
        
        string currentSongNameModified = GameManager.Instance.CurrentSongName.Replace("_", " ").ToLower(); // 언더스코어를 띄어쓰기로 변경하고 소문자로 변환
        for (songNumber = 0; songNumber < Song.Length; songNumber++)
        {
            if (Song[songNumber].name.ToLower().Replace("_", " ").Contains(currentSongNameModified)) // 오디오 클립의 이름을 변경하고 소문자로 변환하여 비교
            {
                Debug.Log($"오디오 클립 '{GameManager.Instance.CurrentSongName}'의 배열 순서: " + songNumber);
                // BGMaudiosource.time= GameManager.Instance.SoundOffset;
                 BGMaudiosource.PlayDelayed(GameManager.Instance.SoundOffset);
               
                break; // 오디오 클립을 찾았으므로 반복문 종료
            }

        }

    }




}
