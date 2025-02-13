using System.Diagnostics;
using UnityEngine;

public class TimeScaleTracker : MonoBehaviour
{
    private float previousTimeScale;

    private void Update()
    {
        if (Time.timeScale != previousTimeScale) // ���� ����Ǿ��� ���� ����
        {
            StackTrace stackTrace = new StackTrace(true); // ���ϰ� �� ��ȣ ����
            UnityEngine.Debug.Log($"[TimeScaleTracker] Time.timeScale ����: {previousTimeScale} -> {Time.timeScale}\n{stackTrace}");

            previousTimeScale = Time.timeScale;
        }
    }
}
