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
    public void EffectPlay(params string[] EffectNames) //�������� ����
    {
        for (int i = 0; i < EffectNames.Length; i++)
        {
            // Effect �迭���� EffectNames[i]�� �����ϴ� ����� Ŭ���� ã���ϴ�.
            AudioClip foundClip = System.Array.Find(Effect, clip => clip.name.Contains(EffectNames[i]));

            if (foundClip != null)
            {
                // ã�� ����� Ŭ���� EffectaudioSource[i]�� �Ҵ��ϰ� ����մϴ�.
                EffectaudioSource[i].clip = foundClip;
                EffectaudioSource[i].Play();
            }
            else
            {
                // ����� Ŭ���� ã�� ���� ��쿡 ���� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
                Debug.LogWarning("����� Ŭ���� ã�� �� �����ϴ�: " + EffectNames[i]);
            }
        }
    }

    //public void EffectPlay(string EffectName)
    //{
    //    // Effect �迭���� EffectName�� �����ϴ� ����� Ŭ���� ã���ϴ�.
    //    AudioClip foundClip = System.Array.Find(Effect, clip => clip.name.Contains(EffectName));

    //    if (foundClip != null)
    //    {
    //        // ã�� ����� Ŭ���� EffectaudioSource�� �Ҵ��ϰ� ����մϴ�.
    //        EffectaudioSource[0].clip = foundClip;
    //        EffectaudioSource[0].Play();
    //    }
    //    else
    //    {
    //        // ����� Ŭ���� ã�� ���� ��쿡 ���� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
    //        Debug.LogWarning("����� Ŭ���� ã�� �� �����ϴ�: " + EffectName);
    //    }
    //}


    public int songNumber;
    public void SongPreview()
    {
        string currentSongNameModified = GameManager.Instance.CurrentSongName.Replace("_", " ").ToLower(); // ������ھ ����� �����ϰ� �ҹ��ڷ� ��ȯ
        for (songNumber = 0; songNumber < Song.Length; songNumber++)
        {
            if (Song[songNumber].name.ToLower().Replace("_", " ").Contains(currentSongNameModified)) // ����� Ŭ���� �̸��� �����ϰ� �ҹ��ڷ� ��ȯ�Ͽ� ��
            {
                Debug.Log($"����� Ŭ�� '{GameManager.Instance.CurrentSongName}'�� �迭 ����: " + songNumber);
                BGMaudiosource.clip = Song[songNumber];
               // BGMaudiosource.time = GameManager.Instance.prelistening;
                BGMaudiosource.Play();
                break; // ����� Ŭ���� ã�����Ƿ� �ݺ��� ����
            }

        }
        //BGMaudiosource.Play();
        // GameManager.Instance.CurrentSong
    }

    public bool SongStart;
   

    public void SongPlay()
    {
        
        string currentSongNameModified = GameManager.Instance.CurrentSongName.Replace("_", " ").ToLower(); // ������ھ ����� �����ϰ� �ҹ��ڷ� ��ȯ
        for (songNumber = 0; songNumber < Song.Length; songNumber++)
        {
            if (Song[songNumber].name.ToLower().Replace("_", " ").Contains(currentSongNameModified)) // ����� Ŭ���� �̸��� �����ϰ� �ҹ��ڷ� ��ȯ�Ͽ� ��
            {
                Debug.Log($"����� Ŭ�� '{GameManager.Instance.CurrentSongName}'�� �迭 ����: " + songNumber);
                // BGMaudiosource.time= GameManager.Instance.SoundOffset;
                 BGMaudiosource.PlayDelayed(GameManager.Instance.SoundOffset);
               
                break; // ����� Ŭ���� ã�����Ƿ� �ݺ��� ����
            }

        }

    }




}
