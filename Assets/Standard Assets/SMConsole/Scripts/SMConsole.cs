#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

/*
 * SMConsole, a cutomized console with multiple functions
 * To Log call the several SMConsole.Log functions
 */

#if UNITY_EDITOR
public class SMConsole : EditorWindow
#else
public class SMConsole
#endif
{
  #if UNITY_EDITOR
  // Console components
  SMConsoleHeaderBar _headerBar;
  SMConsoleTopSection _topSection;
  SMConsoleSplitWindow _splitWindow;
  SMConsoleBotSection _botSection;
  SMConsoleData _data;  // Asset paths

  public const string ASSETS_PATH = "Assets/Standard Assets/SMConsole/";
  public const string SKINS_DIR = "Skins";
  public const string SPRITES_DIR = "Sprites";

  [MenuItem("Window/SM Console")]
  public static void ShowWindow()
  {
    
    //Show existing window instance. If one doesn't exist, make one.
    EditorWindow.GetWindow(typeof(SMConsole));
   

  }

  public void OnEnable()
  {
    // Create the singleton and intialize
    _data = SMConsoleData.Instance;
    _data.currentScrollViewHeight = this.position.height / 2;
    _data.mainEditorConsole = this;

    Application.logMessageReceivedThreaded += systemLogReceiver;
    //Application.RegisterLogCallback(SMConsoleData.Instance.HandleLog);
    //Application.RegisterLogCallbackThreaded(SMConsoleData.Instance.HandleLog);

     // Init components
    _headerBar = new SMConsoleHeaderBar();
    _topSection = new SMConsoleTopSection();
    _splitWindow = new SMConsoleSplitWindow();
    _botSection = new SMConsoleBotSection();
  }

   void OnDisable()
  {
      Application.logMessageReceivedThreaded -= systemLogReceiver;
  }

  // Draw Editor
  public void OnGUI()
  {

    EditorGUILayout.BeginVertical();
    title = "SM Console";

    EditorGUILayout.EndVertical();
      
    _headerBar.drawHeaderBar();
    // HEADER

    // TOP
    GUILayout.BeginVertical();

    _topSection.drawTopSection(this.position.width, this.position.height);

      
      SMConsoleData data = SMConsoleData.Instance;
      if((!data.canCollapse && data.selectedLogMessage.hashKey() != new LogMessage().hashKey())
          || (data.canCollapse && data.selectedCollapsedMessage.message.hashKey() != new LogMessage().hashKey()))
      {
     
      
    // MID
    _splitWindow.drawWindow(this.position.width);
     
    // BOT
    GUILayout.BeginHorizontal();

    _botSection.drawBotSection(this.position.width, this.position.height);
          
             GUILayout.EndHorizontal();
        }
    GUILayout.EndVertical();

    if (_data.repaint)
        Repaint();
      
 
   }

    public void systemLogReceiver  (String logString, String stackTrace, LogType type) {
        SMConsole.Log(logString, SMConsoleData.SYSTEM_TAG, convertLogTypes(type));
	}

 #endif

  #region static Log functions
  public static void Log(string log)
  {
    Log(log, SMConsoleData.EMPTY_TAG);
  }

  public static void Log(string log, string tag)
  {
    Log(log, tag, SMLogType.NORMAL);
  }

  public static void Log(string log, string tag, SMLogType type)
  {
      Log(log, tag, type, StackTraceEntry.EMPTY_STACK_TRACE);
  }

   private static SMLogType convertLogTypes(LogType type)
  {
       switch(type)
       {
           case  LogType.Error :
           case LogType.Exception:
           case LogType.Assert:
               return SMLogType.ERROR;
           case  LogType.Warning:
              return SMLogType.WARNING;
           case  LogType.Log:
              return SMLogType.NORMAL;   
       }
       return SMLogType.NORMAL;
  }
  private static void Log(string log, string tag, SMLogType type, string stackTrace)
  {
#if UNITY_EDITOR
    LogMessage message;
    SMConsoleData _data = SMConsoleData.Instance;

    if (stackTrace == StackTraceEntry.EMPTY_STACK_TRACE)
      message = new LogMessage(log, tag, type,Environment.StackTrace);
    else
      message = new LogMessage(log, tag, type, stackTrace);

    _data.logs.Add(message);

    _data.logCounter[(int)type]++;

    string hashKey = message.hashKey();

    if (!_data.collapsedHash.ContainsKey(hashKey))
    {
      CollapsedMessage collapsed = new CollapsedMessage(message);
      _data.collapsedHash.Add(hashKey, collapsed);
    }
    else
    {
      CollapsedMessage collapsed = _data.collapsedHash[hashKey];
      collapsed.counter++;
      _data.collapsedHash[hashKey] = collapsed;
    }

    if (!_data.tags.Contains(message.tag))
    {
      _data.tags.Add(message.tag);
      _data.selectedTags.Add(message.tag);
    }

    // See if should be added to currently showing
     if (_data.searchFilter == SMConsoleData.DEFAULT_SEARCH_STR || _data.searchFilter == "")
    {
      _data.showingLogs.Add(message);
    }
    else
    {
       if(!_data.canCollapse)
       {
         if(message.log.IndexOf(_data.searchFilter, StringComparison.OrdinalIgnoreCase) >= 0)
            _data.showingLogs.Add(message);
       }
       else if(_data.collapsedHash[hashKey].counter == 1) // first instance
       {
         if (message.log.IndexOf(_data.searchFilter, StringComparison.OrdinalIgnoreCase) >= 0)
           _data.showingLogs.Add(_data.collapsedHash[hashKey]);
       }
    }
        if(_data.mainEditorConsole != null)
        {
            _data.mainEditorConsole.Repaint();
        }

#endif
  }

  public static void LogError(string log)
  {
      Log(log, SMConsoleData.EMPTY_TAG, SMLogType.ERROR);
  }

  public static void LogError(string log, string tag)
  {
      Log(log, tag, SMLogType.ERROR);
  }

  public static void LogWarning(string log)
  {
      Log(log, SMConsoleData.EMPTY_TAG, SMLogType.WARNING);
  }

  public static void LogWarning(string log, string tag)
  {
      Log(log, tag, SMLogType.WARNING);
  }
  #endregion

}
