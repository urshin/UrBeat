using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isButtonPressed = false;
    private int counter = 0;
    private Coroutine incrementCoroutine;

    // ��ư�� ���� �� ����� �޼���
    public void OnPointerDown(PointerEventData eventData)
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
            Debug.Log("Counter: " + counter);

            yield return new WaitForSeconds(1.0f); // 1�� ���
        }

        // ��ư�� �� �� �ڷ�ƾ�� �����Ǹ� incrementCoroutine�� null�� �ʱ�ȭ
        incrementCoroutine = null;
    }
}