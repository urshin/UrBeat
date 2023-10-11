using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Btn : Button  //��ư ��� �ޱ�
{
    private UnityEngine.UI.Button button;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData); // �θ� Ŭ����(Button)�� OnPointerClick �Լ� ȣ��
        Click();
        
        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }
    public override void KeepPush()
    {
        base.KeepPush();
        Click();
    }


    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>(); //��ư component ��������
        //button.onClick.AddListener(Click); //���ڰ� ���� �� �Լ� ȣ��

    }

    void Click() //Ŭ���ϴ� ��ư�� ���� �ٸ� ��� ����
    {
        switch (gameObject.name)
        {
            case "AutoPlay":
                GameManager.Instance.autoPlay = !GameManager.Instance.autoPlay;
                Debug.Log("�����÷���" + GameManager.Instance.autoPlay);
                break;
            case "RightBtn":
                LobbyManager.Instance.LeftMove();
                SoundManager.Instance.EffectPlay("left");

                break;
            case "LeftBtn":
                LobbyManager.Instance.RightMove();
                SoundManager.Instance.EffectPlay("right");
                break;
            case "NextBtn":
                if (GameManager.Instance.currentState == CurrentState.LobbySongSelect && GameManager.Instance.CurrentSongName != "" )
                {
                    LobbyManager.Instance.CreatDif();
                    SoundManager.Instance.EffectPlay("toggle_on");
                }
                else if (GameManager.Instance.currentState == CurrentState.LobbyDifficultSelect && GameManager.Instance.CurrentDifficult != "")
                {
                    LobbyManager.Instance.GoToInGame();
                    SoundManager.Instance.EffectPlay("toggle_on");
                }
                break;
            case "NextBtn(Clone)":
                if(GameManager.Instance.currentState == CurrentState.result)
                {
                    GameManager.Instance.GoToScene("Lobby");
                    SoundManager.Instance.EffectPlay("toggle_on");
                }
                    break;
            case "BackBtn":
                LobbyManager.Instance.Back();
                SoundManager.Instance.EffectPlay("toggle_off");
                break;
            case "basic(Clone)":
                GameManager.Instance.CurrentDifficult = gameObject.name.Replace("(Clone)", "");
                LobbyManager.Instance.InputSongInfo();
                SoundManager.Instance.EffectPlay("basic");
                break;
            case "advanced(Clone)":
                GameManager.Instance.CurrentDifficult = gameObject.name.Replace("(Clone)", "");
                LobbyManager.Instance.InputSongInfo();
                SoundManager.Instance.EffectPlay("advanced");
                break;
            case "extreme(Clone)":
                GameManager.Instance.CurrentDifficult = gameObject.name.Replace("(Clone)", "");
                LobbyManager.Instance.InputSongInfo();
                SoundManager.Instance.EffectPlay("extreme");
                break;

        }





    }


}
