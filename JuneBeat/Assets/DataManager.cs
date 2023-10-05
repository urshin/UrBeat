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
   public int isPerfect;
   public int isGreat;
   public int isBad;
   public int isMiss;





}
