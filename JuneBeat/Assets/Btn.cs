using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Btn : Button  //버튼 상속 받기
{
    private UnityEngine.UI.Button button;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData); // 부모 클래스(Button)의 OnPointerClick 함수 호출
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
        button = GetComponent<UnityEngine.UI.Button>(); //버튼 component 가져오기
        //button.onClick.AddListener(Click); //인자가 없을 때 함수 호출

    }

    void Click() //클릭하는 버튼에 따라 다른 기능 구현
    {
        switch (gameObject.name)
        {
            case "AutoPlay":
                GameManager.Instance.autoPlay = !GameManager.Instance.autoPlay;
                Debug.Log("오토플레이" + GameManager.Instance.autoPlay);
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
