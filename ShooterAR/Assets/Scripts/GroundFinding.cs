using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class GroundFinding : MonoBehaviour
{
    public delegate void PortalReadyHandler();
    public static PortalReadyHandler OnStartGame = delegate { };
    public static PortalReadyHandler OnGroundDetected = delegate { };

    [SerializeField]
    private GameObject MyScene;

    public enum FocusState
    {
        Initializing,
        Finding,
        Found
    }

    public GameObject findingSquare;
    public GameObject foundSquare;
    public float findingSquareDist = 0.5f;

    private FocusState squareState;
    private bool floorFound = false;
    private bool portalPutted = false;


    public FocusState SquareState
    {
        get
        {
            return squareState;
        }
        set
        {
            squareState = value;
            foundSquare.SetActive(squareState == FocusState.Found);
            findingSquare.SetActive(squareState != FocusState.Found);
        }
    }

    bool trackingInitialized;

    // Use this for initialization
    void Start()
    {
        SquareState = FocusState.Initializing;
        trackingInitialized = true;
        foundSquare.GetComponent<Animator>().SetBool("StartSonar", true);
    }


    bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        if (hitResults.Count > 0)
        {
            foreach (var hitResult in hitResults)
            {
                foundSquare.transform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                foundSquare.transform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (portalPutted)
            return;
        
        if (Input.GetMouseButtonDown(0) && SquareState == FocusState.Found)
        {
            foundSquare.GetComponent<Animator>().SetBool("StartSonar", false);
            portalPutted = true;
            OnStartGame();
        }

        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, findingSquareDist);
        var screenPosition = Camera.main.ScreenToViewportPoint(center);
        ARPoint point = new ARPoint
        {
            x = screenPosition.x,
            y = screenPosition.y
        };

        // prioritize reults types
        ARHitTestResultType[] resultTypes = {
            ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent,
            ARHitTestResultType.ARHitTestResultTypeExistingPlane,
        };

        foreach (ARHitTestResultType resultType in resultTypes)
        {
            if (HitTestWithResultType(point, resultType))
            {
                if (floorFound)
                    return;
                
                MyScene.SetActive(true);
                MyScene.transform.position = new Vector3(MyScene.transform.position.x, foundSquare.transform.position.y, MyScene.transform.position.z);
                SquareState = FocusState.Found;
                OnGroundDetected();
                floorFound = true;
                return;
            }
        }


        //if you got here, we have not found a plane, so if camera is facing below horizon, display the focus "finding" square
        if (trackingInitialized)
        {
            SquareState = FocusState.Finding;

            //check camera forward is facing downward
            if (Vector3.Dot(Camera.main.transform.forward, Vector3.down) > 0)
            {

                //position the focus finding square a distance from camera and facing up
                findingSquare.transform.position = Camera.main.ScreenToWorldPoint(center);

                //vector from camera to focussquare
                Vector3 vecToCamera = findingSquare.transform.position - Camera.main.transform.position;

                //find vector that is orthogonal to camera vector and up vector
                Vector3 vecOrthogonal = Vector3.Cross(vecToCamera, Vector3.up);

                //find vector orthogonal to both above and up vector to find the forward vector in basis function
                Vector3 vecForward = Vector3.Cross(vecOrthogonal, Vector3.up);


                findingSquare.transform.rotation = Quaternion.LookRotation(vecForward, Vector3.up);

            }
            else
            {
                //we will not display finding square if camera is not facing below horizon
                findingSquare.SetActive(false);
            }

        }

    }


}
