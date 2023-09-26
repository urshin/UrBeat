using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn : MonoBehaviour
{
    private Button button;


    void Start()
    {
        button = GetComponent<Button>(); //��ư component ��������
        button.onClick.AddListener(Click); //���ڰ� ���� �� �Լ� ȣ��

    }

    void Click() //Ŭ���ϴ� ��ư�� ���� �ٸ� ��� ����
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
