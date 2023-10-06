using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    [Header("�� ����")]
    public string CurrentSongName; //���� �� �̸�
    public string CurrentDifficult; //���� �� ���̵�
    public string CurrentSongAndDiff; //�̸�+���̵�
    public string Title; //�ؽ�Ʈ ���Ϸ� ���� Ÿ��Ʋ
    public string Artist;
    public float BPM;
    public float Notes;
    public float Dif;
    public float Level;
    public float TotalNote;
    public Sprite MusicImage;

    public float findrasmemo;
    public string filePath;
    public TextAsset textAsset;
    public string[] lines;



    public void ReadSongTxt()
    {
        CurrentSongAndDiff = CurrentSongName + "_" + CurrentDifficult;
        filePath = "music/" + CurrentSongAndDiff;
        textAsset = new TextAsset();
        textAsset = Resources.Load<TextAsset>(filePath);
        Debug.Log(filePath);
        lines = textAsset.text.Split('\n');
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








}
