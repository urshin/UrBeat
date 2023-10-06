using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        GameManager.Instance.currentState = CurrentState.Ingame;
        //���� �Ŵ������� ���� ������. �˷��ֱ�
        GameManager.Instance.CurrentSongAndDiff = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult;
        isSongEnd = false;
        ShowInfoSong();
        ReadSongTxt();
        PositionTimingParsing();
       
    }






    private void ShowInfoSong()
    {
        musicBPM = GameManager.Instance.BPM; //���� BPM �ֱ�
        LineNum = (int)GameManager.Instance.findrasmemo;

    }

    [SerializeField] int lineCount;
    public void ReadSongTxt()
    {
        filePath = "music/" + GameManager.Instance.CurrentSongAndDiff;
        textAsset = new TextAsset();
        textAsset = Resources.Load<TextAsset>(filePath);
        Debug.Log(filePath);
        lines = textAsset.text.Split('\n');
        lineCount = lines.Length; //���� �߰��Ͽ� ���� �� ����
    }

    private void ReadNextNotePosition() //������ �б� �Լ�
    {
        GoNextPositionParsing = true;
        PositionTimingParsing();
    }

    public List<string> NotePosision = new List<string>(); //��Ʈ ������
    public List<string> NoteTimeing = new List<string>(); //��Ʈ Ÿ�̹�
    [SerializeField] int LineNum; //���� �ѹ�
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

    private float countdown = 3.0f; // 3�� Ÿ�̸� ����
    private float timer = 0.0f;
    [SerializeField] Sprite[] ReadyGo;
    [SerializeField] Image MarkPositionSprite;

    bool TimerEnd = false;
    void Timer()
    {
        timer += Time.deltaTime; // ������ ���ݿ� ���� �ð� ������Ʈ

        if (timer >= 1.0f) // 1�ʸ���
        {
            timer = 0.0f; // Ÿ�̸� �ʱ�ȭ
            countdown--; // ���� �ð� ����
            Debug.Log("���� �ð�: " + countdown + "��"); // ����� �α� ���

            if (countdown >= 1)
            {
                MarkPositionSprite.sprite = ReadyGo[(int)countdown - 1];
               
            }

            if (countdown <= 0) // Ÿ�̸Ӱ� 0 ���Ϸ� ��������
            {
                Debug.Log("Ÿ�̸� ����");
                TimerEnd = true;

                // MarkPositionSprite�� �����ϰ� �����
                Color spriteColor = MarkPositionSprite.color;
                spriteColor.a = 0.0f; // ���İ��� 0���� �����Ͽ� �����ϰ� ����ϴ�.
                MarkPositionSprite.color = spriteColor;
            }
            else
            {
                // Ÿ�̸Ӱ� 0�� �ƴ� ��쿡 MarkPositionSprite�� ���̰� �����
                Color spriteColor = MarkPositionSprite.color;
                spriteColor.a = 1.0f; // ���İ��� 1�� �����Ͽ� ���̰� ����ϴ�.
                MarkPositionSprite.color = spriteColor;
            }
        }
    }

    private void FixedUpdate()
    {
        tikTime = (stdBPM / musicBPM) * (musicTempo / stdTempo); //��Ʈ ��� �ӵ�
        nextTime += Time.deltaTime;

        if (!TimerEnd)
        {
            Timer();

        }

        if (nextTime >= tikTime) // '>='�� ����
        {
            if (!isSongEnd && TimerEnd)
            {
                StartCoroutine(PlayTik(tikTime));
            }

            nextTime -= tikTime; // -= ���� �߰�
        }

        if (isSongEnd && GameManager.Instance.currentState == CurrentState.Ingame)
        {
            GameManager.Instance.currentState = CurrentState.result;
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

    List<int> LongNoteOrder = new List<int>(); //�ճ�Ʈ ����
    public char LongNoteTimingCharactor;

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
    };
    IEnumerator PlayTik(float tikTime) //BPM�� ���� ��Ʈ ������
    {

        if (countNoteTiming > ccharactor.Count - 1)
        {
            ReadNextNotePosition();
            mappingNote(NotePosision);
            ListingNoteTiming();
            //Debug.Log(NotePosision.Count);
            countNoteTiming = 0;

            symbolToIndex2.Clear();
            // Dictionary�� ��� ��Ʈ���� ��ȸ�ϸ鼭 ���ϴ� Value�� ���� Key ã��
            foreach (var kvp in symbolToIndex1)
            {
                if (kvp.Value == '��')
                {
                    symbolToIndex2.Add(kvp.Key, kvp.Value);
                }
                if (kvp.Value == '��')
                {
                    symbolToIndex2.Add(kvp.Key, kvp.Value);
                }
                if (kvp.Value == '��')
                {
                    symbolToIndex2.Add(kvp.Key, kvp.Value);
                }
                if (kvp.Value == '��')
                {
                    symbolToIndex2.Add(kvp.Key, kvp.Value);
                }
            }


            foreach (var kvp in symbolToIndex1)
            {
                if (kvp.Value == '��')
                {
                    symbolToIndex2.Add(kvp.Key, kvp.Value);
                }
                if (kvp.Value == '��')
                {
                    symbolToIndex2.Add(kvp.Key, kvp.Value);
                }
            }

            // ã�� ��� Key�� ����� �α׷� ���
            //foreach (var key in symbolToIndex2)
            //{
            //    Debug.Log("�ճ�Ʈ ����   " + key.Key + "�� �� �ִ°� " + key.Value);
            //    if (key.Value == '��')
            //    {
            //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
            //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 270f);
            //    }
            //    if (key.Value == '��')
            //    {
            //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
            //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            //    }
            //    if (key.Value == '��')
            //    {
            //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
            //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            //    }
            //    if (key.Value == '��')
            //    {
            //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
            //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            //    }

            //}

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




                    Instantiate(Note, NotePositionArray[p]); //��Ʈ ����
                    currentNoteCount++;
                }
            }

        }
        countNoteTiming++;


        if (currentNoteCount > GameManager.Instance.TotalNote)
        {
            //isSongEnd = true;
        }


        if (LineNum >= lineCount)
        {
            isSongEnd = true;
            Debug.Log("�뷡 ������ ������");
        }
        yield return new WaitForSeconds(tikTime);
    }

}
