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
    public AudioSource audiosource;

    public AudioClip[] Song;


    //private void Update()
    //{
    //    if(GameManager.Instance.currentState == CurrentState.Ingame)
    //    {
    //       audiosource.Stop();
    //    }
    //}
    public void StopBGM()
    {
        audiosource.Stop();
    }
    public void StartBGM()
    {
        audiosource.Play();
    }
    private void Start()
    {
       // audiosource = GetComponent<AudioSource>();
        Song = Resources.LoadAll<AudioClip>("Mp3");

    }

    public int songNumber;
    public void SongPreview()
    {
        string currentSongNameModified = GameManager.Instance.CurrentSongName.Replace("_", " ").ToLower(); // 언더스코어를 띄어쓰기로 변경하고 소문자로 변환
        for (songNumber = 0; songNumber < Song.Length; songNumber++)
        {
            if (Song[songNumber].name.ToLower().Replace("_", " ").Contains(currentSongNameModified)) // 오디오 클립의 이름을 변경하고 소문자로 변환하여 비교
            {
                Debug.Log($"오디오 클립 '{GameManager.Instance.CurrentSongName}'의 배열 순서: " + songNumber);
                audiosource.clip = Song[songNumber];
                audiosource.Play();
                break; // 오디오 클립을 찾았으므로 반복문 종료
            }

        }
        //audiosource.Play();
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
                audiosource.clip = Song[songNumber];
                audiosource.Play();
                break; // 오디오 클립을 찾았으므로 반복문 종료
            }

        }
       
    }





}
