//  /*********************************************************************************
//   *********************************************************************************
//   *********************************************************************************
//   * Produced by Skard Games										                  *
//   * Facebook: https://goo.gl/5YSrKw											      *
//   * Contact me: https://goo.gl/y5awt4								              *											
//   * Developed by Cavit Baturalp Gürdin: https://tr.linkedin.com/in/baturalpgurdin *
//   *********************************************************************************
//   *********************************************************************************
//   *********************************************************************************/

using UnityEngine;
using System.Collections;

public enum InputMethod
{
    KeyboardInput,
    MouseInput,
    TouchInput
}

public class PlayerInputManager : MonoBehaviour
{
    public bool isActive;
    public InputMethod inputType;
    public int OneScreenMove = 6;
    void Update()
    {
		if (isActive&&Managers.Game.currentShape&& Managers.Game.State.ToString()== "GamePlayState")
        {
            if (inputType == InputMethod.KeyboardInput)
                KeyboardInput();
            else if (inputType == InputMethod.MouseInput)
                MouseInput();
            else if (inputType == InputMethod.TouchInput)
                TouchInput();
        }
    }

    #region KEYBOARD
    void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.UpArrow))
            Managers.Game.currentShape.movementController.RotateClockWise(false);
        else if (Input.GetKeyDown(KeyCode.D))
            Managers.Game.currentShape.movementController.RotateClockWise(true);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Managers.Game.currentShape != null)
            {
                isActive = false;
                Managers.Game.currentShape.movementController.InstantFall();
            }
        }
    }
    #endregion

    #region MOUSE
    Vector2 _startPressPosition;
    Vector2 _endPressPosition;
    Vector2 _currentSwipe;
    float _buttonDownPhaseStart;
    public float tapInterval;

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            _startPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            _buttonDownPhaseStart = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - _buttonDownPhaseStart > tapInterval)
            {
                //save ended touch 2d point
                _endPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //create vector from the two points
                _currentSwipe = new Vector2(_endPressPosition.x - _startPressPosition.x, _endPressPosition.y - _startPressPosition.y);

                //normalize the 2d vector
                _currentSwipe.Normalize();

                //swipe left
                if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
                }
                //swipe right
                if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                {
                    Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
                }

                //swipe down
                if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                {
                    if (Managers.Game.currentShape != null)
                    {
                        isActive = false;
                        Managers.Game.currentShape.movementController.InstantFall();
                    }
                }
            }
            else
            {
                if (_startPressPosition.x < Screen.width / 2)
                    Managers.Game.currentShape.movementController.RotateClockWise(false);
                else
                    Managers.Game.currentShape.movementController.RotateClockWise(true);
            }
        }
    }
    #endregion

    bool isLongMove; 
    #region TOUCH
    void TouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _startPressPosition = touch.position;
                _endPressPosition = touch.position;
                _buttonDownPhaseStart = Time.time;
                isLongMove=false;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                _buttonDownPhaseStart = Time.time;

                //save ended touch 2d point
                _endPressPosition = new Vector2(touch.position.x, touch.position.y);

                //create vector from the two points
                _currentSwipe = new Vector2(_endPressPosition.x - _startPressPosition.x, _endPressPosition.y - _startPressPosition.y);

                if (Mathf.Abs(_currentSwipe.x)>Screen.width/ OneScreenMove)
                {
                    isLongMove=true;
                    _startPressPosition = touch.position;
                    
                    //normalize the 2d vector
                    _currentSwipe.Normalize();

                    //swipe left
                    if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                    {
                        Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
                    }
                    //swipe right
                    if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                    {
                        Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (Time.time - _buttonDownPhaseStart > 0)
                {
                    //save ended touch 2d point
                    _endPressPosition = new Vector2(touch.position.x, touch.position.y);

                    //create vector from the two points
                    _currentSwipe = new Vector2(_endPressPosition.x - _startPressPosition.x, _endPressPosition.y - _startPressPosition.y);

                    //normalize the 2d vector
                    _currentSwipe.Normalize();
                    
                    if(!isLongMove){
                         //swipe left
                        if (_currentSwipe.x < 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                        {
                            Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
                        }
                        //swipe right
                        if (_currentSwipe.x > 0 && _currentSwipe.y > -0.5f && _currentSwipe.y < 0.5f)
                        {
                            Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
                        }
                    }

                    //swipe down
                    if (_currentSwipe.y < 0 && _currentSwipe.x > -0.5f && _currentSwipe.x < 0.5f)
                    {
                        if (Managers.Game.currentShape != null)
                        {
                            isActive = false;
                            Managers.Game.currentShape.movementController.InstantFall();
                        }
                    }
                }
                else /*if (_currentSwipe.x + _currentSwipe.y< 0.5f */
                {
                    if (_startPressPosition.x < Screen.width / 2)
                        Managers.Game.currentShape.movementController.RotateClockWise(false);
                    else
                        Managers.Game.currentShape.movementController.RotateClockWise(true);
                }
            }
        }

    }
    //protected Vector2 m_StartingTouch;
    //protected bool m_IsSwiping = false;
    //void TouchInput() {
    //    // Use touch input on mobile
    //    if (Input.touchCount == 1)
    //    {
    //        if (m_IsSwiping)
    //        {
    //            Vector2 diff = Input.GetTouch(0).position - m_StartingTouch;

    //            // Put difference in Screen ratio, but using only width, so the ratio is the same on both
    //            // axes (otherwise we would have to swipe more vertically...)
    //            diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);

    //            if (diff.magnitude > 0.01f) //we set the swip distance to trigger movement to 1% of the screen width
    //            {
    //                if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
    //                {
    //                    if (diff.y < 0)
    //                    {
    //                        //Slide();
    //                        if (Managers.Game.currentShape != null)
    //                        {
    //                            isActive = false;
    //                            Managers.Game.currentShape.movementController.InstantFall();
    //                        }
    //                    }
    //                    else
    //                    {
    //                        //Jump();
    //                    }
    //                }
    //                else
    //                {
    //                    if (diff.x < 0)
    //                    {
    //                        //ChangeLane(-1);
    //                        Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.left);
    //                    }
    //                    else
    //                    {
    //                        Managers.Game.currentShape.movementController.MoveHorizontal(Vector2.right);
    //                    }
    //                }

    //                m_IsSwiping = false;
    //            }
    //        }

    //        // Input check is AFTER the swip test, that way if TouchPhase.Ended happen a single frame after the Began Phase
    //        // a swipe can still be registered (otherwise, m_IsSwiping will be set to false and the test wouldn't happen for that began-Ended pair)
    //        if (Input.GetTouch(0).phase == TouchPhase.Began)
    //        {
    //            m_StartingTouch = Input.GetTouch(0).position;
    //            m_IsSwiping = true;
    //        }
    //        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
    //        {
    //            m_IsSwiping = false;
    //        }
    //    }
    //}
    #endregion

}
