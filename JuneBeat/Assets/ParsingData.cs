using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsingData : MonoBehaviour
{
    string ParsingName;

    private void Start()
    {
        //���� �Ŵ������� ���� ������. �˷��ֱ�
        GameManager.Instance.CurrentSongAndDiff = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult; 
        ParsingName = GameManager.Instance.CurrentSongName + "_" + GameManager.Instance.CurrentDifficult;
    }




}
