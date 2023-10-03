using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParsingData : MonoBehaviour
{

    [SerializeField] Text Title;
    [SerializeField] Text Artist;
    [SerializeField] Text BPM;
    [SerializeField] Text Notes;
    [SerializeField] Text Dif;
    [SerializeField] Text Level;
    [SerializeField] Image MusicImage;

    public string filePath;
    public TextAsset textAsset;
    public string[] lines;


    private void Start()
    {
        //게임 매니저에게 현재 곡정보. 알려주기
        GameManager.Instance.CurrentSongAndDiff = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult;

        ShowInfoSong();
        //이전 코드것들
        // filePath = "music/" + SongName + "_" + SongDif;
        ReadSongTxt();
        
        NoteData();


    }




    public Dictionary<int, char> symbolToIndex = new Dictionary<int, char>
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

    [Header("곡")]
    [SerializeField] List<List<string>> groupedLines = new List<List<string>>(); // 줄 그룹을 저장할 리스트
    [SerializeField] List<string> currentGroup = new List<string>(); // 현재 그룹의 줄을 저장할 리스트
    [SerializeField] Transform notePosion; //노트 생성 위치
    [SerializeField] GameObject note; //노트 프리 팹
    [SerializeField] Transform[] NoteArray;//노트 생성 위치값을 가지고 있는 배열
    [SerializeField] List<string> TimePosition = new List<string>();
    [SerializeField] List<string> notePosition = new List<string>(); //노트 포지션
    [SerializeField] List<string> noteTiming = new List<string>();//노트 타이밍
    int pipeCount = 0; //노트 생성 타이밍 구분하기 위한 기호 셈.
    int pipepipeCount = 0; //노트 생성 타이밍 구분하기 위한 기호 셈.
    float timer = 0f; // 타이머 변수
    int currentSymbolIndex = 0; // 현재 출력 중인 심볼 인덱스
    int currentCharacterIndex = 0;
    int j = 0;
    [SerializeField] int CurrentNoteCount;
    public int nextLine;


    private void ShowInfoSong()
    {
        Title.text = GameManager.Instance.Title;
        Artist.text = GameManager.Instance.Artist;
        BPM.text = GameManager.Instance.BPM.ToString();
        Notes.text = GameManager.Instance.Notes.ToString();
        MusicImage.sprite = GameManager.Instance.MusicImage.sprite;
        musicBPM = GameManager.Instance.BPM; //음악 BPM 넣기
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





    public void mappingNote(List<string> notepo) //딕셔너리와 연동하는 함수
    {
        symbolToIndex.Clear();
        int p = 0;
        for (int j = 0; j < 4; j++)
        {
            for (int k = 0; k < 4; k++)
            {
                symbolToIndex[p] = notepo[j][k];
                p++;
            }

        }

    }





    public void NoteData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        lines = textAsset.text.Split('\n');
        Debug.Log(lines[0]);

        CurrentNoteCount = 0;

        for (int i = 0; i <= lines.Length - 1; i++)
        {
            string line = lines[i].Trim(); // 현재 줄을 가져와서 공백 제거

            if (pipeCount == 4)
            {
                groupedLines.Add(currentGroup);
                currentGroup = new List<string>();
                notePosition = new List<string>();
                pipeCount = 0;

            }
            currentGroup.Add(line); // 현재 줄을 현재 그룹에 추가

            if (!IsNumber(line) && line.Length >= 4)
            {
                if (currentGroup.Count > 0)
                {

                    string firstFourCharacters = line.Substring(0, 4); //문자열이 4이상인 라인에서 0부터 3번까지 가져오기

                    notePosition.Add(firstFourCharacters);
                    // 노트 타이밍 정보를 추출하여 noteTiming 리스트에 추가해야 함
                    string timingInfo = line.Substring(4); // 예시: 1234567890 -> "567890"

                    if (line.Contains("|")) // | 기호가 포함된 줄인 경우에만 처리
                    {
                        pipeCount++;

                        string[] splitParts = line.Split('|');
                        string symbol = splitParts[1].Trim();
                        string beforesymbol = splitParts[0].Trim();
                        noteTiming.Add(symbol);
                    }
                }
            }


        }
    }



    private void FixedUpdate()
    {
        tikTime = (stdBPM / musicBPM) * (musicTempo / stdTempo); //노트 출력 속도
        nextTime += Time.deltaTime;

        if (nextTime >= tikTime) // '>='로 변경
        {
            if (CurrentNoteCount <= GameManager.Instance.TotalNote)
            {
                StartCoroutine(PlayTik(tikTime));

            }
            nextTime -= tikTime; // -= 연산 추가
        }
    }


    // 문자열이 숫자인지 확인하는 함수
    bool IsNumber(string str)
    {
        int n;
        return int.TryParse(str, out n);
    }



    public int noteCount;
    void timepositions()
    {
     
        while (pipepipeCount < 4)
        {

            string line = lines[j].Trim(); // 현재 줄을 가져와서 공백 제거

            if (IsNumber(line))
            {
                TimePosition = new List<string>();

            }
            if (!IsNumber(line) && line.Length >= 4)
            {
                string firstFourCharacters = line.Substring(0, 4); //문자열이 4이상인 라인에서 0부터 3번까지 가져오기

                TimePosition.Add(firstFourCharacters);
                if (line.Contains("|")) //  기호가 포함된 줄인 경우에만 처리
                {
                    pipepipeCount++;
                }
            }
            j++;
            CurrentNoteCount++;
        }



    }




    IEnumerator PlayTik(float tikTime) //BPM에 따라서 노트 생성기
    {

        if (noteCount == 0) //노트 카운트가 0일때 다음 노트 읽어오고 채보 보기
        {
            timepositions();


        }
        if (TimePosition.Count >= 8) // TimePosition 리스트 크기가 4보다 클 경우
        {
            int range = TimePosition.Count / 2;

            if (noteCount < 8)
                mappingNote(TimePosition.GetRange(0, range));
            if (noteCount > 7)
                mappingNote(TimePosition.GetRange(range, 4));
        }
        else
        {
            mappingNote(TimePosition);


        }

        if (currentCharacterIndex < noteTiming[currentSymbolIndex].Length)
        {
            char character = noteTiming[currentSymbolIndex][currentCharacterIndex];


            if (character == '－')
            {
                noteCount++;
            }
            else
            {
                noteCount++;
                if (symbolToIndex.ContainsValue(character))
                {
                    foreach (var kvp in symbolToIndex)
                    {
                        if (kvp.Value == character)
                        {
                            //  Debug.Log(kvp.Key);
                            Instantiate(note, NoteArray[kvp.Key]);
                        }
                    }

                }


            }
            currentCharacterIndex++;

        }
        if (currentCharacterIndex == 4 && currentSymbolIndex < noteTiming.Count + 1)
        {
            currentSymbolIndex++;
            currentCharacterIndex = 0;
        }
        if (noteCount > 15)
        {
            pipepipeCount = 0;
            noteCount = 0;
        }

        yield return new WaitForSeconds(tikTime);
    }


}
