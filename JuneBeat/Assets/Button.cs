using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isButtonPressed = false;
    private int counter = 0;
    private Coroutine incrementCoroutine;

    // 버튼을 누를 때 실행될 메서드
    public void OnPointerDown(PointerEventData eventData)
    {
        // 버튼이 눌린 상태로 변경
        isButtonPressed = true;
        counter = 0;

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
            Debug.Log("Counter: " + counter);

            yield return new WaitForSeconds(1.0f); // 1초 대기
        }

        // 버튼을 뗄 때 코루틴이 중지되면 incrementCoroutine을 null로 초기화
        incrementCoroutine = null;
    }
}