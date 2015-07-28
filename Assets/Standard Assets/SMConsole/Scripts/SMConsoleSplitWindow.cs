#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

// Creates an adjustable bar
public class SMConsoleSplitWindow
{
  bool resize = false;
  Rect cursorChangeRect;
  private static Texture2D splitTex;

  private const float minHeight = 68.0f; // Minimum allowed height for split window
  private const float maxHeight = 197.0f; // Maximum allowed height for split window

  SMConsoleData _data;

  public SMConsoleSplitWindow()
  {
    _data = SMConsoleData.Instance;
    
  }

  // Texture of the bar
  private void createSplitTex()
  {
    splitTex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
    splitTex.SetPixel(0, 0, new Color(0.335f, 0.335f, 0.335f));
    splitTex.Apply();
  }

  // draws the texture
  public void drawWindow(float width)
  {
    if (splitTex == null)
      this.createSplitTex();

    cursorChangeRect = new Rect(0, _data.currentScrollViewHeight, width, 2.0f);

    ResizeScrollView();

  }

  // Detects input to resize area
  private void ResizeScrollView()
  {

    GUI.DrawTexture(cursorChangeRect, splitTex);
    EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeVertical);

    if (Event.current.type == EventType.mouseDown && cursorChangeRect.Contains(Event.current.mousePosition))
    {
      resize = true;
    }
    if (resize)
    {
       float height = Event.current.mousePosition.y;
       if (height < minHeight)
           height = minHeight;
       if (height > maxHeight)
           height = maxHeight;

       _data.currentScrollViewHeight = height;
      
      cursorChangeRect.Set(cursorChangeRect.x, _data.currentScrollViewHeight, cursorChangeRect.width, cursorChangeRect.height);
    }

    _data.repaint = resize; // if actively resizing it should repaint

    // Comes after since it should also repaint on the frame it stops resizing
    if (Event.current.type == EventType.MouseUp )
    { 
      resize = false;
    }

  
  }

}

#endif
