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

    public string filePath;
    public TextAsset textAsset;
    public string[] lines;



    public void ReadSongTxt()
    {
        filePath = "music/" + GameManager.Instance.CurrentSongAndDiff;
        textAsset = new TextAsset();
        textAsset = Resources.Load<TextAsset>(filePath);
        Debug.Log(filePath);
        lines = textAsset.text.Split('\n');
    }


}
