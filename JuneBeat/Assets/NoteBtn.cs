using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPerfect;
    public bool isGreat;
    public bool isBad;
    public bool isMiss;

    private bool isButtonPressed = false;
    private int counter = 0;
    private Coroutine incrementCoroutine;

    public enum NoteJudge
    {
        Perfect,
        Great,
        Bad,
        Miss,

    }

    public NoteJudge currentJudge;
    private Button button;
    // Start is called before the first frame update

    Animator animator;


    void Start()
    {
        isPerfect = false;
        isGreat = false;
        isBad = false;
        isMiss = false;
        animator = GetComponent<Animator>();
        button = GetComponent<Button>(); //버튼 component 가져오기
        //button.onClick.AddListener(judge); //인자가 없을 때 함수 호출
    }

    void AutoPlay()//오토플레이용
    {
        if(GameManager.Instance.autoPlay)
        {
            judge();
        }
    }

    // 버튼을 누를 때 실행될 메서드
    public void OnPointerDown(PointerEventData eventData)
    {
        // 버튼이 눌린 상태로 변경
        isButtonPressed = true;
        counter = 0;
        judge();
        // 이미 실행 중인 코루틴이 없다면 시작
        if (incrementCoroutine == null)
        {
            incrementCoroutine = StartCoroutine(IncrementCounter());
        }
    }

    // 버튼을 뗄 때 실행될 메서드
    public void OnPointerUp(PointerEventData eventData)
    {
        // 버튼을 뗀 상태로 변경
        isButtonPressed = false;
    }

    // 숫자를 1초에 한 번씩 증가시키는 코루틴
    private IEnumerator IncrementCounter()
    {
        while (isButtonPressed)
        {
            // 숫자를 1씩 증가시키고 디버그 로그로 출력
            counter++;
            //Debug.Log("Counter: " + counter);

            yield return new WaitForSeconds(1.0f); // 1초 대기
        }

        // 버튼을 뗄 때 코루틴이 중지되면 incrementCoroutine을 null로 초기화
        incrementCoroutine = null;
    }



    void judge()
    {
        if (currentJudge == NoteJudge.Perfect)
        {
            animator.SetBool("Perfect", true);
            DataManager.Instance.totalScore += DataManager.Instance.Perfect;
            DataManager.Instance.Combo++;
        }
        else if (currentJudge == NoteJudge.Great)
        {
            animator.SetBool("Great", true);
            DataManager.Instance.totalScore += DataManager.Instance.Great;
            DataManager.Instance.Combo++;

        }
        else if (currentJudge == NoteJudge.Bad)
        {
            animator.SetBool("Bad", true);
            DataManager.Instance.totalScore += DataManager.Instance.Bad;
            DataManager.Instance.Combo++;
        }
    }

    void Perfect()
    {
        currentJudge = NoteJudge.Perfect;
        isGreat = false;
        isPerfect = true;
    }
    void Great()
    {
        currentJudge = NoteJudge.Great;
        isBad = false;
        isGreat = true;
    }
    void Bad()
    {
        currentJudge = NoteJudge.Bad;
        isBad = true;

    }
    void Miss()
    {
        currentJudge = NoteJudge.Miss;
        isPerfect = false;
        isMiss = true;
        DataManager.Instance.Combo = 0;

        //미쓰는 아무것도 안해도 점수 ㅇㅇ 해줘얗ㅁ
    }


    void SelfDie()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {


    }
}
