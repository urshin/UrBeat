using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySongBtn : MonoBehaviour
{
    private UnityEngine.UI.Button button;


    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>(); //��ư component ��������
        button.onClick.AddListener(SelectedSong); //���ڰ� ���� �� �Լ� ȣ��

    }



    void SelectedSong()
    {

        GameManager.Instance.CurrentSongName = gameObject.name;
        LobbyManager.Instance.MusicImage.sprite = gameObject.GetComponent<Image>().sprite;
        LobbyManager.Instance.Title.text = gameObject.name;
        GameManager.Instance.FindTextFile();
        SoundManager.Instance.SongPreview();
        LobbyManager.Instance.BestScore.text = DataManager.Instance.readScore(gameObject.name).ToString();
    }

    void Update()
    {

    }
}
