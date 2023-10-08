using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public void Awake()
    {
        if (Instance == null) //�������� �ڽ��� üũ��, null����
        {
            Instance = this; //���� �ڱ� �ڽ��� ������.
            DontDestroyOnLoad(gameObject);//���� ��ȯ�� �Ǿ �ı����� �ʰ� ������.
        }
    }
    public AudioSource audiosource;

    public AudioClip[] audioClips;


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
        audioClips = Resources.LoadAll<AudioClip>("Mp3");

    }

    public int songNumber;
    public void SongPreview()
    {
        string currentSongNameModified = GameManager.Instance.CurrentSongName.Replace("_", " ").ToLower(); // ������ھ ����� �����ϰ� �ҹ��ڷ� ��ȯ
        for (songNumber = 0; songNumber < audioClips.Length; songNumber++)
        {
            if (audioClips[songNumber].name.ToLower().Replace("_", " ").Contains(currentSongNameModified)) // ����� Ŭ���� �̸��� �����ϰ� �ҹ��ڷ� ��ȯ�Ͽ� ��
            {
                Debug.Log($"����� Ŭ�� '{GameManager.Instance.CurrentSongName}'�� �迭 ����: " + songNumber);
                audiosource.clip = audioClips[songNumber];
                audiosource.Play();
                break; // ����� Ŭ���� ã�����Ƿ� �ݺ��� ����
            }

        }
        //audiosource.Play();
        // GameManager.Instance.CurrentSong
    }

    public bool SongStart;
    public void SongPlay()
    {
        string currentSongNameModified = GameManager.Instance.CurrentSongName.Replace("_", " ").ToLower(); // ������ھ ����� �����ϰ� �ҹ��ڷ� ��ȯ
        for (songNumber = 0; songNumber < audioClips.Length; songNumber++)
        {
            if (audioClips[songNumber].name.ToLower().Replace("_", " ").Contains(currentSongNameModified)) // ����� Ŭ���� �̸��� �����ϰ� �ҹ��ڷ� ��ȯ�Ͽ� ��
            {
                Debug.Log($"����� Ŭ�� '{GameManager.Instance.CurrentSongName}'�� �迭 ����: " + songNumber);
                audiosource.clip = audioClips[songNumber];
                audiosource.Play();
                break; // ����� Ŭ���� ã�����Ƿ� �ݺ��� ����
            }

        }
        //audiosource.Play();
        // GameManager.Instance.CurrentSong
    }





}

    //public void SongPreview2()
    //{
    //    string currentSongName = GameManager.Instance.CurrentSongName.ToString().Replace("_", " "); // ������ھ ����� ����

    //    for (int i = 0; i < audioClips.Length; i++)
    //    {
    //        if (audioClips[i] && audioClips[i].name.Contains(currentSongName))
    //        {
    //            Debug.Log($"����� Ŭ�� '{currentSongName}'�� �迭 ����: " + i);
    //            audiosource.clip = audioClips[i];
    //            break; // ����� Ŭ���� ã�����Ƿ� �ݺ��� ����
    //        }
    //    }

    //    if (audiosource.clip != null)
    //    {
    //        audiosource.Play();
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"����� Ŭ�� '{currentSongName}'�� ã�� �� �����ϴ�.");
    //    }
    //    // currentSongName ���
    //}

