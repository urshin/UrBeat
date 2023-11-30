using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Profiling.Editor;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class NewParsing : MonoBehaviour
{
    public string filePath;
    public TextAsset textAsset;
    public string[] lines;

    public bool isSongEnd;




    [SerializeField] GameObject Note;
    [SerializeField] Transform[] NotePositionArray;

    public Dictionary<int, char> symbolToIndex1 = new Dictionary<int, char>
    {
         {0, ' '},  {1, ' '},  {2, ' '},  {3, ' '},
         {4, ' '},  {5, ' '},  {6, ' '},  {7, ' '},
         {8, ' '},  {9, ' '},  {10, ' '}, {11, ' '},
         {12, ' '}, {13, ' '}, {14, ' '}, {15, ' '},
    };
    public Dictionary<int, char> symbolToIndex2 = new Dictionary<int, char>
    {
        {0, ' '},  {1, ' '},  {2, ' '},  {3, ' '},
         {4, ' '},  {5, ' '},  {6, ' '},  {7, ' '},
         {8, ' '},  {9, ' '},  {10, ' '}, {11, ' '},
         {12, ' '}, {13, ' '}, {14, ' '}, {15, ' '},

         {0+16, ' '},  {1+16, ' '},  {2+16, ' '},  {3+16, ' '},
         {4+16, ' '},  {5+16, ' '},  {6+16, ' '},  {7+16, ' '},
         {8+16, ' '},  {9+16, ' '},  {10+16, ' '}, {11+16, ' '},
         {12+16, ' '}, {13+16, ' '}, {14+16, ' '}, {15+16, ' '},

         {0+32, ' '},  {1+32, ' '},  {2+32, ' '},  {3+32, ' '},
         {4+32, ' '},  {5+32, ' '},  {6+32, ' '},  {7+32, ' '},
         {8+32, ' '},  {9+32, ' '},  {10+32, ' '}, {11+32, ' '},
         {12+32, ' '}, {13+32, ' '}, {14+32, ' '}, {15+32, ' '},

         {0+48, ' '},  {1+48, ' '},  {2+48, ' '},  {3+48, ' '},
         {4+48, ' '},  {5+48, ' '},  {6+48, ' '},  {7+48, ' '},
         {8+48, ' '},  {9+48, ' '},  {10+48, ' '}, {11+48, ' '},
         {12+48, ' '}, {13+48, ' '}, {14+48, ' '}, {15+48, ' '},
    };


    [Header("BPM관련")]
    public float musicBPM = 60f;
    public float stdBPM = 60;
    public float musicTempo = 4;
    public float stdTempo = 4;
    float tikTime = 0;
    float nextTime = 0;

    private void Start()
    {
        GameManager.Instance.currentState = CurrentState.Ingame;
        //게임 매니저에게 현재 곡정보. 알려주기
        //GameManager.Instance.CurrentSongAndDiff = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult;
        isSongEnd = false;
        SoundManager.Instance.StopBGM();
        songStart = true;
        ShowInfoSong();
        ReadSongTxt();
        PositionTimingParsing();


    }

    private void ShowInfoSong()
    {
        musicBPM = GameManager.Instance.BPM; //음악 BPM 넣기
        LineNum = (int)GameManager.Instance.findrasmemo;
        currentNoteCount = 0;
    }

    [SerializeField] int lineCount;
    public void ReadSongTxt()
    {
        filePath = "music/" + GameManager.Instance.CurrentSongAndDiff;
        textAsset = new TextAsset();
        textAsset = Resources.Load<TextAsset>(filePath);
        Debug.Log(filePath);
        lines = textAsset.text.Split('\n');
        lineCount = textAsset.text.Length; //줄을 추가하여 줄의 수 구함
        ReadNotePositonAll();//전체 포지션 읽기.

    }

    private void ReadNextNotePosition() //다음줄 읽기 함수
    {
        GoNextPositionParsing = true;
        PositionTimingParsing();
    }

    public List<string> NotePosision = new List<string>(); //노트 포지션
    public List<string> NoteTimeing = new List<string>(); //노트 타이밍
    [SerializeField] int LineNum; //라인 넘버
    bool GoNextPositionParsing = true; // 다음줄로 넘어갈지?


    private void PositionTimingParsing() //노트 포지션 읽어오기
    {
        NotePosision = new List<string>(); //리스트 생성
        NoteTimeing = new List<string>();
        while (GoNextPositionParsing && LineNum < lines.Length)
        {

            if (IsNumber(lines[LineNum]) || string.IsNullOrWhiteSpace(lines[LineNum])) //읽은 라인이 숫자면
            {
            

                GoNextPositionParsing = false;
            }
            if (!IsNumber(lines[LineNum]) && lines[LineNum].Length >= 4) //읽은 라인이 숫자가 아니고 4자리 이상이라면
            {
                NotePosision.Add(lines[LineNum].Substring(0, 4));
                if (lines[LineNum].Length >= 6)
                {
                    NoteTimeing.Add(lines[LineNum].Substring(6, lines[LineNum].LastIndexOf('|') - 6)); 
                  
                }
            }
            LineNum++; //다음줄
        }
    }
    public List<string> allNotePosition = new List<string>();
    [SerializeField] int allLineNum;

    void ReadNotePositonAll()
    {
        int maxCombo = 0; // MaxCombo를 로컬 변수로 변경
        allLineNum = (int)GameManager.Instance.findrasmemo;
        allNotePosition.Clear(); // 리스트 초기화

        while (allLineNum < lines.Length) // '<=' 대신 '<'를 사용하여 배열 경계를 초과하지 않도록 합니다.
        {
            if (!IsNumber(lines[allLineNum]) && lines[allLineNum].Length >= 4)
            {
                string notePosition = lines[allLineNum].Substring(0, 4);
                allNotePosition.Add(notePosition);

                foreach (char character in notePosition)
                {
                    // 불필요한 문자를 검사하지 않고, 문자 수만 세도록 변경
                    if (character != '＜' && character != '＞' && character != '∨' && character != '∧' && character != '―' && character != '｜' && character != '口' && character != '┼')
                    {
                        maxCombo++;
                    }
                }
            }

            allLineNum++; // 다음 줄로 이동
        }

        GameManager.Instance.MaxComboNum = maxCombo;
    }


    // public List<string> allNotePosition = new List<string>();
    // [SerializeField] int allLineNum;
    //public int MaxCombo;
    // void ReadNotePositonAll()
    // {
    //     MaxCombo = 0;
    //     allLineNum = (int)GameManager.Instance.findrasmemo;
    //     allNotePosition = new List<string>();
    //     while (allLineNum <= lines.Length)
    //     {
    //         if (IsNumber(lines[allLineNum])) //읽은 라인이 숫자면
    //         {
    //             //NotePosision = new List<string>();

    //         }
    //         if (!IsNumber(lines[allLineNum]) && lines[allLineNum].Length >= 4) //읽은 라인이 숫자가 아니고 4자리 이상이라면
    //         {
    //             allNotePosition.Add(lines[allLineNum].Substring(0, 4));
    //            for(int i =0; i < lines[allLineNum].Substring(0, 4).Length;  i++)
    //             {
    //                 char charactor = lines[allLineNum][i];
    //                 if( charactor != '＜' && charactor != '＞' && charactor != '∨' && charactor != '∧' && charactor != '―' &&charactor != '｜' && charactor != '口' && charactor != '┼')
    //                 {
    //                     MaxCombo++;
    //                 }
    //             }
    //         }
    //         allLineNum++; //다음줄

    //     }
    //     GameManager.Instance.MaxComboNum = MaxCombo;
    // }



    public void mappingNote1(List<string> notepo) //딕셔너리와 연동하는 함수
    {
        symbolToIndex1.Clear();
        int p = 0;
        for (int j = 0; j < notepo.Count; j++)
        {
            for (int k = 0; k < notepo[j].Length; k++)
            {
                symbolToIndex1[p] = notepo[j][k];
                p++;
            }

        }

    }
    public void mappingNote(List<string> notepo)
    {

        symbolToIndex1.Clear();
        // int p = 0;
        for (int j = 0; j < notepo.Count; j++)
        {
            for (int k = 0; k < notepo[j].Length; k++)
            {
                int key = j * notepo[j].Length + k; // key를 j*k로 설정
                char value = notepo[j][k]; // 문자를 리스트의 문자로 설정
                symbolToIndex1.Add(key, value);
                //symbolToIndex1[key] = value;
            }
        }
    }



    public List<char> ccharactor = new List<char>();

    bool songStart;
    private void Update()
    {
        if (TimerEnd && songStart)
        {
            SoundManager.Instance.SongPlay();
            songStart = false;
        }
    }

    private void ListingNoteTiming()
    {
        ccharactor = null;
        ccharactor = new List<char>();
        for (int j = 0; j < NoteTimeing.Count; j++)
        {
            for (int i = 0; i < NoteTimeing[j].Length; i++)
            {
                char character = NoteTimeing[j][i];
                ccharactor.Add(NoteTimeing[j][i]);
                //Debug.Log(character.ToString());
            }

        }
    }

    private float countdown = 3.0f; // 3초 타이머 설정
    private float timer = 0.0f;
    [SerializeField] Sprite[] ReadyGo;
    [SerializeField] Image MarkPositionSprite;

    public bool TimerEnd = false;

    //void Timer()
    //{
    //    timer += Time.deltaTime; // 프레임 간격에 따른 시간 업데이트

    //    if (timer >= 1.0f) // 1초마다
    //    {
    //        timer = 0.0f; // 타이머 초기화
    //        countdown--; // 남은 시간 감소
    //        Debug.Log("남은 시간: " + countdown + "초"); // 디버그 로그 출력

    //        if (countdown == 1)
    //        {
    //            Debug.Log("Ready");
    //            MarkPositionSprite.sprite = ReadyGo[0];
    //        }
    //        else if (countdown == 0)
    //        {
    //            Debug.Log("Go");
    //            MarkPositionSprite.sprite = ReadyGo[1];
    //        }

    //        if (countdown <= 0) // 타이머가 0 이하로 내려가면
    //        {
    //            Debug.Log("타이머 종료");
    //            TimerEnd = true;

    //            // MarkPositionSprite를 투명하게 만들기
    //            Color spriteColor = MarkPositionSprite.color;
    //            spriteColor.a = 0.0f; // 알파값을 0으로 설정하여 투명하게 만듭니다.
    //            MarkPositionSprite.color = spriteColor;
    //        }
    //        else
    //        {
    //            // 타이머가 0이 아닌 경우에 MarkPositionSprite를 보이게 만들기
    //            Color spriteColor = MarkPositionSprite.color;
    //            spriteColor.a = 1.0f; // 알파값을 1로 설정하여 보이게 만듭니다.
    //            MarkPositionSprite.color = spriteColor;
    //        }
    //    }
    //}
    void Timer()
    {
        timer += Time.deltaTime; // 프레임 간격에 따른 시간 업데이트

        if (timer >= 1.0f) // 1초마다
        {
            timer = 0.0f; // 타이머 초기화
            countdown--; // 남은 시간 감소
            Debug.Log("남은 시간: " + countdown + "초"); // 디버그 로그 출력

            if (countdown >= 1)
            {
                //MarkPositionSprite.sprite = ReadyGo[(int)countdown - 1];
                if (countdown == 2)
                {
                    Debug.Log("Ready");
                    SoundManager.Instance.EffectPlay("voice_ready");
                    MarkPositionSprite.sprite = ReadyGo[(int)countdown - 1];
                }
                else if (countdown == 1)
                {
                    Debug.Log("Go");
                    SoundManager.Instance.EffectPlay("voice_go");

                    MarkPositionSprite.sprite = ReadyGo[(int)countdown - 1];
                }
            }

            if (countdown <= 0) // 타이머가 0 이하로 내려가면
            {
                Debug.Log("타이머 종료");
                TimerEnd = true;

                // MarkPositionSprite를 투명하게 만들기
                Color spriteColor = MarkPositionSprite.color;
                spriteColor.a = 0.0f; // 알파값을 0으로 설정하여 투명하게 만듭니다.
                MarkPositionSprite.color = spriteColor;
            }
            else
            {
                // 타이머가 0이 아닌 경우에 MarkPositionSprite를 보이게 만들기
                Color spriteColor = MarkPositionSprite.color;
                spriteColor.a = 1.0f; // 알파값을 1로 설정하여 보이게 만듭니다.
                MarkPositionSprite.color = spriteColor;
            }
        }
    }

    //[SerializeField] int NoteCounting;
    private void FixedUpdate()
    {
        if (GameManager.Instance.currentState == CurrentState.Ingame)
        {
            // 노트 출력 속도 계산
            tikTime = (stdBPM / musicBPM) * (musicTempo / stdTempo);
            nextTime += Time.deltaTime;

            // 타이머가 종료되지 않았을 때만 타이머 실행
            if (!TimerEnd)
            {
                Timer();
            }

            // 주어진 시간 간격마다 노트를 출력
            if (nextTime >= tikTime)
            {
                if (countNoteTiming > ccharactor.Count - 1)
                {
                    ReadNextNotePosition();
                    mappingNote(NotePosision);
                    ListingNoteTiming();
                    //Debug.Log(NotePosision.Count);
                    countNoteTiming = 0;

                    //symbolToIndex2.Clear();
                    //// Dictionary의 모든 엔트리를 순회하면서 원하는 Value를 가진 Key 찾기
                    //foreach (var kvp in symbolToIndex1)
                    //{
                    //    if (kvp.Value == '＜')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //    if (kvp.Value == '＞')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //    if (kvp.Value == '∨')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //    if (kvp.Value == '∧')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //}


                    //foreach (var kvp in symbolToIndex1)
                    //{
                    //    if (kvp.Value == '―')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //    if (kvp.Value == '｜')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //}

                    // 찾은 모든 Key를 디버그 로그로 출력
                    //foreach (var key in symbolToIndex2)
                    //{
                    //    Debug.Log("롱노트 시작   " + key.Key + "에 들어가 있는거 " + key.Value);
                    //    if (key.Value == '＜')
                    //    {
                    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
                    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 270f);
                    //    }
                    //    if (key.Value == '＞')
                    //    {
                    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
                    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                    //    }
                    //    if (key.Value == '∨')
                    //    {
                    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
                    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    //    }
                    //    if (key.Value == '∧')
                    //    {
                    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
                    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
                    //    }

                    //}

                }
                // 노트 출력 중인지와 타이머가 종료되었는지 확인
                if (!isSongEnd && TimerEnd&&ccharactor.Count >0)
                {
                    StartCoroutine(PlayTik(tikTime));
                }

                nextTime -= tikTime; // 다음 노트까지의 시간 계산
            }

        }

        // 노래가 종료되고 게임 상태가 Ingame일 때 결과 화면으로 전환
        if (isSongEnd && GameManager.Instance.currentState == CurrentState.Ingame)
        {
            //StartCoroutine(PlayTik(tikTime));
            GameManager.Instance.currentState = CurrentState.result;
        }
    }

    // 문자열이 숫자인지 확인하는 함수
    bool IsNumber(string str)
    {
        int n;
        return int.TryParse(str, out n);
    }


    public int currentNoteCount;


    public int countNoteTiming = 0;

    public GameObject LongNote;

    List<int> LongNoteOrder = new List<int>(); //롱노트 순서
    public char LongNoteTimingCharactor;


    IEnumerator PlayTik(float tikTime) //BPM에 따라서 노트 생성기
    {

        //if (countNoteTiming > ccharactor.Count - 1)
        //{
        //    ReadNextNotePosition();
        //    mappingNote(NotePosision);
        //    ListingNoteTiming();
        //    //Debug.Log(NotePosision.Count);
        //    countNoteTiming = 0;

        //    //symbolToIndex2.Clear();
        //    //// Dictionary의 모든 엔트리를 순회하면서 원하는 Value를 가진 Key 찾기
        //    //foreach (var kvp in symbolToIndex1)
        //    //{
        //    //    if (kvp.Value == '＜')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //    if (kvp.Value == '＞')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //    if (kvp.Value == '∨')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //    if (kvp.Value == '∧')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //}


        //    //foreach (var kvp in symbolToIndex1)
        //    //{
        //    //    if (kvp.Value == '―')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //    if (kvp.Value == '｜')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //}

        //    // 찾은 모든 Key를 디버그 로그로 출력
        //    //foreach (var key in symbolToIndex2)
        //    //{
        //    //    Debug.Log("롱노트 시작   " + key.Key + "에 들어가 있는거 " + key.Value);
        //    //    if (key.Value == '＜')
        //    //    {
        //    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
        //    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 270f);
        //    //    }
        //    //    if (key.Value == '＞')
        //    //    {
        //    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
        //    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        //    //    }
        //    //    if (key.Value == '∨')
        //    //    {
        //    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
        //    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        //    //    }
        //    //    if (key.Value == '∧')
        //    //    {
        //    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
        //    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
        //    //    }

        //    //}

        //}


        if (symbolToIndex1.ContainsValue(ccharactor[countNoteTiming]) && ccharactor.Count > 0)
        {
            foreach (var kvp in symbolToIndex1)
            {

                // 아마 이 타이밍에 롱노트 생성 그거 넣으면 될듯
                if (kvp.Value == ccharactor[countNoteTiming])
                {
                    // int p = kvp.Key;

                    //if (p >= 48) // kvp.Key 값이 48 이상일 때
                    //{
                    //    p -= 48;
                    //}
                    //if (p >= 32) // kvp.Key 값이 32 이상일 때
                    //{
                    //    p -= 32;
                    //}
                    //if (p >= 16) // kvp.Key 값이 16 이상일 때
                    //{
                    //    p -= 16;
                    //}
                    int p = kvp.Key % NotePositionArray.Length; //원래는 p 값을 수정했지만 p값을 배열의 길이로 나눈 나머지를 사용하여 배열 범위 내로 조정함



                    Instantiate(Note, NotePositionArray[p]); // 노트 생성
                    currentNoteCount++;
                    // Instantiate(Note, NotePositionArray[p]); //노트 생성
                    // currentNoteCount++;
                }
            }
            if (currentNoteCount >= GameManager.Instance.TotalNote)
            {
                isSongEnd = true;
                Debug.Log("노래 끝났다 나가라");
            }

        }
        countNoteTiming++;

        yield return new WaitForSeconds(tikTime);
    }

}
