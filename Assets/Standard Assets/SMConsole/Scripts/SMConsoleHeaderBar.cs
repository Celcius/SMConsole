#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public class SMConsoleHeaderBar {


  const int SEARCH_WIDTH = 300;
  const int SEARCH_MARGIN = 10;

  bool _isFirstOnPlayClearCheck;

  SMConsoleData _data;

  public SMConsoleHeaderBar()
  {
    _data = SMConsoleData.Instance;
    _isFirstOnPlayClearCheck = true;
  }

  public void drawHeaderBar()
  {
      // Check for clear on play
      if (EditorApplication.isPlayingOrWillChangePlaymode && _data.canClearOnPlay && !_data.hasClearedOnPlay)
      {
          _data.hasClearedOnPlay = true;
          clearButton();
      }

      if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPaused)
      {
          _data.hasClearedOnPlay = false;
      }

    GUILayout.BeginHorizontal();

    // Search Field

  //  Vector2 cursorPos = Input.compositionCursorPos;

   // Rect searchFilterRect = new Rect(SEARCH_MARGIN, 1, SEARCH_WIDTH, 14);

    string prevSearch = _data.searchFilter;
    _data.searchFilter = GUILayout.TextField(_data.searchFilter, EditorStyles.textField, GUILayout.MaxWidth(SEARCH_WIDTH));

    // soft reset
    if (_data.searchFilter != SMConsoleData.DEFAULT_SEARCH_STR && prevSearch == SMConsoleData.DEFAULT_SEARCH_STR)
    {
      if (_data.searchFilter.Length < SMConsoleData.DEFAULT_SEARCH_STR.Length) // Something was removed
        _data.searchFilter = "";
      else if (_data.searchFilter.Length > SMConsoleData.DEFAULT_SEARCH_STR.Length) // Something was added
      {
        _data.searchFilter = _data.searchFilter.Trim().Replace(SMConsoleData.DEFAULT_SEARCH_STR, "");
      }
    }

    // will search
    if (_data.searchFilter != SMConsoleData.DEFAULT_SEARCH_STR && prevSearch != _data.searchFilter)
    {
      searchLogs();
    }

    if (GUILayout.Button("X", EditorStyles.toolbarButton, new GUILayoutOption[1] { GUILayout.Width(20) }))
      clearSearchButton();
    
    
    // Buttons
    GUILayout.Space(SEARCH_MARGIN * 2);

    if (GUILayout.Button("Clear", EditorStyles.toolbarButton, new GUILayoutOption[1] { GUILayout.Width(50) }))
      clearButton();

    if (GUILayout.Button("Save", EditorStyles.toolbarButton, new GUILayoutOption[1] { GUILayout.Width(50) }))
      saveButton();

    GUILayout.Space(SEARCH_MARGIN * 2);

    if (_data.canCollapse = GUILayout.Toggle(_data.canCollapse, "Collapse", "ToolbarButton", new GUILayoutOption[1] { GUILayout.Width(80) }))
      collapseButton();

    _data.canClearOnPlay = GUILayout.Toggle(_data.canClearOnPlay, "Clear on Play", "ToolbarButton", new GUILayoutOption[1] { GUILayout.Width(80) });
    if (_data.canClearOnPlay && _isFirstOnPlayClearCheck && Application.isPlaying)
    {
      _isFirstOnPlayClearCheck = false;
      clearOnPlayButton();
    }

    GUILayout.Space(SEARCH_MARGIN * 2);

    if (_data.showLogs = GUILayout.Toggle(_data.showLogs, "N(" + _data.logCounter[(int)SMLogType.NORMAL] + ")", "ToolbarButton", new GUILayoutOption[1] { GUILayout.Width(80) }))
      normalButton();

    if (_data.showWarnings = GUILayout.Toggle(_data.showWarnings, "W(" + _data.logCounter[(int)SMLogType.WARNING] + ")", "ToolbarButton", new GUILayoutOption[1] { GUILayout.Width(80) }))
      warningButton();

    if (_data.showErrors = GUILayout.Toggle(_data.showErrors, "E(" + _data.logCounter[(int)SMLogType.ERROR] + ")", "ToolbarButton", new GUILayoutOption[1] { GUILayout.Width(80) }))
      errorButton();

    GUILayout.Space(SEARCH_MARGIN * 7);

    if (GUILayout.Button("All", EditorStyles.toolbarButton, new GUILayoutOption[1] { GUILayout.Width(50) }))
      allTagsButton();

    if (GUILayout.Button("None", EditorStyles.toolbarButton, new GUILayoutOption[1] { GUILayout.Width(50) }))
      noTagsButton();


    GUILayout.EndHorizontal();

    // reset check for clear on play
    if (!_isFirstOnPlayClearCheck && !Application.isPlaying)
      _isFirstOnPlayClearCheck = true;
  }

  void clearSearchButton()
  {
    _data.showingLogs = new ArrayList(_data.logs);
    _data.searchFilter = SMConsoleData.DEFAULT_SEARCH_STR;
  }

  void searchLogs()
  {
    _data.showingLogs = new ArrayList();
    if (!_data.canCollapse)
    {
      foreach (LogMessage message in _data.logs)
        if (message.log.IndexOf(_data.searchFilter, StringComparison.OrdinalIgnoreCase) >= 0)
          _data.showingLogs.Add(message);
    }
    else
    {
      foreach (CollapsedMessage message in _data.logs)
        if (message.message.log.IndexOf(_data.searchFilter, StringComparison.OrdinalIgnoreCase) >= 0)
          _data.showingLogs.Add(message);
    }
  }

  void normalButton()
  {

  }

  void warningButton()
  {

  }
  void errorButton()
  {

  }
  void collapseButton()
  {

  }


  void clearOnPlayButton()
  {
      _data.hasClearedOnPlay = false;
  }

  void clearButton()
  {
    for (int i = 0; i < _data.logCounter.Length; i++)
      _data.logCounter[i] = 0;

    _data.showingLogs = new ArrayList();
    _data.logs = new ArrayList();
    _data.tags = new ArrayList();
    _data.selectedTags = new ArrayList();
    _data.collapsedHash = new Dictionary<string, CollapsedMessage>();

    _data.selectedLogMessage = new LogMessage();
    _data.selectedCollapsedMessage = new CollapsedMessage(new LogMessage());
  }

  void saveButton()
  {
    _data.saveLogs();
  }

  void allTagsButton()
  {
    _data.selectedTags = new ArrayList(_data.tags);
  }

  void noTagsButton()
  {
    _data.selectedTags = new ArrayList();
  }



}


#endif