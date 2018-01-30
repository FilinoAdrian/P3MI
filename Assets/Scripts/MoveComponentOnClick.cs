using UnityEngine;
using System.Collections;
using HoloToolkit.Unity.InputModule;

public class MoveComponentOnClick : MonoBehaviour, IInputClickHandler
{
    public enum AxisConstraint
    {
        PositiveX,
        NegativeX,
        PositiveY,
        NegativeY,
        PositiveZ,
        NegativeZ,
    }

    public AxisConstraint axisConstraint = AxisConstraint.PositiveX;

    RaycastHit hit;
    bool isTapped = false;

    public GameObject parent;
    public GameObject[] children;

    public float moveDistance = 0.125f;

    private Vector3 parentStartPos;
    private Vector3[] childStartPos;

    private Vector3 parentTargetPos;
    private Vector3[] childTargetPos;

    private void Start()
    {
        childStartPos = new Vector3[children.Length];
        childTargetPos = new Vector3[children.Length];
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        hit = GazeManager.Instance.HitInfo;
        if (hit.transform.gameObject != null)
        {
            isTapped = !isTapped;

            parentStartPos = parent.transform.position;
            for (int i = 0; i < children.Length; i++)
            {
                childStartPos[i] = children[i].transform.position;
            }

            CalculateTargetPos();

            StartCoroutine(MoveToPosition(parentStartPos, parentTargetPos, childStartPos, childTargetPos, 1f));
        }
    }

    private void CalculateTargetPos()
    {
        if (axisConstraint == AxisConstraint.PositiveX)
        {
            if (isTapped)
            {
                parentTargetPos = parentStartPos + transform.right * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + transform.right * moveDistance * 2.0f;
                }
            }
            else
            {
                parentTargetPos = parentStartPos + -transform.right * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + -transform.right * moveDistance * 2.0f;
                }
            }
        }
        else if (axisConstraint == AxisConstraint.NegativeX)
        {
            if (isTapped)
            {
                parentTargetPos = parentStartPos + -transform.right * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + -transform.right * moveDistance * 2.0f;
                }
            }
            else
            {
                parentTargetPos = parentStartPos + transform.right * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + transform.right * moveDistance * 2.0f;
                }
            }
        }
        else if (axisConstraint == AxisConstraint.PositiveY)
        {
            if (isTapped)
            {
                parentTargetPos = parentStartPos + transform.forward * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + transform.forward * moveDistance * 2.0f;
                }
            }
            else
            {
                parentTargetPos = parentStartPos + -transform.forward * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + -transform.forward * moveDistance * 2.0f;
                }
            }
        }
        else if (axisConstraint == AxisConstraint.NegativeY)
        {
            if (isTapped)
            {
                parentTargetPos = parentStartPos + -transform.forward * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + -transform.forward * moveDistance * 2.0f;
                }
            }
            else
            {
                parentTargetPos = parentStartPos + transform.forward * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + transform.forward * moveDistance * 2.0f;
                }
            }
        }
        else if (axisConstraint == AxisConstraint.PositiveZ)
        {
            if (isTapped)
            {
                parentTargetPos = parentStartPos + -transform.up * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + -transform.up * moveDistance * 2.0f;
                }
            }
            else
            {
                parentTargetPos = parentStartPos + transform.up * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + transform.up * moveDistance * 2.0f;
                }
            }
        }
        else if (axisConstraint == AxisConstraint.NegativeZ)
        {
            if (isTapped)
            {
                parentTargetPos = parentStartPos + transform.up * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + transform.up * moveDistance * 2.0f;
                }
            }
            else
            {
                parentTargetPos = parentStartPos + -transform.up * moveDistance;
                for (int i = 0; i < children.Length; i++)
                {
                    childTargetPos[i] = childStartPos[i] + -transform.up * moveDistance * 2.0f;
                }
            }
        }
    }

    IEnumerator MoveToPosition(Vector3 startPos, Vector3 newPosition, Vector3[] startPos2, Vector3[] newPosition2, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            parent.transform.position = Vector3.Lerp(startPos, newPosition, elapsedTime / time);
            for (int i = 0; i < children.Length; i++)
            {
                children[i].transform.position = Vector3.Lerp(startPos2[i], newPosition2[i], elapsedTime / time);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
