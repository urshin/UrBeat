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
        button = GetComponent<Button>(); //��ư component ��������
        //button.onClick.AddListener(judge); //���ڰ� ���� �� �Լ� ȣ��
    }

    void AutoPlay()//�����÷��̿�
    {
        if(GameManager.Instance.autoPlay)
        {
            judge();
        }
    }

    // ��ư�� ���� �� ����� �޼���
    public void OnPointerDown(PointerEventData eventData)
    {
        // ��ư�� ���� ���·� ����
        isButtonPressed = true;
        counter = 0;
        judge();
        // �̹� ���� ���� �ڷ�ƾ�� ���ٸ� ����
        if (incrementCoroutine == null)
        {
            incrementCoroutine = StartCoroutine(IncrementCounter());
        }
    }

    // ��ư�� �� �� ����� �޼���
    public void OnPointerUp(PointerEventData eventData)
    {
        // ��ư�� �� ���·� ����
        isButtonPressed = false;
    }

    // ���ڸ� 1�ʿ� �� ���� ������Ű�� �ڷ�ƾ
    private IEnumerator IncrementCounter()
    {
        while (isButtonPressed)
        {
            // ���ڸ� 1�� ������Ű�� ����� �α׷� ���
            counter++;
            //Debug.Log("Counter: " + counter);

            yield return new WaitForSeconds(1.0f); // 1�� ���
        }

        // ��ư�� �� �� �ڷ�ƾ�� �����Ǹ� incrementCoroutine�� null�� �ʱ�ȭ
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

        //�̾��� �ƹ��͵� ���ص� ���� ���� ����餱
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
