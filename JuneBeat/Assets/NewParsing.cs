using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class NewParsing : MonoBehaviour
{

    // [SerializeField] Text Title;
    // [SerializeField] Text Artist;
    // [SerializeField] Text BPM;
    // [SerializeField] Text Notes;
    // [SerializeField] Text Dif;
    // [SerializeField] Text Level;
    // [SerializeField] Image MusicImage;

    public string filePath;
    public TextAsset textAsset;
    public string[] lines;


    private void Start()
    {
        //게임 매니저에게 현재 곡정보. 알려주기
        GameManager.Instance.CurrentSongAndDiff = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult;

        ShowInfoSong();

        ReadSongTxt();



        PositionTimingParsing();

    }




    public Dictionary<int, char> symbolToIndex1 = new Dictionary<int, char>
    {
         {0, ' '},  {1, ' '},  {2, ' '},  {3, ' '},
         {4, ' '},  {5, ' '},  {6, ' '},  {7, ' '},
         {8, ' '},  {9, ' '},  {10, ' '}, {11, ' '},
         {12, ' '}, {13, ' '}, {14, ' '}, {15, ' '},
         //{0+16, ' '},  {1+16, ' '},  {2+16, ' '},  {3, ' '},
         //{4+16, ' '},  {5+16, ' '},  {6+16, ' '},  {7, ' '},
         //{8+16, ' '},  {9+16, ' '},  {10+16, ' '}, {11+16, ' '},
         //{12+16, ' '}, {13+16, ' '}, {14+16, ' '}, {15+16, ' '},
    }; 
    public Dictionary<int, char> symbolToIndex2 = new Dictionary<int, char>
    {
         {0, ' '},  {1, ' '},  {2, ' '},  {3, ' '},
         {4, ' '},  {5, ' '},  {6, ' '},  {7, ' '},
         {8, ' '},  {9, ' '},  {10, ' '}, {11, ' '},
         {12, ' '}, {13, ' '}, {14, ' '}, {15, ' '},
    };


    [Header("BPM관련")]
    public float musicBPM = 60f;
    public float stdBPM = 60;
    public float musicTempo = 4;
    public float stdTempo = 4;
    float tikTime = 0;
    float nextTime = 0;



    private void ShowInfoSong()
    {
        //  Title.text = GameManager.Instance.Title;
        //  Artist.text = GameManager.Instance.Artist;
        //  BPM.text = GameManager.Instance.BPM.ToString();
        //  Notes.text = GameManager.Instance.Notes.ToString();
        //  MusicImage.sprite = GameManager.Instance.MusicImage.sprite;
        musicBPM = GameManager.Instance.BPM; //음악 BPM 넣기
        LineNum = (int)GameManager.Instance.findrasmemo;

        //이미지 배열로 처리 해두기 나중에....
        //Dif.text = GameManager.Instance.Dif.ToString()  ;
        //Level.text = GameManager.Instance.Level.ToString() ;
    }


    public void ReadSongTxt()
    {
        filePath = "music/" + GameManager.Instance.CurrentSongAndDiff;
        textAsset = new TextAsset();
        textAsset = Resources.Load<TextAsset>(filePath);
        Debug.Log(filePath);
        lines = textAsset.text.Split('\n');
    }

    private void ReadNextNotePosition() //다음줄 읽기 함수
    {
        GoNextPositionParsing = true;
        PositionTimingParsing();
    }

    public List<string> NotePosision = new List<string>(); //노트 포지션
    public List<string> NoteTimeing = new List<string>(); //노트 타이밍
    int LineNum; //라인 넘버
    bool GoNextPositionParsing = true; // 다음줄로 넘어갈지?
    

    private void PositionTimingParsing() //노트 포지션 읽어오기
    {
        NotePosision = new List<string>(); //리스트 생성
        NoteTimeing= new List<string>();
        while (GoNextPositionParsing)
        {
            if (IsNumber(lines[LineNum])) //읽은 라인이 숫자면
            {
                //NotePosision = new List<string>();
                
                GoNextPositionParsing = false;
            }
            if (!IsNumber(lines[LineNum]) && lines[LineNum].Length >= 4) //읽은 라인이 숫자가 아니고 4자리 이상이라면
            {
                NotePosision.Add(lines[LineNum].Substring(0, 4));
                if (lines[LineNum].Length >= 9)
                {
                    NoteTimeing.Add(lines[LineNum].Substring(6, 4)); //노트 타이밍 리스트 넣어주기
                }
            }
            LineNum++; //다음줄
        }
    }




    public void mappingNote(List<string> notepo) //딕셔너리와 연동하는 함수
    {
        symbolToIndex1.Clear();
        int p = 0;
        for (int j = 0; j < 4; j++)
        {
            for (int k = 0; k < 4; k++)
            {
                symbolToIndex1[p] = notepo[j][k];
                p++;
            }

        }

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReadNextNotePosition();
        }
    }

    private void FixedUpdate()
    {
        tikTime = (stdBPM / musicBPM) * (musicTempo / stdTempo); //노트 출력 속도
        nextTime += Time.deltaTime;

        if (nextTime >= tikTime) // '>='로 변경
        {

            StartCoroutine(PlayTik(tikTime));


            nextTime -= tikTime; // -= 연산 추가
        }


    }


    // 문자열이 숫자인지 확인하는 함수
    bool IsNumber(string str)
    {
        int n;
        return int.TryParse(str, out n);
    }






    IEnumerator PlayTik(float tikTime) //BPM에 따라서 노트 생성기
    {


        Debug.Log("응애 실행중");
        yield return new WaitForSeconds(tikTime);
    }


}
