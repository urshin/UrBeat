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
        musicBPM = GameManager.Instance.BPM; //���� BPM �ֱ�
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
        lineCount = textAsset.text.Length; //���� �߰��Ͽ� ���� �� ����
        ReadNotePositonAll();//��ü ������ �б�.

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
        while (GoNextPositionParsing && LineNum < lines.Length)
        {

            if (IsNumber(lines[LineNum]) || string.IsNullOrWhiteSpace(lines[LineNum])) //���� ������ ���ڸ�
            {
            

                GoNextPositionParsing = false;
            }
            if (!IsNumber(lines[LineNum]) && lines[LineNum].Length >= 4) //���� ������ ���ڰ� �ƴϰ� 4�ڸ� �̻��̶��
            {
                NotePosision.Add(lines[LineNum].Substring(0, 4));
                if (lines[LineNum].Length >= 6)
                {
                    NoteTimeing.Add(lines[LineNum].Substring(6, lines[LineNum].LastIndexOf('|') - 6)); 
                  
                }
            }
            LineNum++; //������
        }
    }
    public List<string> allNotePosition = new List<string>();
    [SerializeField] int allLineNum;

    void ReadNotePositonAll()
    {
        int maxCombo = 0; // MaxCombo�� ���� ������ ����
        allLineNum = (int)GameManager.Instance.findrasmemo;
        allNotePosition.Clear(); // ����Ʈ �ʱ�ȭ

        while (allLineNum < lines.Length) // '<=' ��� '<'�� ����Ͽ� �迭 ��踦 �ʰ����� �ʵ��� �մϴ�.
        {
            if (!IsNumber(lines[allLineNum]) && lines[allLineNum].Length >= 4)
            {
                string notePosition = lines[allLineNum].Substring(0, 4);
                allNotePosition.Add(notePosition);

                foreach (char character in notePosition)
                {
                    // ���ʿ��� ���ڸ� �˻����� �ʰ�, ���� ���� ������ ����
                    if (character != '��' && character != '��' && character != '��' && character != '��' && character != '��' && character != '��' && character != 'Ϣ' && character != '��')
                    {
                        maxCombo++;
                    }
                }
            }

            allLineNum++; // ���� �ٷ� �̵�
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
    //         if (IsNumber(lines[allLineNum])) //���� ������ ���ڸ�
    //         {
    //             //NotePosision = new List<string>();

    //         }
    //         if (!IsNumber(lines[allLineNum]) && lines[allLineNum].Length >= 4) //���� ������ ���ڰ� �ƴϰ� 4�ڸ� �̻��̶��
    //         {
    //             allNotePosition.Add(lines[allLineNum].Substring(0, 4));
    //            for(int i =0; i < lines[allLineNum].Substring(0, 4).Length;  i++)
    //             {
    //                 char charactor = lines[allLineNum][i];
    //                 if( charactor != '��' && charactor != '��' && charactor != '��' && charactor != '��' && charactor != '��' &&charactor != '��' && charactor != 'Ϣ' && charactor != '��')
    //                 {
    //                     MaxCombo++;
    //                 }
    //             }
    //         }
    //         allLineNum++; //������

    //     }
    //     GameManager.Instance.MaxComboNum = MaxCombo;
    // }



    public void mappingNote1(List<string> notepo) //��ųʸ��� �����ϴ� �Լ�
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
                int key = j * notepo[j].Length + k; // key�� j*k�� ����
                char value = notepo[j][k]; // ���ڸ� ����Ʈ�� ���ڷ� ����
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

    private float countdown = 3.0f; // 3�� Ÿ�̸� ����
    private float timer = 0.0f;
    [SerializeField] Sprite[] ReadyGo;
    [SerializeField] Image MarkPositionSprite;

    public bool TimerEnd = false;

    //void Timer()
    //{
    //    timer += Time.deltaTime; // ������ ���ݿ� ���� �ð� ������Ʈ

    //    if (timer >= 1.0f) // 1�ʸ���
    //    {
    //        timer = 0.0f; // Ÿ�̸� �ʱ�ȭ
    //        countdown--; // ���� �ð� ����
    //        Debug.Log("���� �ð�: " + countdown + "��"); // ����� �α� ���

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

    //        if (countdown <= 0) // Ÿ�̸Ӱ� 0 ���Ϸ� ��������
    //        {
    //            Debug.Log("Ÿ�̸� ����");
    //            TimerEnd = true;

    //            // MarkPositionSprite�� �����ϰ� �����
    //            Color spriteColor = MarkPositionSprite.color;
    //            spriteColor.a = 0.0f; // ���İ��� 0���� �����Ͽ� �����ϰ� ����ϴ�.
    //            MarkPositionSprite.color = spriteColor;
    //        }
    //        else
    //        {
    //            // Ÿ�̸Ӱ� 0�� �ƴ� ��쿡 MarkPositionSprite�� ���̰� �����
    //            Color spriteColor = MarkPositionSprite.color;
    //            spriteColor.a = 1.0f; // ���İ��� 1�� �����Ͽ� ���̰� ����ϴ�.
    //            MarkPositionSprite.color = spriteColor;
    //        }
    //    }
    //}
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

    //[SerializeField] int NoteCounting;
    private void FixedUpdate()
    {
        if (GameManager.Instance.currentState == CurrentState.Ingame)
        {
            // ��Ʈ ��� �ӵ� ���
            tikTime = (stdBPM / musicBPM) * (musicTempo / stdTempo);
            nextTime += Time.deltaTime;

            // Ÿ�̸Ӱ� ������� �ʾ��� ���� Ÿ�̸� ����
            if (!TimerEnd)
            {
                Timer();
            }

            // �־��� �ð� ���ݸ��� ��Ʈ�� ���
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
                    //// Dictionary�� ��� ��Ʈ���� ��ȸ�ϸ鼭 ���ϴ� Value�� ���� Key ã��
                    //foreach (var kvp in symbolToIndex1)
                    //{
                    //    if (kvp.Value == '��')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //    if (kvp.Value == '��')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //    if (kvp.Value == '��')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //    if (kvp.Value == '��')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //}


                    //foreach (var kvp in symbolToIndex1)
                    //{
                    //    if (kvp.Value == '��')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //    if (kvp.Value == '��')
                    //    {
                    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
                    //    }
                    //}

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
                // ��Ʈ ��� �������� Ÿ�̸Ӱ� ����Ǿ����� Ȯ��
                if (!isSongEnd && TimerEnd&&ccharactor.Count >0)
                {
                    StartCoroutine(PlayTik(tikTime));
                }

                nextTime -= tikTime; // ���� ��Ʈ������ �ð� ���
            }

        }

        // �뷡�� ����ǰ� ���� ���°� Ingame�� �� ��� ȭ������ ��ȯ
        if (isSongEnd && GameManager.Instance.currentState == CurrentState.Ingame)
        {
            //StartCoroutine(PlayTik(tikTime));
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


    IEnumerator PlayTik(float tikTime) //BPM�� ���� ��Ʈ ������
    {

        //if (countNoteTiming > ccharactor.Count - 1)
        //{
        //    ReadNextNotePosition();
        //    mappingNote(NotePosision);
        //    ListingNoteTiming();
        //    //Debug.Log(NotePosision.Count);
        //    countNoteTiming = 0;

        //    //symbolToIndex2.Clear();
        //    //// Dictionary�� ��� ��Ʈ���� ��ȸ�ϸ鼭 ���ϴ� Value�� ���� Key ã��
        //    //foreach (var kvp in symbolToIndex1)
        //    //{
        //    //    if (kvp.Value == '��')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //    if (kvp.Value == '��')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //    if (kvp.Value == '��')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //    if (kvp.Value == '��')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //}


        //    //foreach (var kvp in symbolToIndex1)
        //    //{
        //    //    if (kvp.Value == '��')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //    if (kvp.Value == '��')
        //    //    {
        //    //        symbolToIndex2.Add(kvp.Key, kvp.Value);
        //    //    }
        //    //}

        //    // ã�� ��� Key�� ����� �α׷� ���
        //    //foreach (var key in symbolToIndex2)
        //    //{
        //    //    Debug.Log("�ճ�Ʈ ����   " + key.Key + "�� �� �ִ°� " + key.Value);
        //    //    if (key.Value == '��')
        //    //    {
        //    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
        //    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 270f);
        //    //    }
        //    //    if (key.Value == '��')
        //    //    {
        //    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
        //    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        //    //    }
        //    //    if (key.Value == '��')
        //    //    {
        //    //        GameObject Long = Instantiate(LongNote, NotePositionArray[key.Key]);
        //    //        Long.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        //    //    }
        //    //    if (key.Value == '��')
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

                // �Ƹ� �� Ÿ�ֿ̹� �ճ�Ʈ ���� �װ� ������ �ɵ�
                if (kvp.Value == ccharactor[countNoteTiming])
                {
                    // int p = kvp.Key;

                    //if (p >= 48) // kvp.Key ���� 48 �̻��� ��
                    //{
                    //    p -= 48;
                    //}
                    //if (p >= 32) // kvp.Key ���� 32 �̻��� ��
                    //{
                    //    p -= 32;
                    //}
                    //if (p >= 16) // kvp.Key ���� 16 �̻��� ��
                    //{
                    //    p -= 16;
                    //}
                    int p = kvp.Key % NotePositionArray.Length; //������ p ���� ���������� p���� �迭�� ���̷� ���� �������� ����Ͽ� �迭 ���� ���� ������



                    Instantiate(Note, NotePositionArray[p]); // ��Ʈ ����
                    currentNoteCount++;
                    // Instantiate(Note, NotePositionArray[p]); //��Ʈ ����
                    // currentNoteCount++;
                }
            }
            if (currentNoteCount >= GameManager.Instance.TotalNote)
            {
                isSongEnd = true;
                Debug.Log("�뷡 ������ ������");
            }

        }
        countNoteTiming++;

        yield return new WaitForSeconds(tikTime);
    }

}
