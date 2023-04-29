using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

//pitch increase of .07 per fret is expected

[RequireComponent(typeof(LineRenderer))]
public class RopeSimulation : MonoBehaviour
{

    public AudioSource audioSource;


    [SerializeField] private Transform transPoint1;
    [SerializeField] private Transform transPoint2;

    private LineRenderer lineRenderer;

    public bool heldString = false;
    public int fretNumbers = 5;

    private float clampDistance;
    private List<float> fretPositions = new List<float>();

    public float bendLimit = 0.5f;

    public float stringSensitivity = 0.05f;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        clampDistance = transPoint2.transform.position.x - transPoint1.transform.position.x;
        for (int i= 0; i <= fretNumbers; i++)
        {
            fretPositions.Add(transPoint2.transform.position.x - (clampDistance / fretNumbers) * i);
        }
    }


    // Update is called once per frame
    void Update()
    {
        float hitDistanceFromRight = 0f;

        if (Input.GetMouseButtonDown(0) && !heldString)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if ( (transPoint2.transform.position.y + stringSensitivity) > mousePosition.y && mousePosition.y > (transPoint2.transform.position.y - stringSensitivity))
            {
                if (lineRenderer.positionCount < 3)
                {
                    lineRenderer.positionCount = 3;
                    lineRenderer.SetPosition(1, new Vector2(transPoint1.transform.position.y, mousePosition.y));
                    hitDistanceFromRight = transPoint2.transform.position.x - mousePosition.x;
                }

                float hitPercent = hitDistanceFromRight / clampDistance;

                heldString = true;
                audioSource.pitch = 1f + 0.07f *FretHit(mousePosition.x);
                audioSource.Play();
            }
        }

        if (heldString)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.y = Mathf.Clamp(mousePosition.y, transPoint1.transform.position.y - bendLimit, transPoint1.transform.position.y + bendLimit);
             

            if (mousePosition.y > transPoint1.transform.position.y -0.05)
            {
                float verticalDisplacementPerc = Mathf.Abs(transPoint1.transform.position.y - bendLimit - mousePosition.y) / bendLimit;
                audioSource.pitch = 0.07f * (verticalDisplacementPerc);
            }

            if (mousePosition.y < transPoint1.transform.position.y + 0.05)
            {
                float verticalDisplacementPerc = Mathf.Abs(transPoint1.transform.position.y + bendLimit - mousePosition.y) / bendLimit;
                audioSource.pitch = 0.07f * (verticalDisplacementPerc);
            }

            audioSource.pitch += 1f + 0.07f * FretHit(mousePosition.x);
            


            lineRenderer.SetPosition(1, mousePosition);
            heldString = true;
        }

        if (Input.GetMouseButtonUp(0) && heldString)
        {
            lineRenderer.positionCount = 2;
            heldString= false;
        }

        clampLines();
    }

    private void clampLines()
    {
        if (transPoint1 && transPoint2)
        {
            lineRenderer.SetPosition(0, transPoint1.position);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transPoint2.position);

        }
    }

    private int FretHit(float x)
    {
        int fretHitPosition = 0;
        for(int i=0; i<fretPositions.Count; i++)
        {
            if (x > fretPositions[i+1] && x < fretPositions[i])
            {
                fretHitPosition = i;
                break;
            }
        }
        return fretHitPosition;
    }
}
