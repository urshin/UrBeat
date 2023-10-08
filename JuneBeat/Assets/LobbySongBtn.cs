using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySongBtn : MonoBehaviour
{
    private Button button;


    void Start()
    {
        button = GetComponent<Button>(); //��ư component ��������
        button.onClick.AddListener(SelectedSong); //���ڰ� ���� �� �Լ� ȣ��

    }



    void SelectedSong()
    {

        GameManager.Instance.CurrentSongName = gameObject.name;
        LobbyManager.Instance.MusicImage.sprite = gameObject.GetComponent<Image>().sprite;
        LobbyManager.Instance.Title.text = gameObject.name;
        GameManager.Instance.FindTextFile();
        SoundManager.Instance.SongPreview();
    }

    void Update()
    {

    }
}
