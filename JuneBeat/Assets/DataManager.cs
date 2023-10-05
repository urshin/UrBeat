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
   public int isPerfect;
   public int isGreat;
   public int isBad;
   public int isMiss;





}
