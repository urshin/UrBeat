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



    }

    [SerializeField] GameObject ComboImage;
    private void Update()
    {
        PositionNumber();
        ComboImageSystem();

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

    void Result()
    {


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
