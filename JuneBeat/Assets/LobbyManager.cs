using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class LobbyManager : MonoBehaviour
{
    [Header("ǥ��")]
    public Text Title;
    public Text Artist;
    public Text BPM;
    public Text Notes;
    //public Text Dif;
    //public Text Level;
    public Image MusicImage;

    public static LobbyManager Instance;
    public GameObject[] ImagePosition;
    public GameObject selectBtn;
    public List<GameObject> Row1;
    public List<GameObject> Row2;
    public List<GameObject> Row3;
    float xOffset = 270.0f;

    float LeftX;
    float RightX;


    public TextAsset[] textAssets;

    [SerializeField] GameObject AdvBtn;
    [SerializeField] GameObject BasBtn;
    [SerializeField] GameObject ExtBtn;

    public void Awake()
    {
        if (Instance == null) //�������� �ڽ��� üũ��, null����
        {
            Instance = this; //���� �ڱ� �ڽ��� ������.
        }
    }
    void Start()
    {
        CreatSongSelection();
    }

    private void CreatSongSelection()
    {
        GameManager.Instance.ChangeState(CurrentState.LobbySongSelect); //���� ���� �ٲ�
        // "Resources/music" ������ ��� Texture2D �ε�
        Texture2D[] textures = Resources.LoadAll<Texture2D>("music");

        List<Texture2D> textureList = new List<Texture2D>(textures);
        Row1 = new List<GameObject>();
        Row2 = new List<GameObject>();
        Row3 = new List<GameObject>();
        int Row1Count = 0, Row2Count = 0, Row3Count = 0;
        //�뷡 ���ϵ� ����
        for (int i = 0; i < textures.Length; i++)
        {


            int p = 0;
            Sprite sprite = Sprite.Create(textureList[i], new Rect(0, 0, textureList[i].width, textureList[i].height), new Vector2(0.5f, 0.5f));
            GameObject selectObject = Instantiate(selectBtn, ImagePosition[0].transform.position, Quaternion.identity);
            selectObject.GetComponent<Image>().sprite = sprite;
            //selectObject.transform.parent = GameObject.Find("Canvas").transform.GetChild(0);
            selectObject.transform.SetParent(GameObject.Find("Canvas").transform);

            if (i >= textures.Length / 3 + 1 && i < 1 + (textures.Length - (textures.Length / 3)))
            {
                p = 1;
            }
            if (i >= (textures.Length - (textures.Length / 3)))
            {
                p = 2;
            }

            switch (p)
            {
                case 0:
                    Row1.Add(selectObject);
                    Row1[Row1Count].name = textureList[i].name; //�̸� �����ֱ�
                    Row1Count++;
                    break;
                case 1:
                    Row2.Add(selectObject);
                    Row2[Row2Count].name = textureList[i].name;//�̸� �����ֱ�
                    Row2Count++;
                    break;
                case 2:
                    Row3.Add(selectObject);
                    Row3[Row3Count].name = textureList[i].name;//�̸� �����ֱ�
                    Row3Count++;
                    break;

            }
            selectObject.transform.position += new Vector3(-270, -270 * p, 0);
        }

        for (int i = 0; i < Row1.Count; i++)
        {
            Row1[i].transform.position += new Vector3(xOffset * i, 0, 0);

        }
        for (int i = 0; i < Row2.Count; i++)
        {
            Row2[i].transform.position += new Vector3(xOffset * i, 0, 0);

        }
        for (int i = 0; i < Row3.Count; i++)
        {
            Row3[i].transform.position += new Vector3(xOffset * i, 0, 0);

        }

        LeftX = Row1[0].transform.localPosition.x;
        RightX = Row1[Row1.Count - 1].transform.localPosition.x + 1;
    }



    void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    List<GameObject> DifBtn = new List<GameObject>();
    public void CreatDif()
    {
        GameManager.Instance.ChangeState(CurrentState.LobbyDifficultSelect); //���� ���� �ٲ�
        for (int i = 0; i < Row1.Count; i++)
        {
            Destroy(Row1[i]);
        }
        for (int i = 0; i < Row2.Count; i++)
        {
            Destroy(Row2[i]);
        }
        for (int i = 0; i < Row3.Count; i++)
        {
            Destroy(Row3[i]);
        }

        Row1.Clear();
        Row2.Clear();
        Row3.Clear();

        DifBtn = new List<GameObject>();
        if (GameManager.Instance.CurrentSongDiffifultList.Contains(GameManager.Instance.CurrentSongName + "_basic"))
        {
            GameObject basic = Instantiate(BasBtn, ImagePosition[0].transform.position, Quaternion.identity);
            basic.transform.SetParent(GameObject.Find("Canvas").transform);
            DifBtn.Add(basic);
        }
        if (GameManager.Instance.CurrentSongDiffifultList.Contains(GameManager.Instance.CurrentSongName + "_advanced"))
        {
            GameObject advanced = Instantiate(AdvBtn, ImagePosition[1].transform.position, Quaternion.identity);
            advanced.transform.SetParent(GameObject.Find("Canvas").transform);
            DifBtn.Add(advanced);
        }
        if (GameManager.Instance.CurrentSongDiffifultList.Contains(GameManager.Instance.CurrentSongName + "_extreme"))
        {
            GameObject extreme = Instantiate(ExtBtn, ImagePosition[2].transform.position, Quaternion.identity);
            extreme.transform.SetParent(GameObject.Find("Canvas").transform);
            DifBtn.Add(extreme);
        }
    }
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
            string line = lines[i].Trim(); // ���� ���� �����ͼ� ���� ����

            if (line.StartsWith("#title"))
            {
                GameManager.Instance.Title = line.Substring(8).Trim();
                Title.text = line.Substring(8);
               // GameManager.Instance.MusicImage.sprite = Resources.Load<Sprite>("music/" + GameManager.Instance.CurrentSongName);
            }
            if (line.StartsWith("#artist"))
            {
                GameManager.Instance.Artist = line.Substring(9).Trim();
                Artist.text = line.Substring(9);
            }
            if (line.StartsWith("#dif"))
            {
                float.TryParse(line.Substring(6), out GameManager.Instance.Dif);

                //patternData.Instance.dif = float.Parse(line.Substring(5));
            }
            if (line.StartsWith("#lev"))
            {
                float.TryParse(line.Substring(6), out GameManager.Instance.Level);
                // patternData.Instance.lev = float.Parse(line.Substring(5));
            }

            if (line.StartsWith("t"))
            {
                float.TryParse(line.Substring(2), out GameManager.Instance.BPM);
                BPM.text = line.Substring(2);
            }
            if (line.StartsWith("r"))
            {
                float.TryParse(line.Substring(2), out GameManager.Instance.TotalNote);
                Notes.text = line.Substring(2);
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
            CreatSongSelection();
        }

    }


    public void GoToInGame()
    {

        if (GameManager.Instance.CurrentSongName != null && GameManager.Instance.CurrentDifficult!=null)
        {

            SceneManager.LoadScene("InGame");
        }

    }




    //�¿� ��ũ�� ģ����
    private float moveDuration = 0.1f; // �̵��� �ɸ��� �ð� (���� ����)
    private bool isMoving = false; // �̵� �� ���θ� ��Ÿ���� ����
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
 

    //������ ��ũ�� �ڷ�ƾ
    private IEnumerator MoveRight(List<GameObject> row)
    {
        isMoving = true; // �̵� �� �÷��� ����

        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        // ���� ��ġ�� ��ǥ ��ġ ����
        foreach (GameObject item in row)
        {
            startPositions.Add(item.transform.localPosition); // ���� ��ġ ����
            endPositions.Add(item.transform.localPosition + new Vector3(xOffset, 0, 0)); // ��ǥ ��ġ ����
        }

        float elapsedTime = 0f;

        // �̵� �ð����� �ݺ�
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration; // ������ ���

            // ��� ��ü�� ���� ���� ��ġ�� �����Ͽ� ����
            for (int i = 0; i < row.Count; i++)
            {
                row[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }

            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // �� �������� ��ٸ��ϴ�.
        }

        // �̵��� ���� �� ���� ��ġ ����
        for (int i = 0; i < row.Count; i++)
        {
            row[i].transform.localPosition = endPositions[i];

            // ������ ������ �Ѿ ��ü ó��
            if (row[i].transform.localPosition.x > RightX)
            {
                row[i].transform.localPosition -= new Vector3(xOffset * row.Count, 0, 0);
            }
        }

        isMoving = false; // �̵� ���� �� �÷��� ����
    }


    //���� ��ũ�� �ڷ�ƾ
    private IEnumerator MoveRowLeft(List<GameObject> row)
    {
        isMoving = true; // �̵� �� �÷��� ����

        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();

        // ���� ��ġ�� ��ǥ ��ġ ����
        foreach (GameObject item in row)
        {
            startPositions.Add(item.transform.localPosition); // ���� ��ġ ����
            endPositions.Add(item.transform.localPosition - new Vector3(xOffset, 0, 0)); // ��ǥ ��ġ ���� (�������� �̵�)
        }

        float elapsedTime = 0f;

        // �̵� �ð� ���� �ݺ�
        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration; // ������ ���

            // ��� ��ü�� ���� ���� ��ġ�� �����Ͽ� ����
            for (int i = 0; i < row.Count; i++)
            {
                row[i].transform.localPosition = Vector3.Lerp(startPositions[i], endPositions[i], t);
            }

            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // �� �������� ��ٸ��ϴ�.
        }

        // �̵��� ���� �� ���� ��ġ ����
        for (int i = 0; i < row.Count; i++)
        {
            row[i].transform.localPosition = endPositions[i];

            // ���� ������ �Ѿ ��ü ó��
            if (row[i].transform.localPosition.x < LeftX)
            {
                row[i].transform.localPosition += new Vector3(xOffset * row.Count, 0, 0);
            }
        }

        isMoving = false; // �̵� ���� �� �÷��� ����
    }




}
