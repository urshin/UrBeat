using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteBtn : MonoBehaviour
{
    public bool isPerfect;
    public bool isGreat;
    public bool isBad;
    public bool isMiss;

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
        button.onClick.AddListener(judge); //���ڰ� ���� �� �Լ� ȣ��
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
