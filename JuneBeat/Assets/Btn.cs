using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn : MonoBehaviour
{
    private Button button;


    void Start()
    {
        button = GetComponent<Button>(); //버튼 component 가져오기
        button.onClick.AddListener(Click); //인자가 없을 때 함수 호출

    }

    void Click() //클릭하는 버튼에 따라 다른 기능 구현
    {
        switch (gameObject.name)
        {
            case "RightBtn":
                LobbyManager.Instance.LeftMove();
                break;
            case "LeftBtn":
                LobbyManager.Instance.RightMove();
                break;
            case "NextBtn":
                LobbyManager.Instance.CreatDif();
                if (GameManager.Instance.CurrentSongName != null && GameManager.Instance.CurrentDifficult != null)
                {
                    LobbyManager.Instance.GoToInGame();
                }
                    break;
            case "BackBtn":
                LobbyManager.Instance.Back();
                break;
            case "basic(Clone)":
                GameManager.Instance.CurrentDifficult = gameObject.name.Replace("(Clone)", "");
                LobbyManager.Instance.InputSongInfo();
                break;
            case "advanced(Clone)":
                GameManager.Instance.CurrentDifficult = gameObject.name.Replace("(Clone)", "");
                LobbyManager.Instance.InputSongInfo();
                break;
            case "extreme(Clone)":
                GameManager.Instance.CurrentDifficult = gameObject.name.Replace("(Clone)", "");
                LobbyManager.Instance.InputSongInfo();
                break;

        }





    }


}
