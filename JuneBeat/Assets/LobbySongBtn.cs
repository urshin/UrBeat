using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySongBtn : MonoBehaviour
{
    private Button button;


    void Start()
    {
        button = GetComponent<Button>(); //버튼 component 가져오기
        button.onClick.AddListener(SelectedSong); //인자가 없을 때 함수 호출

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
