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

    public enum NoteJud
    {
        Perfect,
        Great,
        Bad,
        Miss,

    }

    public NoteJud currentJudge;
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
        switch (currentJudge)
        {
            case NoteJud.Perfect:
                animator.SetBool("Perfect",true);
                
                break;

            case NoteJud.Great:
                animator.SetBool("Great", true);
                break;

            case NoteJud.Bad:
                animator.SetBool("Bad", true);
                break;


        }
    }

    void Perfect()
    {
        currentJudge = NoteJud.Perfect;
        isGreat = false;
        isPerfect = true;
    }
    void Great()
    {
        currentJudge = NoteJud.Great;
        isBad = false;
        isGreat = true;
    }
    void Bad()
    {
        currentJudge = NoteJud.Bad;
        isBad = true;

    }
    void Miss()
    {
        currentJudge = NoteJud.Miss;
        isPerfect = false;
        isMiss = true;
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
