using System.Diagnostics;
using UnityEngine;

public class TimeScaleTracker : MonoBehaviour
{
    private float previousTimeScale;

    private void Update()
    {
        if (Time.timeScale != previousTimeScale) // 값이 변경되었을 때만 실행
        {
            StackTrace stackTrace = new StackTrace(true); // 파일과 줄 번호 포함
            UnityEngine.Debug.Log($"[TimeScaleTracker] Time.timeScale 변경: {previousTimeScale} -> {Time.timeScale}\n{stackTrace}");

            previousTimeScale = Time.timeScale;
        }
    }
}
