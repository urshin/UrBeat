using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public void Awake()
    {
        if (Instance == null) //정적으로 자신을 체크함, null인진
        {
            Instance = this; //이후 자기 자신을 저장함.
            DontDestroyOnLoad(gameObject);//씬이 전환이 되어도 파괴되지 않고 유지됨.
        }
    }




    public float Perfect;
    public float Great;
    public float Bad;
    public float Miss;

    public int Combo;


    public void ResetScore()
    {
        Perfect = 0;
        Great = 0;
        Bad = 0;
        Miss = 0;
        Combo = 0;
    }


    public float totalScore;

    float BaseScore = 1000000;
    public void SetingScoreSystem() //점수 판정
    {
        Perfect = BaseScore / GameManager.Instance.TotalNote; //총 얻을 수 있는 점수인 1000000를 노트 수만큼 나눈것
        Great = (Perfect / 100) * 70; //퍼펙트 점수의 70퍼
        Bad = (Perfect / 100) * 30; // 30퍼
        Miss = 0; //없음
    }






}
