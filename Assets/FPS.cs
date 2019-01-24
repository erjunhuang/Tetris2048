
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FPS : MonoBehaviour
{
    private float m_LastUpdateShowTime = 0f;    //上一次更新帧率的时间;

    private float m_UpdateShowDeltaTime = 0.01f;//更新帧率的时间间隔;

    private int m_FrameUpdate = 0;//帧数;

    private float m_FPS = 0;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start()
    {
        m_LastUpdateShowTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {

         
        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, Screen.width, Screen.height-130);
        style.alignment = TextAnchor.LowerRight;
        style.fontSize = (int)(Screen.height * 0.06);
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        string text = string.Format("{0:0.} fps", m_FPS);
        GUI.Label(rect, text, style);

    }
}