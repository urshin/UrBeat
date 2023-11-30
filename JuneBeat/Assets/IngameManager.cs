using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class IngameManager : MonoBehaviour
{
    [Header("������")]
    [SerializeField] Text Title;
    [SerializeField] Text Artist;
    [SerializeField] Text BPM;
    [SerializeField] Text Notes;
    [SerializeField] Image Dif;
    [SerializeField] Image Level;
    [SerializeField] Image MusicImage;


    [SerializeField] Sprite[] DifSprite;
    [SerializeField] Sprite[] LevelSprite;



    [SerializeField] Sprite[] IngameScoreSprite;
    [SerializeField] Image[] Score;

    [SerializeField] Sprite[] ComboImageSprite;


    [Header("���� ���")]
    [SerializeField] Image ScoreImage;
    [SerializeField] Sprite[] ScoreSprite;
    public float fadeDuration = 2.0f; // ���İ� ������ �ɸ��� �ð� (��)

    private void Start()
    {
        Title.text = GameManager.Instance.CurrentSongName;
        Artist.text = GameManager.Instance.Artist;
        BPM.text = GameManager.Instance.BPM.ToString();
        Notes.text = GameManager.Instance.TotalNote.ToString();
        Dif.sprite = DifSprite[(int)GameManager.Instance.Dif - 1];
        Level.sprite = LevelSprite[(int)GameManager.Instance.Level - 1];
        MusicImage.sprite = GameManager.Instance.MusicImage;
        DataManager.Instance.ResetScore(); //���� �ý��� �ʱ�ȭ
        DataManager.Instance.SetingScoreSystem(); // ���� �ý��� �ݿ�
        showResult = true;


    }

    [SerializeField] GameObject ComboImage;
    private void Update()
    {
        PositionNumber();
        ComboImageSystem();
        if (GameManager.Instance.currentState == CurrentState.result && showResult)
        {
            Invoke("Result", 3.0f);
            showResult = false;
        }

    }

    private void ComboImageSystem()
    {



        int combo = (int)DataManager.Instance.Combo;

        GameObject ComboOne = ComboImage.transform.GetChild(0).gameObject;
        GameObject ComboTwo = ComboImage.transform.GetChild(1).gameObject;
        GameObject ComboThree = ComboImage.transform.GetChild(2).gameObject;
        GameObject ComboFour = ComboImage.transform.GetChild(3).gameObject;
        if (combo >= 1)
        {
            ComboOne.SetActive(true);
            ComboOne.GetComponent<Image>().sprite = ComboImageSprite[combo % 10];
        }
        if (combo >= 10)
        {
            ComboTwo.SetActive(true);
            ComboTwo.GetComponent<Image>().sprite = ComboImageSprite[(combo / 10) % 10];
        }
        if (combo >= 100)
        {
            ComboThree.gameObject.SetActive(true);
            ComboThree.gameObject.GetComponent<Image>().sprite = ComboImageSprite[(combo / 100) % 10];
        }
        if (combo >= 1000)
        {
            ComboFour.gameObject.SetActive(true);
            ComboFour.gameObject.GetComponent<Image>().sprite = ComboImageSprite[(combo / 1000) % 10];
        }
        if (combo <= 0)
        {
            for (int i = 0; i < ComboImage.transform.childCount; i++)
            {
                ComboImage.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //bool showResult;
    //void Result()
    //{
    //    int totalScore = (int)DataManager.Instance.totalScore;

    //    if (totalScore < 500000)
    //    {
    //        Debug.Log("E Rating - " + totalScore);
    //        ScoreImage.sprite = ScoreSprite[0];
    //        StartCoroutine(FadeInAlpha());

    //    }
    //    else if (totalScore < 700000)
    //    {
    //        Debug.Log("D Rating - " + totalScore);
    //        ScoreImage.sprite = ScoreSprite[1];
    //        StartCoroutine(FadeInAlpha());
    //    }
    //    else if (totalScore < 800000)
    //    {
    //        Debug.Log("C Rating - " + totalScore);
    //        ScoreImage.sprite = ScoreSprite[2];
    //        StartCoroutine(FadeInAlpha());
    //    }
    //    else if (totalScore < 850000)
    //    {
    //        Debug.Log("B Rating - " + totalScore);
    //        ScoreImage.sprite = ScoreSprite[3];
    //        StartCoroutine(FadeInAlpha());
    //    }
    //    else if (totalScore < 900000)
    //    {
    //        Debug.Log("A Rating - " + totalScore);
    //        ScoreImage.sprite = ScoreSprite[4];
    //        StartCoroutine(FadeInAlpha());
    //    }
    //    else if (totalScore < 950000)
    //    {
    //        Debug.Log("S Rating - " + totalScore);
    //        ScoreImage.sprite = ScoreSprite[5];
    //        StartCoroutine(FadeInAlpha());
    //    }
    //    else if (totalScore < 980000)
    //    {
    //        Debug.Log("SS Rating - " + totalScore);
    //        ScoreImage.sprite = ScoreSprite[6];
    //        StartCoroutine(FadeInAlpha());
    //    }
    //    else if (totalScore < 1000000)
    //    {
    //        Debug.Log("SSS Rating - " + totalScore);
    //        ScoreImage.sprite = ScoreSprite[7];
    //        StartCoroutine(FadeInAlpha());
    //    }
    //    else
    //    {
    //        Debug.Log("Excellent - " + totalScore);
    //        ScoreImage.sprite = ScoreSprite[8];
    //        StartCoroutine(FadeInAlpha());
    //    }
    //   showResult= false;
    //}
    //private IEnumerator FadeInAlpha()
    //{
    //    float startTime = Time.time;
    //    Color startColor = ScoreImage.color;
    //    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // ���� ���İ��� 1.0���� ����

    //    while (Time.time - startTime < fadeDuration)
    //    {
    //        float t = (Time.time - startTime) / fadeDuration; // �ð��� ���� ���� �� ���
    //        ScoreImage.color = Color.Lerp(startColor, endColor, t); // ���İ� ����

    //        yield return null; // �� ������ ��ٸ�
    //    }

    //    ScoreImage.color = endColor; // ���� ���İ� ����
    //}



    bool showResult; // ����� ǥ������ ���θ� ��Ÿ���� ����

    void Result()
    {
        // ������ �Ŵ������� �� ������ ������
        int totalScore = (int)DataManager.Instance.totalScore;

        // �� ������ ������� ����� ������
        string rating = GetRating(totalScore);

        // ��ް� �� ������ ����� �α׿� ���
        Debug.Log(rating + " - " + totalScore);

        // �� ������ ������� ��������Ʈ �ε����� ������
        int spriteIndex = GetSpriteIndex(totalScore);

        // ScoreImage�� ��������Ʈ�� ScoreSprite �迭���� ������ ��������Ʈ�� ����
        ScoreImage.sprite = ScoreSprite[spriteIndex];

        DataManager.Instance.InputScoreData(GameManager.Instance.CurrentSongAndDiff, totalScore);

        StartCoroutine(ShowingScore());

        // ��� ǥ�ø� ��Ȱ��ȭ
        showResult = false;

    }

    [SerializeField] Sprite[] ClearExelFailed;
    [SerializeField] Image CEF;
    [SerializeField] Image FullCombo;
    [SerializeField] Image Combo;
    [SerializeField] GameObject comboNum;
    [SerializeField] Image Rating;
    [SerializeField] float WaitingTime = 5f;
    [SerializeField] GameObject NextBtn;
    [SerializeField] GameObject NextBtnPosition;

    IEnumerator ShowingScore()
    {
        SoundManager.Instance.EffectPlay("result");
        for (int i = 0; i < ComboImage.transform.childCount; i++)
        {
            StartCoroutine(FadeOutAlpha(ComboImage.transform.GetChild(i).gameObject.GetComponent<Image>()));
          
        }
        yield return new WaitForSeconds(fadeDuration + WaitingTime);
        if (DataManager.Instance.totalScore >= DataManager.Instance.BaseScore) //������Ʈ
        {
            CEF.sprite = ClearExelFailed[0];
            StartCoroutine(FadeInAlpha(CEF));
            SoundManager.Instance.EffectPlay("voice_excellent", "snd_excellent");
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            StartCoroutine(FadeOutAlpha(CEF));

            yield return new WaitForSeconds(fadeDuration + WaitingTime);

            StartCoroutine(FadeInAlpha(FullCombo));
            SoundManager.Instance.EffectPlay("voice_fullcombo", "snd_fullcombo");
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            StartCoroutine(FadeOutAlpha(FullCombo));

            yield return new WaitForSeconds(fadeDuration + WaitingTime);

            StartCoroutine(FadeInAlpha(Combo));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            ResultCombo(true);
            ScoreImage.sprite = ScoreSprite[GetSpriteIndex((int)DataManager.Instance.totalScore)];
            StartCoroutine(FadeInAlpha(Rating));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            StartCoroutine(FadeInAlpha(ScoreImage));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);


        }
        else if (DataManager.Instance.totalScore < (DataManager.Instance.BaseScore / 10) * 6) //����
        {
            CEF.sprite = ClearExelFailed[2];
            StartCoroutine(FadeInAlpha(CEF));
            SoundManager.Instance.EffectPlay("voice_failed", "snd_failed");
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            StartCoroutine(FadeOutAlpha(CEF));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);

            StartCoroutine(FadeInAlpha(Combo));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            ResultCombo(true);
            ScoreImage.sprite = ScoreSprite[GetSpriteIndex((int)DataManager.Instance.totalScore)];
            StartCoroutine(FadeInAlpha(Rating));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            StartCoroutine(FadeInAlpha(ScoreImage));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
        }
        else //�⺻
        {
            CEF.sprite = ClearExelFailed[1];
            StartCoroutine(FadeInAlpha(CEF));
            SoundManager.Instance.EffectPlay("voice_cleared", "snd_cleared");
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            StartCoroutine(FadeOutAlpha(CEF));
            if (GameManager.Instance.MaxComboNum == DataManager.Instance.Combo) //Ǯ�޺��϶�
            {
                StartCoroutine(FadeInAlpha(FullCombo));
                yield return new WaitForSeconds(fadeDuration + WaitingTime);
                StartCoroutine(FadeOutAlpha(FullCombo));
            }

            StartCoroutine(FadeInAlpha(Combo));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            ResultCombo(true);
            ScoreImage.sprite = ScoreSprite[GetSpriteIndex((int)DataManager.Instance.totalScore)];
            StartCoroutine(FadeInAlpha(Rating));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            StartCoroutine(FadeInAlpha(ScoreImage));
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
        }


        GameObject button =  Instantiate(NextBtn, NextBtnPosition.transform.position, Quaternion.identity);
        button.transform.SetParent(GameObject.Find("Canvas").transform);


    }




    private void ResultCombo(bool on)
    {
        if (on)
        {
            int combo = (int)DataManager.Instance.Combo;
            GameObject ComboOne = comboNum.transform.GetChild(0).gameObject;
            GameObject ComboTwo = comboNum.transform.GetChild(1).gameObject;
            GameObject ComboThree = comboNum.transform.GetChild(2).gameObject;
            GameObject ComboFour = comboNum.transform.GetChild(3).gameObject;
            if (combo >= 1)
            {
                ComboOne.SetActive(true);
                ComboOne.GetComponent<Image>().sprite = ComboImageSprite[combo % 10];
            }
            if (combo >= 10)
            {
                ComboTwo.SetActive(true);
                ComboTwo.GetComponent<Image>().sprite = ComboImageSprite[(combo / 10) % 10];
            }
            if (combo >= 100)
            {
                ComboThree.gameObject.SetActive(true);
                ComboThree.gameObject.GetComponent<Image>().sprite = ComboImageSprite[(combo / 100) % 10];
            }
            if (combo >= 1000)
            {
                ComboFour.gameObject.SetActive(true);
                ComboFour.gameObject.GetComponent<Image>().sprite = ComboImageSprite[(combo / 1000) % 10];
            }
            if (combo <= 0)
            {
                for (int i = 0; i < comboNum.transform.childCount; i++)
                {
                    comboNum.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

        }
        if(!on)
        {
            for (int i = 0; i < Combo.transform.childCount; i++)
            {
                comboNum.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }


    string GetRating(int score)
    {
        if (score < 600000) return "E Rating";
        else if (score < 700000) return "D Rating";
        else if (score < 800000) return "C Rating";
        else if (score < 850000) return "B Rating";
        else if (score < 900000) return "A Rating";
        else if (score < 950000) return "S Rating";
        else if (score < 980000) return "SS Rating";
        else if (score < 1000000) return "SSS Rating";
        else return "Excellent";
    }

    int GetSpriteIndex(int score)
    {
        if (score < 600000) return 0;
        else if (score < 700000) return 1;
        else if (score < 800000) return 2;
        else if (score < 850000) return 3;
        else if (score < 900000) return 4;
        else if (score < 950000) return 5;
        else if (score < 980000) return 6;
        else if (score < 1000000) return 7;
        else return 8;
    }

    private IEnumerator FadeInAlpha(Image image)
    {
        float startTime = Time.time;
        Color startColor = image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // ���� ä���� 1.0f (������ ������)�� ����

        while (Time.time - startTime < fadeDuration) // fadeDuration ���� �ݺ�
        {
            float t = (Time.time - startTime) / fadeDuration; // ��� �ð��� ���� ���� ���
            image.color = Color.Lerp(startColor, endColor, t); // ���� ���� �� �� ���̸� �����Ͽ� �̹��� ���� ����
            yield return null;
        }

        image.color = endColor; // fadeDuration ������ ������ ������ �̹��� ������ ���� �������� ����
    }


    private IEnumerator FadeOutAlpha(Image image)
    {
        float startTime = Time.time;
        Color startColor = image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f); // ���� ä���� 0.0f (������ ����)�� ����

        while (Time.time - startTime < fadeDuration) // fadeDuration ���� �ݺ�
        {
            float t = (Time.time - startTime) / fadeDuration; // ��� �ð��� ���� ���� ���
            image.color = Color.Lerp(startColor, endColor, t); // ���� ���� �� �� ���̸� �����Ͽ� �̹��� ���� ����
            yield return null;
        }

        image.color = endColor; // fadeDuration ������ ������ ������ �̹��� ������ ���� �������� ���� (������ ����)
    }




    int[] numbers = new int[7];

    public void PositionNumber() //��Ʈ�� �ڸ������� ���� ���ϱ�
    {
      


        //����ȭ ����
        int score = (int)DataManager.Instance.totalScore;
        int[] numbers = new int[7];

        for (int i = 0; i < 7; i++)
        {
            numbers[i] = (score / (int)Mathf.Pow(10, i)) % 10;
            Score[i].sprite = IngameScoreSprite[numbers[i]];
        }


    }


    





}
