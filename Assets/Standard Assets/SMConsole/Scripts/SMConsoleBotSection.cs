#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

public class SMConsoleBotSection {


  SMConsoleData _data;
  
  // Scroll Vectors for ScrollViews
  Vector2 _botLeftWindowScroll;
  Vector2 _botMidWindowScroll;
  Vector2 _botRightWindowScroll;

  // Texture for stack trace jump button
  Texture2D _warpTex;


  public SMConsoleBotSection()
  {
    _data = SMConsoleData.Instance;
    _warpTex = AssetDatabase.LoadAssetAtPath(SMConsole.ASSETS_PATH + SMConsole.SPRITES_DIR + "/warp.png", typeof(Texture2D)) as Texture2D;
  }

  public void drawBotSection(float width, float height)
  {
    // Selected Message display
    _botLeftWindowScroll = GUILayout.BeginScrollView(_botLeftWindowScroll, GUILayout.MaxHeight(height - _data.currentScrollViewHeight), GUILayout.Width(width * 0.3f));
    displaySelectedMessage(height);
    GUILayout.EndScrollView();

    // Stack trace display
    _botMidWindowScroll = GUILayout.BeginScrollView(_botMidWindowScroll, GUILayout.MaxHeight(height - _data.currentScrollViewHeight), GUILayout.Width(width * 0.7f));
    displayStackTrace(height);
    GUILayout.EndScrollView();
  }

  // displays the selected message
  void displaySelectedMessage(float height)
  {
    string selectedMessage;
    if (!_data.canCollapse)
    {
      selectedMessage = _data.selectedLogMessage.log;
    }
    else
    {
      selectedMessage = _data.selectedCollapsedMessage.message.log;
    }
    EditorGUILayout.SelectableLabel(selectedMessage, GUI.skin.label, GUILayout.MaxHeight(height - _data.currentScrollViewHeight));
  }

  // displays the stack trace of selected message
  void displayStackTrace(float height)
  {
    StackTraceEntry[] stackTrace;
    if (!_data.canCollapse)
    {
      stackTrace = _data.selectedLogMessage.stackTrace;
    }
    else
    {
      stackTrace = _data.selectedCollapsedMessage.message.stackTrace;
    }

    if (stackTrace != null)
    {
      foreach (StackTraceEntry trace in stackTrace)
      {
          if (trace.isEmpty())
              continue;

        //string lineEntry = stackTraces[stackTraces.Length - 1];

        GUILayout.BeginHorizontal(GUILayout.Height(32));

        if (trace.isEntryJumpable())
        {
          if (GUILayout.Button(_warpTex, GUILayout.Width(32), GUILayout.Height(32)))
              trace.jumpToPath();
        }

        GUIStyle skin = GUI.skin.label;
        skin.wordWrap = true;

        EditorGUILayout.SelectableLabel(trace.ToString(), skin, GUILayout.Height(32), GUILayout.MaxHeight(height - _data.currentScrollViewHeight));
        GUILayout.EndHorizontal();
      }
    }
  }
    


}

#endif
