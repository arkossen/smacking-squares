using System;
using System.Collections.Generic;
using UnityEngine;

internal struct LogItem
{
    public String Log;
    public String StackTrace;
    public Color Color;
}

public class DebugConsole : MonoBehaviour
{
    private Vector2 scrollPosition;

    private bool showConsole = false;
    private bool showStack = true;
    private bool locked = false;

    private List<LogItem> logs = new List<LogItem>();

    public Vector2 TogglePosition = new Vector2(0, 0);
    public float FontSize = 12;
    public float Alpha = 1;
    public int MaxLogItems = 10;

    private void Awake() {
        Application.RegisterLogCallback(HandleLog);
    }

    private void HandleLog(string logString, string stackTrace, LogType type) {
        LogItem log = new LogItem();
        log.Log = logString;
        log.StackTrace = stackTrace;

        switch (type) {
            case LogType.Assert:
                log.Color = Color.red;
                break;

            case LogType.Error:
                log.Color = Color.red;
                break;

            case LogType.Exception:
                log.Color = Color.red;
                break;

            case LogType.Log:
                log.Color = Color.white;
                break;

            case LogType.Warning:
                log.Color = Color.yellow;
                break;
        }
        logs.Add(log);
        if (logs.Count > MaxLogItems) {
            logs.RemoveAt(0);
        }
    }

    private void OnGUI() {
        GUI.skin.label.fontSize = 16;
        GUI.skin.horizontalSlider.fontSize = 16;
        GUI.skin.button.fontSize = 16;
        GUI.skin.toggle.fontSize = 16;
        GUI.color = new Color(1, 1, 1, Alpha);
        GUI.backgroundColor = GUI.color;
        GUI.contentColor = GUI.color;

        if (!showConsole) {
            showConsole = GUI.Toggle(new Rect(TogglePosition.x, TogglePosition.y, 130, 25), showConsole, "Show console");
        } else if (showConsole) {
            GUI.Box(new Rect(0, 2, Screen.width, 230), "");
            showConsole = GUI.Toggle(new Rect(TogglePosition.x, TogglePosition.y, 130, 25), showConsole, "Show console");

            showStack = GUI.Toggle(new Rect(140, 2, 155, 25), showStack, "Show Stack Traces");

            if (GUI.Button(new Rect(300, 2, 70, 25), "Clear")) {
                logs.Clear();
            }

            GUI.Label(new Rect(380, 2, 140, 25), "Font Size: ");

            GUI.color = Color.green;
            FontSize = GUI.HorizontalSlider(new Rect(460, 8, 100, 25), FontSize, 10, 20);
            GUI.color = Color.white;

            GUI.Label(new Rect(590, 2, 130, 25), "Transparency: ");

            GUI.color = Color.green;
            Alpha = GUI.HorizontalSlider(new Rect(710, 8, 100, 25), Alpha, 0.2f, 1f);
            GUI.color = Color.white;

            GUI.skin.textArea.fontSize = (int)FontSize;
            locked = GUI.Toggle(new Rect(Screen.width - 70, 0, 50, 25), locked, "Lock");

            GUILayout.Label("");

            if (locked) {
                scrollPosition = new Vector2(0, Int32.MaxValue);
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(200));

            foreach (LogItem log in logs) {
                GUI.color = log.Color;
                if (showStack) {
                    GUILayout.TextArea(log.Log + "\n" + log.StackTrace);
                } else {
                    GUILayout.TextArea(log.Log);
                }
                GUI.color = Color.white;
            }
            GUILayout.EndScrollView();
        }
    }
}