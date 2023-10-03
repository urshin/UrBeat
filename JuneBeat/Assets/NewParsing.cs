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
        //���� �Ŵ������� ���� ������. �˷��ֱ�
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


    [Header("BPM����")]
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
        musicBPM = GameManager.Instance.BPM; //���� BPM �ֱ�
        LineNum = (int)GameManager.Instance.findrasmemo;

        //�̹��� �迭�� ó�� �صα� ���߿�....
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

    private void ReadNextNotePosition() //������ �б� �Լ�
    {
        GoNextPositionParsing = true;
        PositionTimingParsing();
    }

    public List<string> NotePosision = new List<string>(); //��Ʈ ������
    public List<string> NoteTimeing = new List<string>(); //��Ʈ Ÿ�̹�
    int LineNum; //���� �ѹ�
    bool GoNextPositionParsing = true; // �����ٷ� �Ѿ��?
    

    private void PositionTimingParsing() //��Ʈ ������ �о����
    {
        NotePosision = new List<string>(); //����Ʈ ����
        NoteTimeing= new List<string>();
        while (GoNextPositionParsing)
        {
            if (IsNumber(lines[LineNum])) //���� ������ ���ڸ�
            {
                //NotePosision = new List<string>();
                
                GoNextPositionParsing = false;
            }
            if (!IsNumber(lines[LineNum]) && lines[LineNum].Length >= 4) //���� ������ ���ڰ� �ƴϰ� 4�ڸ� �̻��̶��
            {
                NotePosision.Add(lines[LineNum].Substring(0, 4));
                if (lines[LineNum].Length >= 9)
                {
                    NoteTimeing.Add(lines[LineNum].Substring(6, 4)); //��Ʈ Ÿ�̹� ����Ʈ �־��ֱ�
                }
            }
            LineNum++; //������
        }
    }




    public void mappingNote(List<string> notepo) //��ųʸ��� �����ϴ� �Լ�
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
        tikTime = (stdBPM / musicBPM) * (musicTempo / stdTempo); //��Ʈ ��� �ӵ�
        nextTime += Time.deltaTime;

        if (nextTime >= tikTime) // '>='�� ����
        {

            StartCoroutine(PlayTik(tikTime));


            nextTime -= tikTime; // -= ���� �߰�
        }


    }


    // ���ڿ��� �������� Ȯ���ϴ� �Լ�
    bool IsNumber(string str)
    {
        int n;
        return int.TryParse(str, out n);
    }






    IEnumerator PlayTik(float tikTime) //BPM�� ���� ��Ʈ ������
    {


        Debug.Log("���� ������");
        yield return new WaitForSeconds(tikTime);
    }


}
