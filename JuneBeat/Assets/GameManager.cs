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
        if (Instance == null) //정적으로 자신을 체크함, null인진
        {
            Instance = this; //이후 자기 자신을 저장함.
            DontDestroyOnLoad(gameObject);//씬이 전환이 되어도 파괴되지 않고 유지됨.
        }
    }
   
   
    public CurrentState currentState;

    // currentState를 변경하는 예제 함수
    public void ChangeState(CurrentState newState)
    {
        currentState = newState;
    }

    public List<string> CurrentSongDiffifultList = new List<string>();

    [Header("게임마스터 권한")]
    public bool autoPlay;
    public bool TimeStopButton;



    [Header("곡 정보")]
    public string CurrentSongName; //현재 곡 이름
    public string CurrentDifficult; //현재 곡 난이도
    public string CurrentSongAndDiff; //이름+난이도
    public string Title; //텍스트 파일로 읽은 타이틀
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
    //    lines = textAsset.text.Split('\n'); // 예외처리 필요
    //}
    public void ReadSongTxt()
    {
        CurrentSongAndDiff = CurrentSongName + "_" + CurrentDifficult;
        filePath = "music/" + CurrentSongAndDiff;
        textAsset = Resources.Load<TextAsset>(filePath);

        if (textAsset == null)
        {
            Debug.LogError("Failed to load text asset at path: " + filePath);
            return; // 예외 처리 또는 오류 처리를 수행하거나 이 메서드를 종료합니다.
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



    public TextAsset[] textAssets; //읽어들인 텍스트 파일 배열
    void Start()
    {
        // Resources 폴더에서 "music" 폴더 안의 모든 TextAsset을 검색
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
