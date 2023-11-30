using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;


    string icpPth = "Assets/Data/ScoreData.txt";
    public void Awake()
    {
        if (Instance == null) //정적으로 자신을 체크함, null인진
        {
            Instance = this; //이후 자기 자신을 저장함.
            DontDestroyOnLoad(gameObject);//씬이 전환이 되어도 파괴되지 않고 유지됨.
        }



        if (File.Exists(icpPth) == false)
        {
            var file = File.CreateText(icpPth); //텍스트 파일 생성
            file.Close(); //텍스트 파일 닫아주기
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
        totalScore = 0;
    }


    public float totalScore;

    public float BaseScore = 1000000;
    public void SetingScoreSystem() //점수 판정
    {
        Perfect = BaseScore / GameManager.Instance.TotalNote; //총 얻을 수 있는 점수인 1000000를 노트 수만큼 나눈것
        Great = (Perfect / 100) * 70; //퍼펙트 점수의 70퍼
        Bad = (Perfect / 100) * 30; // 30퍼
        Miss = 0; //없음
    }

    public int readScore(string name)
    {
        string read = File.ReadAllText(icpPth);
        int score = 0;

        string[] lines = read.Split('\n');
        foreach (string line in lines)
        {
            if (line.Contains(name))
            {
                // 이름이 일치하는 줄을 찾아서 해당 줄의 숫자를 score에 할당
                string[] data = line.Split(':');
                score = int.Parse(data[1].Trim());
                break;
            }
        }

        return score;
    }

    public void InputScoreData(string name, int Score)
    {
        string read = File.ReadAllText(icpPth);

        if (!read.Contains(name))
        {
            using (StreamWriter ScoreData = new StreamWriter(icpPth, true))
            {
                ScoreData.WriteLine(name + ":" + Score);
            }
        }
        else
        {
            // 이름이 이미 존재할 때의 처리
            // 기존의 점수를 업데이트
            string[] lines = read.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(name))
                {
                    // 이름이 일치하는 줄을 찾아서 기존 점수를 업데이트
                    string[] data = lines[i].Split(':');

                    if(Score < int.Parse(data[1].Trim()))
                    {
                        lines[i] = name + ":" + Score;
                        break;
                    }    

                    
                }
            }

            // 업데이트된 정보를 파일에 씀
            File.WriteAllLines(icpPth, lines);
        }
    }
}




