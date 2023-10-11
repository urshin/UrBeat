using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum CurrentState
{
    logo,
    LobbySongSelect,
    LobbyDifficultSelect,
    Ingame,
    result,
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public void Awake()
    {
        if (Instance == null) //�������� �ڽ��� üũ��, null����
        {
            Instance = this; //���� �ڱ� �ڽ��� ������.
            DontDestroyOnLoad(gameObject);//���� ��ȯ�� �Ǿ �ı����� �ʰ� ������.
        }
    }
   
   
    public CurrentState currentState;

    // currentState�� �����ϴ� ���� �Լ�
    public void ChangeState(CurrentState newState)
    {
        currentState = newState;
    }

    public List<string> CurrentSongDiffifultList = new List<string>();

    [Header("���Ӹ����� ����")]
    public bool autoPlay;
    public bool TimeStopButton;



    [Header("�� ����")]
    public string CurrentSongName; //���� �� �̸�
    public string CurrentDifficult; //���� �� ���̵�
    public string CurrentSongAndDiff; //�̸�+���̵�
    public string Title; //�ؽ�Ʈ ���Ϸ� ���� Ÿ��Ʋ
    public string Artist;
    public float BPM;
    public float Notes;
    public float Dif;
    public string Difficult;
    public float Level;
    public float TotalNote;
    public float prelistening;
    public float SoundOffset;

    public int MaxComboNum;
    public Sprite MusicImage;

    public float findrasmemo;
    public string filePath;
    public TextAsset textAsset;
    public string[] lines;



    //public void ReadSongTxt()
    //{
    //    CurrentSongAndDiff = CurrentSongName + "_" + CurrentDifficult;
    //    filePath = "music/" + CurrentSongAndDiff;
    //    textAsset = new TextAsset();
    //    textAsset = Resources.Load<TextAsset>(filePath);
    //    Debug.Log(filePath);
    //    lines = textAsset.text.Split('\n'); // ����ó�� �ʿ�
    //}
    public void ReadSongTxt()
    {
        CurrentSongAndDiff = CurrentSongName + "_" + CurrentDifficult;
        filePath = "music/" + CurrentSongAndDiff;
        textAsset = Resources.Load<TextAsset>(filePath);

        if (textAsset == null)
        {
            Debug.LogError("Failed to load text asset at path: " + filePath);
            return; // ���� ó�� �Ǵ� ���� ó���� �����ϰų� �� �޼��带 �����մϴ�.
        }

        Debug.Log(filePath);
        lines = textAsset.text.Split('\n');
    }


    private void Update()
    {
        TimeStop();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            autoPlay = !autoPlay;
        }
    }
    void TimeStop()
    {
        if(TimeStopButton)
        {
            Time.timeScale = 0;

        }
        else
        {
            Time.timeScale = 1;
        }
    }



    public TextAsset[] textAssets; //�о���� �ؽ�Ʈ ���� �迭
    void Start()
    {
        // Resources �������� "music" ���� ���� ��� TextAsset�� �˻�
        textAssets = Resources.LoadAll<TextAsset>("music");
        
    }
    public void FindTextFile()
    {
        CurrentSongDiffifultList = new List<string>();

        for (int i = 0; i < textAssets.Length; i++)
        {
            if (textAssets[i].text.Contains(CurrentSongName))
            {
                CurrentSongDiffifultList.Add(textAssets[i].name);
                //Debug.Log(textAssets[i].name);

            }
        }

    }

    public void GoToScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }






}
