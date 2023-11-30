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
        if (Instance == null) //�������� �ڽ��� üũ��, null����
        {
            Instance = this; //���� �ڱ� �ڽ��� ������.
            DontDestroyOnLoad(gameObject);//���� ��ȯ�� �Ǿ �ı����� �ʰ� ������.
        }



        if (File.Exists(icpPth) == false)
        {
            var file = File.CreateText(icpPth); //�ؽ�Ʈ ���� ����
            file.Close(); //�ؽ�Ʈ ���� �ݾ��ֱ�
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
    public void SetingScoreSystem() //���� ����
    {
        Perfect = BaseScore / GameManager.Instance.TotalNote; //�� ���� �� �ִ� ������ 1000000�� ��Ʈ ����ŭ ������
        Great = (Perfect / 100) * 70; //����Ʈ ������ 70��
        Bad = (Perfect / 100) * 30; // 30��
        Miss = 0; //����
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
                // �̸��� ��ġ�ϴ� ���� ã�Ƽ� �ش� ���� ���ڸ� score�� �Ҵ�
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
            // �̸��� �̹� ������ ���� ó��
            // ������ ������ ������Ʈ
            string[] lines = read.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(name))
                {
                    // �̸��� ��ġ�ϴ� ���� ã�Ƽ� ���� ������ ������Ʈ
                    string[] data = lines[i].Split(':');

                    if(Score < int.Parse(data[1].Trim()))
                    {
                        lines[i] = name + ":" + Score;
                        break;
                    }    

                    
                }
            }

            // ������Ʈ�� ������ ���Ͽ� ��
            File.WriteAllLines(icpPth, lines);
        }
    }
}




