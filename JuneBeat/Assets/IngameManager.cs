using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour
{
    [Header("곡정보")]
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


    [Header("점수 출력")]
    [SerializeField] Image ScoreImage;
    [SerializeField] Sprite[] ScoreSprite;
    public float fadeDuration = 2.0f; // 알파값 증가에 걸리는 시간 (초)

    private void Start()
    {
        Title.text = GameManager.Instance.CurrentSongName;
        Artist.text = GameManager.Instance.Artist;
        BPM.text = GameManager.Instance.BPM.ToString();
        Notes.text = GameManager.Instance.TotalNote.ToString();
        Dif.sprite = DifSprite[(int)GameManager.Instance.Dif - 1];
        Level.sprite = LevelSprite[(int)GameManager.Instance.Level - 1];
        MusicImage.sprite = GameManager.Instance.MusicImage;
        DataManager.Instance.ResetScore(); //점수 시스템 초기화
        DataManager.Instance.SetingScoreSystem(); // 점수 시스템 반영
        showResult = true;


    }

    [SerializeField] GameObject ComboImage;
    private void Update()
    {
        PositionNumber();
        ComboImageSystem();
        if(GameManager.Instance.currentState == CurrentState.result && showResult)
        {
            Invoke("Result", 3.0f);
        }

    }

    private void ComboImageSystem()
    {

        ////간략버전
        //int combo = (int)DataManager.Instance.Combo;

        //for (int i = 0; i < ComboImage.transform.childCount; i++)
        //{
        //    GameObject comboObject = ComboImage.transform.GetChild(i).gameObject;
        //    int digit = (combo / (int)Mathf.Pow(10, i)) % 10;

        //    if (combo > 0 && i < 4)
        //    {
        //        comboObject.SetActive(true);
        //        comboObject.GetComponent<Image>().sprite = ComboImageSprite[digit];
        //    }
        //    else
        //    {
        //        comboObject.SetActive(false);
        //    }
        //}

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
    //    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // 최종 알파값을 1.0으로 설정

    //    while (Time.time - startTime < fadeDuration)
    //    {
    //        float t = (Time.time - startTime) / fadeDuration; // 시간에 따른 보간 값 계산
    //        ScoreImage.color = Color.Lerp(startColor, endColor, t); // 알파값 보간

    //        yield return null; // 한 프레임 기다림
    //    }

    //    ScoreImage.color = endColor; // 최종 알파값 설정
    //}



    bool showResult;

    void Result()
    {
        int totalScore = (int)DataManager.Instance.totalScore;

        string rating = GetRating(totalScore);
        Debug.Log(rating + " - " + totalScore);

        int spriteIndex = GetSpriteIndex(totalScore);
        ScoreImage.sprite = ScoreSprite[spriteIndex];

        StartCoroutine(FadeInAlpha());

        showResult = false;
    }

    string GetRating(int score)
    {
        if (score < 500000) return "E Rating";
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
        if (score < 500000) return 0;
        else if (score < 700000) return 1;
        else if (score < 800000) return 2;
        else if (score < 850000) return 3;
        else if (score < 900000) return 4;
        else if (score < 950000) return 5;
        else if (score < 980000) return 6;
        else if (score < 1000000) return 7;
        else return 8;
    }

    private IEnumerator FadeInAlpha()
    {
        float startTime = Time.time;
        Color startColor = ScoreImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            ScoreImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        ScoreImage.color = endColor;
    }









    int[] numbers = new int[7];

    public void PositionNumber() //노트의 자리수마다 점수 구하기
    {
        //int score = (int)DataManager.Instance.totalScore;

        //numbers[0] = score % 10;
        //numbers[1]= (score / 10) % 10;
        //numbers[2] = (score / 100) % 10;
        //numbers[3] = (score / 1000) % 10;
        //numbers[4] = (score / 10000) % 10;
        //numbers[5]= (score / 100000) % 10;
        //numbers[6] = (score / 1000000) % 10;

        //Score[0].sprite = IngameScoreSprite[numbers[0]];
        //Score[1].sprite = IngameScoreSprite[numbers[1]];
        //Score[2].sprite = IngameScoreSprite[numbers[2]];
        //Score[3].sprite = IngameScoreSprite[numbers[3]];
        //Score[4].sprite = IngameScoreSprite[numbers[4]];
        //Score[5].sprite = IngameScoreSprite[numbers[5]];
        //Score[6].sprite = IngameScoreSprite[numbers[6]];


        //간략화 버전
        int score = (int)DataManager.Instance.totalScore;
        int[] numbers = new int[7];

        for (int i = 0; i < 7; i++)
        {
            numbers[i] = (score / (int)Mathf.Pow(10, i)) % 10;
            Score[i].sprite = IngameScoreSprite[numbers[i]];
        }


    }








}
