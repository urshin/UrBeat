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
        //���� �Ŵ������� ���� ������. �˷��ֱ�
        GameManager.Instance.CurrentSongAndDiff = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult;

        ShowInfoSong();
        //���� �ڵ�͵�
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


    [Header("BPM����")]
    public float musicBPM = 60f;
    public float stdBPM = 60;
    public float musicTempo = 4;
    public float stdTempo = 4;
    float tikTime = 0;
    float nextTime = 0;

    [Header("��")]
    [SerializeField] List<List<string>> groupedLines = new List<List<string>>(); // �� �׷��� ������ ����Ʈ
    [SerializeField] List<string> currentGroup = new List<string>(); // ���� �׷��� ���� ������ ����Ʈ
    [SerializeField] Transform notePosion; //��Ʈ ���� ��ġ
    [SerializeField] GameObject note; //��Ʈ ���� ��
    [SerializeField] Transform[] NoteArray;//��Ʈ ���� ��ġ���� ������ �ִ� �迭
    [SerializeField] List<string> TimePosition = new List<string>();
    [SerializeField] List<string> notePosition = new List<string>(); //��Ʈ ������
    [SerializeField] List<string> noteTiming = new List<string>();//��Ʈ Ÿ�̹�
    int pipeCount = 0; //��Ʈ ���� Ÿ�̹� �����ϱ� ���� ��ȣ ��.
    int pipepipeCount = 0; //��Ʈ ���� Ÿ�̹� �����ϱ� ���� ��ȣ ��.
    float timer = 0f; // Ÿ�̸� ����
    int currentSymbolIndex = 0; // ���� ��� ���� �ɺ� �ε���
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
        musicBPM = GameManager.Instance.BPM; //���� BPM �ֱ�
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





    public void mappingNote(List<string> notepo) //��ųʸ��� �����ϴ� �Լ�
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
            string line = lines[i].Trim(); // ���� ���� �����ͼ� ���� ����

            if (pipeCount == 4)
            {
                groupedLines.Add(currentGroup);
                currentGroup = new List<string>();
                notePosition = new List<string>();
                pipeCount = 0;

            }
            currentGroup.Add(line); // ���� ���� ���� �׷쿡 �߰�

            if (!IsNumber(line) && line.Length >= 4)
            {
                if (currentGroup.Count > 0)
                {

                    string firstFourCharacters = line.Substring(0, 4); //���ڿ��� 4�̻��� ���ο��� 0���� 3������ ��������

                    notePosition.Add(firstFourCharacters);
                    // ��Ʈ Ÿ�̹� ������ �����Ͽ� noteTiming ����Ʈ�� �߰��ؾ� ��
                    string timingInfo = line.Substring(4); // ����: 1234567890 -> "567890"

                    if (line.Contains("|")) // | ��ȣ�� ���Ե� ���� ��쿡�� ó��
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
        tikTime = (stdBPM / musicBPM) * (musicTempo / stdTempo); //��Ʈ ��� �ӵ�
        nextTime += Time.deltaTime;

        if (nextTime >= tikTime) // '>='�� ����
        {
            if (CurrentNoteCount <= GameManager.Instance.TotalNote)
            {
                StartCoroutine(PlayTik(tikTime));

            }
            nextTime -= tikTime; // -= ���� �߰�
        }
    }


    // ���ڿ��� �������� Ȯ���ϴ� �Լ�
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

            string line = lines[j].Trim(); // ���� ���� �����ͼ� ���� ����

            if (IsNumber(line))
            {
                TimePosition = new List<string>();

            }
            if (!IsNumber(line) && line.Length >= 4)
            {
                string firstFourCharacters = line.Substring(0, 4); //���ڿ��� 4�̻��� ���ο��� 0���� 3������ ��������

                TimePosition.Add(firstFourCharacters);
                if (line.Contains("|")) //  ��ȣ�� ���Ե� ���� ��쿡�� ó��
                {
                    pipepipeCount++;
                }
            }
            j++;
            CurrentNoteCount++;
        }



    }




    IEnumerator PlayTik(float tikTime) //BPM�� ���� ��Ʈ ������
    {

        if (noteCount == 0) //��Ʈ ī��Ʈ�� 0�϶� ���� ��Ʈ �о���� ä�� ����
        {
            timepositions();


        }
        if (TimePosition.Count >= 8) // TimePosition ����Ʈ ũ�Ⱑ 4���� Ŭ ���
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


            if (character == '��')
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
