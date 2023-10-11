//�� ��ư���� override�����ϰԲ� ������ ������ֱ�
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public  class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isButtonPressed = false;
    private int counter = 0;
    private Coroutine incrementCoroutine;
    //private float waitingTime;

    public void Start()
    {
        //waitingTime = 1f;
    }

    // ��ư�� ���� �� ����� �޼���
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // ��ư�� ���� ���·� ����
        isButtonPressed = true;
        counter = 0;

        // �̹� ���� ���� �ڷ�ƾ�� ���ٸ� ����
        if (incrementCoroutine == null)
        {
            incrementCoroutine = StartCoroutine(IncrementCounter());
        }
    }

    // ��ư�� �� �� ����� �޼���
    public virtual void  OnPointerUp(PointerEventData eventData)
    {
        // ��ư�� �� ���·� ����
        isButtonPressed = false;
    }

    // ���ڸ� 1�ʿ� �� ���� ������Ű�� �ڷ�ƾ
    public virtual IEnumerator IncrementCounter()
    {
        
        while (isButtonPressed&& counter >0)
        {
            // ���ڸ� 1�� ������Ű�� ����� �α׷� ���
            counter++;
            Debug.Log("Counter: " + counter);
            KeepPush();
            yield return new WaitForSeconds(0.1f); // 0.5�� ���
        }

        // ��ư�� �� �� �ڷ�ƾ�� �����Ǹ� incrementCoroutine�� null�� �ʱ�ȭ
        incrementCoroutine = null;
    }
    public virtual void KeepPush()
    {
      
    }
}