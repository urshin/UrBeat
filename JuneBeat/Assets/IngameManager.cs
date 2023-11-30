using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
//using UnityEngine.UIElements;

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
    //    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // 최종 알파값을 1.0으로 설정

    //    while (Time.time - startTime < fadeDuration)
    //    {
    //        float t = (Time.time - startTime) / fadeDuration; // 시간에 따른 보간 값 계산
    //        ScoreImage.color = Color.Lerp(startColor, endColor, t); // 알파값 보간

    //        yield return null; // 한 프레임 기다림
    //    }

    //    ScoreImage.color = endColor; // 최종 알파값 설정
    //}



    bool showResult; // 결과를 표시할지 여부를 나타내는 변수

    void Result()
    {
        // 데이터 매니저에서 총 점수를 가져옴
        int totalScore = (int)DataManager.Instance.totalScore;

        // 총 점수를 기반으로 등급을 가져옴
        string rating = GetRating(totalScore);

        // 등급과 총 점수를 디버그 로그에 출력
        Debug.Log(rating + " - " + totalScore);

        // 총 점수를 기반으로 스프라이트 인덱스를 가져옴
        int spriteIndex = GetSpriteIndex(totalScore);

        // ScoreImage의 스프라이트를 ScoreSprite 배열에서 가져온 스프라이트로 설정
        ScoreImage.sprite = ScoreSprite[spriteIndex];

        DataManager.Instance.InputScoreData(GameManager.Instance.CurrentSongAndDiff, totalScore);

        StartCoroutine(ShowingScore());

        // 결과 표시를 비활성화
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
        if (DataManager.Instance.totalScore >= DataManager.Instance.BaseScore) //엑셀런트
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
        else if (DataManager.Instance.totalScore < (DataManager.Instance.BaseScore / 10) * 6) //실패
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
        else //기본
        {
            CEF.sprite = ClearExelFailed[1];
            StartCoroutine(FadeInAlpha(CEF));
            SoundManager.Instance.EffectPlay("voice_cleared", "snd_cleared");
            yield return new WaitForSeconds(fadeDuration + WaitingTime);
            StartCoroutine(FadeOutAlpha(CEF));
            if (GameManager.Instance.MaxComboNum == DataManager.Instance.Combo) //풀콤보일때
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
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // 알파 채널을 1.0f (완전히 불투명)로 설정

        while (Time.time - startTime < fadeDuration) // fadeDuration 동안 반복
        {
            float t = (Time.time - startTime) / fadeDuration; // 경과 시간에 따른 보간 계산
            image.color = Color.Lerp(startColor, endColor, t); // 시작 색과 끝 색 사이를 보간하여 이미지 색상 설정
            yield return null;
        }

        image.color = endColor; // fadeDuration 동안의 보간이 끝나면 이미지 색상을 최종 색상으로 설정
    }


    private IEnumerator FadeOutAlpha(Image image)
    {
        float startTime = Time.time;
        Color startColor = image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f); // 알파 채널을 0.0f (완전히 투명)로 설정

        while (Time.time - startTime < fadeDuration) // fadeDuration 동안 반복
        {
            float t = (Time.time - startTime) / fadeDuration; // 경과 시간에 따른 보간 계산
            image.color = Color.Lerp(startColor, endColor, t); // 시작 색과 끝 색 사이를 보간하여 이미지 색상 설정
            yield return null;
        }

        image.color = endColor; // fadeDuration 동안의 보간이 끝나면 이미지 색상을 최종 색상으로 설정 (완전히 투명)
    }




    int[] numbers = new int[7];

    public void PositionNumber() //노트의 자리수마다 점수 구하기
    {
      


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
