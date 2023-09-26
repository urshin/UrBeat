using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsingData : MonoBehaviour
{
    string ParsingName;

    private void Start()
    {
        //게임 매니저에게 현재 곡정보. 알려주기
        GameManager.Instance.CurrentSongAndDiff = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult; 
        ParsingName = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult;
    }




}
