using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public void Awake()
    {
        if (Instance == null) //�������� �ڽ��� üũ��, null����
        {
            Instance = this; //���� �ڱ� �ڽ��� ������.
            DontDestroyOnLoad(gameObject);//���� ��ȯ�� �Ǿ �ı����� �ʰ� ������.
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
    public void SetingScoreSystem() //���� ����
    {
        Perfect = BaseScore / GameManager.Instance.TotalNote; //�� ���� �� �ִ� ������ 1000000�� ��Ʈ ����ŭ ������
        Great = (Perfect / 100) * 70; //����Ʈ ������ 70��
        Bad = (Perfect / 100) * 30; // 30��
        Miss = 0; //����
    }






}
