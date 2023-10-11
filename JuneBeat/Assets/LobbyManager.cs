using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    [Header("표기")]
    public Text Title;
    public Text Artist;
    public Text BPM;
    public Text Notes;
    //public Text Dif;
    public Image Level;
    public Image MusicImage;

    [SerializeField] Sprite[] LobbyLevelImage;


    public GameObject[] ImagePosition;
    public GameObject selectBtn;
    public List<GameObject> Row1;
    public List<GameObject> Row2;
    public List<GameObject> Row3;
    float xOffset = 270.0f;
    float yOffset = 270.0f;

    float LeftX;
    float RightX;


    public TextAsset[] textAssets;

    [SerializeField] GameObject AdvBtn;
    [SerializeField] GameObject BasBtn;
    [SerializeField] GameObject ExtBtn;




    public void Awake()
    {
        if (Instance == null) //정적으로 자신을 체크함, null인진
        {
            Instance = this; //이후 자기 자신을 저장함.
        }
    }
    void Start()
    {
        CreateSongSelection();
        SoundManager.Instance.EffectPlay("selectmusic");
    }

    private void CreateSongSelection()
    {
        GameManager.Instance.ChangeState(CurrentState.LobbySongSelect); // 현재 상태를 LobbySongSelect로 변경

        // "Resources/music" 폴더에서 모든 Texture2D 로드
        Texture2D[] textures = Resources.LoadAll<Texture2D>("music");

        List<Texture2D> textureList = new List<Texture2D>(textures);
        Row1 = new List<GameObject>();
        Row2 = new List<GameObject>();
        Row3 = new List<GameObject>();


        int Row1Count = 0, Row2Count = 0, Row3Count = 0;

        //노래 파일들 생성
        for (int i = 0; i < textures.Length; i++)
        {
            int row = 0;

            // Texture2D를 Sprite로 변환
            Sprite sprite = Sprite.Create(textureList[i], new Rect(0, 0, textureList[i].width, textureList[i].height), new Vector2(0.5f, 0.5f));

            // selectBtn 프리팹을 복제하여 노래 선택 오브젝트 생성
            GameObject selectObject = Instantiate(selectBtn, ImagePosition[0].transform.position, Quaternion.identity);
            selectObject.GetComponent<Image>().sprite = sprite;
            selectObject.transform.SetParent(GameObject.Find("Canvas").transform);



            // 행 (Row) 결정
            //((int)textures.Length/3)
            if (i >= textures.Length / 3)
            {
                row = 1;
            }
            if (i >= (textures.Length / 3) * 2)
            {
                row = 2;
            }

            switch (row)
            {
                case 0:
                    Row1.Add(selectObject);
                    Row1[Row1Count].name = textureList[i].name; // 이름 설정
                    Row1Count++;
                    break;
                case 1:
                    Row2.Add(selectObject);
                    Row2[Row2Count].name = textureList[i].name; // 이름 설정
                    Row2Count++;
                    break;
                case 2:
                    Row3.Add(selectObject);
                    Row3[Row3Count].name = textureList[i].name; // 이름 설정
                    Row3Count++;
                    break;
            }

            // 오브젝트 위치 조정
            selectObject.transform.position += new Vector3(-270, -270 * row, 0);
        }

        // 각 행의 오브젝트 위치 조정
        PositionRows(Row1);
        PositionRows(Row2);
        PositionRows(Row3);

        // 왼쪽 및 오른쪽 끝 위치 저장
        LeftX = Row1[0].transform.localPosition.x;
        RightX = Row1[Row1.Count - 1].transform.localPosition.x + 1;

        // Guid 오브젝트를 가장 위로 이동
        Guid.GetComponent<RectTransform>().SetAsLastSibling();
    }

    // 행의 오브젝트 위치 조정
    private void PositionRows(List<GameObject> row)
    {
        for (int i = 0; i < row.Count; i++)
        {
            row[i].transform.position += new Vector3(xOffset * i, 0, 0);
        }
    }

    [SerializeField] GameObject Guid;
    void Update()
    {

    }
    private void FixedUpdate()
    {

    }




    List<GameObject> DifBtn = new List<GameObject>();
    public void CreatDif()
    {
        GameManager.Instance.ChangeState(CurrentState.LobbyDifficultSelect); // 현재 상태 변경

        // 모든 리스트를 한 번에 처리하고 초기화
        DestroyAllAndClearList(Row1);
        DestroyAllAndClearList(Row2);
        DestroyAllAndClearList(Row3);

        DifBtn = new List<GameObject>();

        // 난이도 버튼을 생성하고 DifBtn 리스트에 추가
        CreateAndAddDifficultyButton(GameManager.Instance.CurrentSongDiffifultList, GameManager.Instance.CurrentSongName + "_basic", BasBtn, ImagePosition[0].transform.position);
        CreateAndAddDifficultyButton(GameManager.Instance.CurrentSongDiffifultList, GameManager.Instance.CurrentSongName + "_advanced", AdvBtn, ImagePosition[1].transform.position);
        CreateAndAddDifficultyButton(GameManager.Instance.CurrentSongDiffifultList, GameManager.Instance.CurrentSongName + "_extreme", ExtBtn, ImagePosition[2].transform.position);

        Guid.GetComponent<RectTransform>().SetAsLastSibling(); // 가이드 위치 조정
    }

    private void DestroyAllAndClearList(List<GameObject> list)
    {
        foreach (var item in list)
        {
            Destroy(item);
        }
        list.Clear();
    }

    private void CreateAndAddDifficultyButton(List<string> difficultyList, string difficultyName, GameObject buttonPrefab, Vector3 position)
    {
        if (difficultyList.Contains(difficultyName))
        {
            GameObject button = Instantiate(buttonPrefab, position, Quaternion.identity);
            button.transform.SetParent(GameObject.Find("Canvas").transform);
            DifBtn.Add(button);
        }
    }
    //public void CreatDif()
    //{



    //    GameManager.Instance.ChangeState(CurrentState.LobbyDifficultSelect); //현재 상태 바꿈
    //    for (int i = 0; i < Row1.Count; i++)
    //    {
    //        Destroy(Row1[i]);
    //    }
    //    for (int i = 0; i < Row2.Count; i++)
    //    {
    //        Destroy(Row2[i]);
    //    }
    //    for (int i = 0; i < Row3.Count; i++)
    //    {
    //        Destroy(Row3[i]);
    //    }

    //    Row1.Clear();
    //    Row2.Clear();
    //    Row3.Clear();

    //    DifBtn = new List<GameObject>();
    //    if (GameManager.Instance.CurrentSongDiffifultList.Contains(GameManager.Instance.CurrentSongName + "_basic"))
    //    {
    //        GameObject basic = Instantiate(BasBtn, ImagePosition[0].transform.position, Quaternion.identity);
    //        basic.transform.SetParent(GameObject.Find("Canvas").transform);
    //        DifBtn.Add(basic);
    //    }
    //    if (GameManager.Instance.CurrentSongDiffifultList.Contains(GameManager.Instance.CurrentSongName + "_advanced"))
    //    {
    //        GameObject advanced = Instantiate(AdvBtn, ImagePosition[1].transform.position, Quaternion.identity);
    //        advanced.transform.SetParent(GameObject.Find("Canvas").transform);
    //        DifBtn.Add(advanced);
    //    }
    //    if (GameManager.Instance.CurrentSongDiffifultList.Contains(GameManager.Instance.CurrentSongName + "_extreme"))
    //    {
    //        GameObject extreme = Instantiate(ExtBtn, ImagePosition[2].transform.position, Quaternion.identity);
    //        extreme.transform.SetParent(GameObject.Find("Canvas").transform);
    //        DifBtn.Add(extreme);
    //    }
    //    Guid.GetComponent<RectTransform>().SetAsLastSibling(); //가이드 두기
    //}
    public bool StopRead;
    int i;
    public void InputSongInfo()
    {
        GameManager.Instance.ReadSongTxt();
        StopRead = true;

        string[] lines = GameManager.Instance.lines;
        i = 0;

        while (StopRead)
        {
            string line = lines[i].Trim(); // 현재 줄을 가져와서 공백 제거

            if (line.StartsWith("#title"))
            {
                GameManager.Instance.Title = line.Substring(8).Trim();
                Title.text = line.Substring(8);
                GameManager.Instance.MusicImage = Resources.Load<Sprite>("music/" + GameManager.Instance.CurrentSongName);
            }
            if (line.StartsWith("#artist"))
            {
                GameManager.Instance.Artist = line.Substring(9).Trim();
                Artist.text = line.Substring(9);
            }
            if (line.StartsWith("#dif"))
            {
                float.TryParse(line.Substring(6), out GameManager.Instance.Dif);

            }
            if (line.StartsWith("prelistening"))
            {
                float.TryParse(line.Substring(14).Trim(), out GameManager.Instance.prelistening) ;

            }
            if (line.StartsWith("Offset"))
            {
                float.TryParse(line.Substring(7).Trim(), out GameManager.Instance.SoundOffset);

            }
            if (line.StartsWith("#lev"))
            {
                float.TryParse(line.Substring(6), out GameManager.Instance.Level);
                Level.sprite = LobbyLevelImage[(int)GameManager.Instance.Level - 1];
            }

            if (line.StartsWith("t"))
            {
                float.TryParse(line.Substring(2), out GameManager.Instance.BPM);
                BPM.text = line.Substring(2);
            }
            if (line.StartsWith("Notes"))
            {
                float.TryParse(line.Substring(6).Trim(), out GameManager.Instance.TotalNote);
                Notes.text = line.Substring(6).Trim();
            }

            i++;

            if (line.Contains("#rasmemo"))
            {
                GameManager.Instance.findrasmemo = i;
                StopRead = false;
            }

        }
    }

    public void Back()
    {
        for (int i = 0; i < DifBtn.Count; i++)
        {
            Destroy(DifBtn[i]);
        }
        if (Row1.Count == 0)
        {
            CreateSongSelection();

        }


    }


    public void GoToInGame()
    {

        if (GameManager.Instance.CurrentSongName != null && GameManager.Instance.CurrentDifficult != null)
        {

            SceneManager.LoadScene("InGame");
        }

    }




    //좌우 스크롤 친구들
    private float moveDuration = 0.1f; // 이동에 걸리는 시간 (조절 가능)
    private bool isMoving = false; // 이동 중 여부를 나타내는 변수
    public void RightMove()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveRight(Row1));
            StartCoroutine(MoveRight(Row2));
            StartCoroutine(MoveRight(Row3));
        }
    }

    public void LeftMove()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveRowLeft(Row1));
            StartCoroutine(MoveRowLeft(Row2));
            StartCoroutine(MoveRowLeft(Row3));
        }
    }




    //오른쪽 스크롤 코루틴
    private IEnumerator MoveRight(List<GameObject> row)
    {
        isMoving = true; // 이동 중 플래그 설정

        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        // 시작 위치와 목표 위치 저장
        foreach (GameObject item in row)
        {
            startPositions.Add(item.transform.localPosition); // 시작 위치 저장
            endPositions.Add(item.transform.localPosition + new Vector3(xOffset, 0, 0)); // 목표 위치 저장
        }

        float elapsedTime = 0f;

        // 이동 시간동안 반복
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration; // 보간값 계산

            // 모든 객체에 대해 현재 위치를 보간하여 설정
            for (int i = 0; i < row.Count; i++)
            {
                row[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }

            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 한 프레임을 기다립니다.
        }

        // 이동이 끝난 후 최종 위치 설정
        for (int i = 0; i < row.Count; i++)
        {
            row[i].transform.localPosition = endPositions[i];

            // 오른쪽 끝으로 넘어간 객체 처리
            if (row[i].transform.localPosition.x > RightX)
            {
                row[i].transform.localPosition -= new Vector3(xOffset * row.Count, 0, 0);
            }
        }

        isMoving = false; // 이동 종료 후 플래그 해제
    }


    //왼쪽 스크롤 코루틴
    private IEnumerator MoveRowLeft(List<GameObject> row)
    {
        isMoving = true; // 이동 중 플래그 설정

        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        // 시작 위치와 목표 위치 저장
        foreach (GameObject item in row)
        {
            startPositions.Add(item.transform.localPosition); // 시작 위치 저장
            endPositions.Add(item.transform.localPosition - new Vector3(xOffset, 0, 0)); // 목표 위치 저장 (왼쪽으로 이동)
        }

        float elapsedTime = 0f;

        // 이동 시간 동안 반복
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration; // 보간값 계산

            // 모든 객체에 대해 현재 위치를 보간하여 설정
            for (int i = 0; i < row.Count; i++)
            {
                row[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }

            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 한 프레임을 기다립니다.
        }

        // 이동이 끝난 후 최종 위치 설정
        for (int i = 0; i < row.Count; i++)
        {
            row[i].transform.localPosition = endPositions[i];

            // 왼쪽 끝으로 넘어간 객체 처리
            if (row[i].transform.localPosition.x < LeftX)
            {
                row[i].transform.localPosition += new Vector3(xOffset * row.Count, 0, 0);
            }
        }

        isMoving = false; // 이동 종료 후 플래그 해제
    }




}
