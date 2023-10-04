using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using UnityEngine.TextCore.Text;
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

    public bool isSongEnd;

    private void Start()
    {
        //���� �Ŵ������� ���� ������. �˷��ֱ�
        GameManager.Instance.CurrentSongAndDiff = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult;
        isSongEnd = false;
        ShowInfoSong();
        ReadSongTxt();
        PositionTimingParsing();

    }


    [SerializeField] GameObject Note;
    [SerializeField] Transform[] NotePositionArray;

    public Dictionary<int, char> symbolToIndex1 = new Dictionary<int, char>
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
        NoteTimeing = new List<string>();
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
                if (lines[LineNum].Length >= 6)
                {
                    NoteTimeing.Add(lines[LineNum].Substring(6, lines[LineNum].LastIndexOf('|') - 6)); //��Ʈ Ÿ�̹� ����Ʈ �־��ֱ�
                }
            }
            LineNum++; //������
        }
    }




    public void mappingNote(List<string> notepo) //��ųʸ��� �����ϴ� �Լ�
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

    public List<char> ccharactor = new List<char>();

    private void Update()
    {

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


    public int currentNoteCount;


    public int countNoteTiming = 0;

    public GameObject LongNote;

    IEnumerator PlayTik(float tikTime) //BPM�� ���� ��Ʈ ������
    {
        if (!isSongEnd)
        {
            if (countNoteTiming > ccharactor.Count - 1)
            {
                ReadNextNotePosition();
                mappingNote(NotePosision);
                ListingNoteTiming();
                Debug.Log(NotePosision.Count);
                countNoteTiming = 0;
            }


            if (symbolToIndex1.ContainsValue(ccharactor[countNoteTiming]))
            {
                foreach (var kvp in symbolToIndex1)
                {
                    // �Ƹ� �� Ÿ�ֿ̹� �ճ�Ʈ ���� �װ� ������ �ɵ�
                    if (kvp.Value == ccharactor[countNoteTiming])
                    {
                        int p = kvp.Key;
                        if (p >= 32) // kvp.Key ���� 32 �̻��� ��
                        {
                            p -= 32;
                        }
                        else if (p >= 16) // kvp.Key ���� 16 �̻��� ��
                        {
                            p -= 16;
                        }

                        

                        //if (p - 1 >= 0)
                        //{
                        //    if (symbolToIndex1[p - 1].ToString().Contains("��"))
                        //    {
                        //        Debug.Log(symbolToIndex1[p] + "     " + "���� �ճ�Ʈ��");
                        //    }
                        //    if (symbolToIndex1[p + 1].ToString().Contains("��"))
                        //    {
                        //        Debug.Log(symbolToIndex1[p] + "     " + "������ �ճ�Ʈ��");
                        //    }
                        //}


                        Instantiate(Note, NotePositionArray[p]); //��Ʈ ����

                        currentNoteCount++;
                    }
                  
                    ////�ճ�Ʈ!!!!!!!!!!!!!!!!!!!!!
                    //switch (kvp.Value) //�ճ�Ʈ ó��
                    //{
                    //    case '��':
                    //        SpawnNoteFromDictionary(LongNote, kvp);
                    //        break;
                    //    case '��':
                    //        SpawnNoteFromDictionary(LongNote, kvp);
                    //        break;
                    //    case '��':
                    //        SpawnNoteFromDictionary(LongNote, kvp);
                    //        break;
                    //    case '��':
                    //        SpawnNoteFromDictionary(LongNote, kvp);
                    //        break;

                    //}


                }
            }
            countNoteTiming++;
            if (currentNoteCount > GameManager.Instance.TotalNote)
            {
                //isSongEnd = true;
            }
        }
        yield return new WaitForSeconds(tikTime);
    }

    private void SpawnNoteFromDictionary(GameObject note, KeyValuePair<int, char> kvp) //��ųʸ����� �� �а� ��Ʈ �����ϱ�
    {
        int p = kvp.Key;
        if (p >= 32) // kvp.Key ���� 32 �̻��� ��
        {
            p -= 32;
        }
        else if (p >= 16) // kvp.Key ���� 16 �̻��� ��
        {
            p -= 16;
        }
        Instantiate(note, NotePositionArray[p]); //��Ʈ ����
    }
}
