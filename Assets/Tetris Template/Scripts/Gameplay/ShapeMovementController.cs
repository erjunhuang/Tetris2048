using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ShapeMovementController : MonoBehaviour {

    public Transform rotationPivot;
	private float transitionInterval = 0.7f;
    public float fastTransitionInterval ;
	private float lastFall = 0;
 

    private bool isInstantFall = false;

    private void Start()
    {
        lastFall = Time.time;
        if (Managers.Config.isAccelerationTime == true) {
			int interval = 60;
			if (Managers.Game.stats.currentGameMode == GameMode.AccelerationMode) {
				interval = 1;
			}
			transitionInterval = transitionInterval - Mathf.Floor(Managers.Game.stats.currentGameTimeSpent / interval) * 0.1f;
            if (transitionInterval <=Managers.Config.minimumTime)
            {
                transitionInterval = Managers.Config.minimumTime;
            }
        }
    }

    public void ShapeUpdate()
    {   
        FreeFall();
    }

    public void RotateClockWise(bool isCw)
    {
        //float rotationDegree = (isCw) ? 90.0f : -90.0f;

        //transform.RotateAround(rotationPivot.position, Vector3.forward, rotationDegree);

        //// Check if it's valid          
        //if (Managers.Grid.IsValidGridPosition(this.transform)) // It's valid. Update grid.
        //{
        //    Managers.Grid.UpdateGrid(this.transform);
        //}
        //else // It's not valid. revert rotation operation.
        //{
        //    transform.RotateAround(rotationPivot.position, Vector3.forward, -rotationDegree);
        //}
    }

    public void MoveHorizontal(Vector2 direction)
    {
        
        float deltaMovement = (direction.Equals(Vector2.right)) ? 2.0f : -2.0f;

        // Modify position
        transform.position += new Vector3(deltaMovement, 0, 0);

        // Check if it's valid
        if (Managers.Grid.IsValidGridPosition(this.transform))// It's valid. Update grid.
        {
            Managers.Grid.UpdateGrid(this.transform);
        }
        else // It's not valid. revert movement operation.
        {
            transform.position += new Vector3(-deltaMovement, 0, 0);
        }
    }

    public void FreeFall()
    {
        if (Time.time - lastFall >= transitionInterval)
        {
            // Modify position
            transform.position += new Vector3(0, -2, 0);

            // See if valid
            if (Managers.Grid.IsValidGridPosition(this.transform))
            {
                if (!isInstantFall)
                {
                    Managers.Audio.Play(SoundType.Drop);
                }
                // It's valid. Update grid.
                Managers.Grid.UpdateGrid(this.transform);
            }
            else
            {
                Managers.Audio.Play(SoundType.Drop);
                // It's not valid. revert.
                transform.position += new Vector3(0, 2, 0);

                GetComponent<ShapeMovementController>().enabled = false;
                GetComponent<TetrisShape>().enabled = false;
                Managers.Game.currentShape = null;

                // Clear filled horizontal lines
				Managers.Grid.PlaceShape(this.transform);
            }

            lastFall = Time.time;
        }
    }
    
    public void InstantFall()
    {
        transitionInterval = fastTransitionInterval;
        isInstantFall = true;
    }
}
