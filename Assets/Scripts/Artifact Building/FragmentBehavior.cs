/*******************************************************************************
// File Name :      FragmentBehavior.cs
// Author :         Avery Macke
// Creation Date :  4 March 2022
// 
// Description :    Allows for movement, placement, and combination of artifact
                    pieces.
*******************************************************************************/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FragmentBehavior : MonoBehaviour
{
    [Tooltip("Fragment ScriptableObject")]
    public Fragment fragment;

    [Tooltip("Correct Snap Point for fragment")]
    public GameObject correctSnapPoint;

    [Tooltip("Sound that plays while dragging the fragment")]
    public AudioClip dragSnd;


    public float rotationOffset;

    /// <summary>
    /// Horizontal and vertical level bounds
    /// </summary>
    private float xBoundary, yBoundary;

    /// <summary>
    /// Z Position for pieces when idle
    /// </summary>
    private float zPosIdle;

    /// <summary>
    /// Boundary offset when looping pieces around level
    /// </summary>
    private float boundaryOffset = 0.25f;

    #region Active Movement
    /// <summary>
    /// Index of SnapPoint layer
    /// </summary>
    private int snapLayerMask;

    /// <summary>
    /// Maximum fragment movement speed
    /// </summary>
    private float maxSpeed;

    /// <summary>
    /// Speed fragment moves when dragged
    /// </summary>
    private float dragSpeed;

    /// <summary>
    /// Speed fragment moves when snapping into place
    /// </summary>
    private float snapSpeed;

    /// <summary>
    /// Speed fragment rotates
    /// </summary>
    private float rotateSpeed;
    #endregion

    #region Idle Movement
    /// <summary>
    /// Speed fragment moves when idle
    /// </summary>
    private float moveIdleSpeed;

    /// <summary>
    /// Speed fragmnet moves when thrown
    /// </summary>
    private float throwIdleSpeed;

    /// <summary>
    /// Speed fragment rotates when idle
    /// </summary>
    private float rotateIdleSpeed;
    #endregion

    /// <summary>
    /// Array of all artifact fragments in scene
    /// </summary>
    private FragmentBehavior[] fbArray;

    /// <summary>
    /// Reference to game manager in scene
    /// </summary>
    private GameManager gm;

    /// <summary>
    /// Reference to sound manager in scene
    /// </summary>
    private SoundManager sm;

    /// <summary>
    /// Audio Source attatched to the object
    /// </summary>
    private AudioSource srce;

    /// <summary>
    /// Current snap point target
    /// </summary>
    private GameObject currentSnapTarget = null;

    /// <summary>
    /// Tracks whether piece is placed in correct position
    /// </summary>
    private bool isPlaced = false;

    /// <summary>
    /// Tracks whether the piece is currently being dragged
    /// </summary>
    private bool isDragged = false;

    /// <summary>
    /// 
    /// </summary>
    private bool rotationCorrect = false;

    /// <summary>
    /// Tracks previous mouse position
    /// </summary>
    private Vector3 prevFragmentPos;

    /// <summary>
    /// Tracks whether child snap points are active
    /// </summary>
    private bool snapPointsActive = true;

    /// <summary>
    /// Starting position of piece
    /// </summary>
    private Vector3 startPos;

    /// <summary>
    /// Reference to BoxCollider component
    /// </summary>
    private BoxCollider bc;

    /// <summary>
    /// Reference to Rigidbody componend
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Called at start; initializes variables
    /// </summary>
    void Start()
    {
        InitializeVariables();
        RandomRotation();
        AddRandomForce();
    }

    /// <summary>
    /// Called once per frame; checks fragment velocity
    /// </summary>
    public void Update()
    {
        // Limits speed fragment can float while idle
        if (rb.velocity.magnitude >= maxSpeed && !isPlaced && !isDragged)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        // Checks for stationary pieces, adds force
        if (rb.velocity.magnitude <= 0.01f && !isPlaced && !isDragged)
            Invoke("AddRandomForce", 2.0f);
    }

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void InitializeVariables()
    {
        xBoundary = fragment.xBoundary;
        yBoundary = fragment.yBoundry;

        zPosIdle = fragment.zPosIdle;

        transform.position = new Vector3 (transform.position.x, transform.position.y, zPosIdle);

        startPos = transform.position;

        snapLayerMask = fragment.snapLayerMask;

        maxSpeed = fragment.maxSpeed;
        dragSpeed = fragment.dragSpeed;
        snapSpeed = fragment.snapSpeed;
        rotateSpeed = fragment.rotateSpeed;

        moveIdleSpeed = fragment.moveIdleSpeed;
        throwIdleSpeed = fragment.throwIdleSpeed;
        rotateIdleSpeed = fragment.rotateIdleSpeed;

        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        gm = FindObjectOfType<GameManager>();

        fbArray = FindObjectsOfType<FragmentBehavior>();

        ToggleSnapPoints(snapPointsActive);

        sm = FindObjectOfType<SoundManager>();
        srce = GetComponent<AudioSource>();
        srce.volume = 0;
        srce.clip = dragSnd;
        srce.playOnAwake = true;
        srce.loop = true;
        srce.Play();
    }

    /// <summary>
    /// Returns whether fragment has been correctly placed
    /// </summary>
    /// <returns>Whether fragment is placed correctly</returns>
    public bool IsPlaced()
    {
        return isPlaced;
    }

    #region Fragment interaction
    /// <summary>
    /// Called when mouse drags object; if mouse collides with snap point, snaps
    /// piece to snap point, else piece follows mouse
    /// </summary>
    private void OnMouseDrag()
    {
        if (!isPlaced && Input.GetMouseButton(1))
        {
            print("rotating");
            Rotate();
        }

        else if (!isPlaced)
            MovePiece();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnMouseDown()
    {
        StopCoroutine("MoveZPos");
    }

    /// <summary>
    /// Called when mouse is released; determines fragment behavior when mouse
    /// is released
    /// </summary>
    private void OnMouseUp()
    {
        isDragged = false;

        srce.volume = 0;

        // If has snap target
        if (currentSnapTarget)
        {
            // If correct target, combine pieces
            if (currentSnapTarget == correctSnapPoint && rotationCorrect)
            {
                CombinePieces();

                sm.Play("RightSnap");
            }

            // If incorrect target                                              // figure out what to do when pieces placed incorrectly
            else if (currentSnapTarget != correctSnapPoint)                     
            {                                                                   // if placed incorrectly anywhere in build zone, teleport it those outside bounds ?
                AddIdleForce();


                sm.Play("WrongSnap");

                StartCoroutine(MoveZPos(zPosIdle));
            }
        }
        // If does not have snap target
        else
        {
            // Throw piece, if magnitude under threshold, add random force
            AddIdleForce();
            StartCoroutine(MoveZPos(zPosIdle));

            if (rb.velocity.magnitude < 0.01f)                                  // need stop movement check for non-drag pieces too
                Invoke("AddRandomForce", 2.0f);
        }
    }
    #endregion


    #region Fragment movement and behavior
    /// <summary>
    /// Moves piece; piece follows mouse if not on snap point; if mouse is over
    /// snap point, piece snaps to snap point
    /// </summary>
    private void MovePiece()
    {
        CancelInvoke("AddRandomForce");

        isDragged = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Gets mouse position, raycasts to find snap points
        Vector3 mousePos = FindMousePos();

        Ray snapRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << snapLayerMask;

        if (Physics.Raycast(snapRay, out RaycastHit hit, Mathf.Infinity, layerMask))
            currentSnapTarget = hit.transform.gameObject;
        else
            currentSnapTarget = null;

        prevFragmentPos = transform.position;

        // If mouse on snap point, snap piece to point
        if (currentSnapTarget)
        {
            Vector3 targetPos = currentSnapTarget.transform.position;
            transform.position = Vector3.MoveTowards(transform.position,
                                 targetPos, snapSpeed * Time.deltaTime);
        }

        // Else follow mouse
        else if (!currentSnapTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                  mousePos, dragSpeed * Time.deltaTime);

            // Clamps fragment in level boundary while following mouse
            float xClamp = Mathf.Clamp(transform.position.x, -xBoundary, xBoundary);
            float yClamp = Mathf.Clamp(transform.position.y, -yBoundary, yBoundary);

            transform.position = new Vector3(xClamp, yClamp, transform.position.z);

            float t = Vector3.Distance(mousePos, transform.position);
            srce.volume = Mathf.Lerp(0.1f, 1, t);
            srce.pitch = Mathf.Lerp(0.75f, 1.25f, t);
        }
    }

    /// <summary>
    /// Moves fragment to specified target on z axis
    /// </summary>
    /// <param name="zPosTarget">target z position</param>
    IEnumerator MoveZPos(float zPosTarget)
    {
        while (transform.position.z - zPosTarget > 0.01f ||
               transform.position.z - zPosTarget < -0.01f)
        {

            Vector3 targetPos = new Vector3 (transform.position.x,
                                transform.position.y, zPosTarget);

            transform.position = Vector3.MoveTowards(transform.position, targetPos,
                                 (moveIdleSpeed / 4) * Time.deltaTime);

            yield return null;
        }

        print("z pos done");
    }


    /// <summary>
    /// Called when fragment moves outside of level boundary collider
    /// </summary>
    /// <param name="other">Other collider involved in collision</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bounds")
        {
            CheckBounds();
        }
    }

    /// <summary>
    /// Adds force to Rigidbody in a random direction; called when non-placed
    /// fragment stops moving
    /// </summary>
    private void AddRandomForce()
    {
        CancelInvoke("AddRandomForce");

        float xForce, yForce;

        xForce = Random.Range(-1.0f, 1.0f);
        yForce = Random.Range(-1.0f, 1.0f);

        Vector3 moveForce = new Vector3(xForce, yForce, 0.0f) * moveIdleSpeed
                                                              * Time.deltaTime;

        rb.AddForce(moveForce);
    }

    /// <summary>
    /// Adds force in direction fragment is thrown after dragging; called when
    /// mouse is released
    /// </summary>
    private void AddIdleForce()                                                 // work on this
    {
        Vector3 direction = transform.position - prevFragmentPos;
        direction.z = 0;

        float speed = direction.magnitude;


        Vector3 moveForce = speed * direction.normalized;

        rb.velocity = moveForce * throwIdleSpeed;
    }

    /// <summary>
    /// Checks if fragment is within level boundaries; if not, moves fragment
    /// piece to opposite side of screen
    /// </summary>
    private void CheckBounds()
    {
        Vector3 newPos = transform.position;

        float xPos = transform.position.x;
        float yPos = transform.position.y;

        if (xPos >= xBoundary)
            newPos.x = -xPos + boundaryOffset;
        else if (xPos <= -xBoundary)
            newPos.x = -xPos - boundaryOffset;
        if (yPos >= yBoundary)
            newPos.y = -yPos + boundaryOffset;
        else if (yPos <= -yBoundary)
            newPos.y = -yPos - boundaryOffset;

        transform.position = newPos;
    }

    /// <summary>
    /// Called on collision; handles knockback and collision of fragments
    /// </summary>
    /// <param name="collision">Other collider ivolved in collision</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!isDragged && collision.gameObject.tag == "Fragment")
        {
            // Applies force in opposite direction of collision
            Vector3 force = transform.position - collision.transform.position;
            force.z = 0;

            force.Normalize();

            rb.AddForce(force * moveIdleSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Finds position of mouse in world space using raycast
    /// </summary>
    /// <returns>Position of mouse in world space</returns>
    private Vector3 FindMousePos()
    {
        Vector3 mousePos = new Vector3();

        Plane plane = new Plane(Vector3.forward, 0);

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(mouseRay, out float distance))
            mousePos = mouseRay.GetPoint(distance);

        return mousePos;
    }

    /// <summary>
    /// Combines fragment when targeting correct snap point; childs fragment
    /// to snap point parent, manages snap points, and checks for win
    /// </summary>
    private void CombinePieces()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StopCoroutine("MoveZPos");

        GameObject parentArtifact = correctSnapPoint.transform.parent.gameObject;
        transform.parent = parentArtifact.transform;

        transform.position = correctSnapPoint.transform.position;

        bc.enabled = false;

        // Enable fragments snap points
        ToggleSnapPoints(snapPointsActive);

        // Disables snap point fragment is connected to
        DisableSnapPoint(correctSnapPoint);

        isPlaced = true;

        CheckArtifactComplete();
    }
    #endregion

    #region Fragment rotation

    /// <summary>
    /// 
    /// </summary>
    private void Rotate()
    {
        Vector3 mousePos = FindMousePos();

        Ray snapRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << snapLayerMask;

        if (Physics.Raycast(snapRay, out RaycastHit hit, Mathf.Infinity, layerMask))
            currentSnapTarget = hit.transform.gameObject;
        else
            currentSnapTarget = null;


        float mouseDif = transform.position.x - mousePos.x;

        Vector3 rotationDir;

        if (mouseDif > 1.0f)
        {
            rotationDir = Vector3.up;
            print("rotate right");
        }
        else if (mouseDif < -1.0f)
        {
            rotationDir = Vector3.down;
            print("rotate left");
        }
        else
            rotationDir = Vector3.zero;

        Vector3 targetRot = transform.eulerAngles + rotationDir;

        Quaternion targetRotation = Quaternion.Euler(targetRot);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if(currentSnapTarget)
        {
            if (transform.rotation.y < rotationOffset || transform.rotation.y > -rotationOffset)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
                rotationCorrect = true;
            }

            else
                rotationCorrect = false;
        }

        //get mouse position in relation to artifact

        //rotation direction = 0

        //if mouse distance.x greater than x, rotation dir = 1

        //if mouse distance.x less than x, rotation dir = -1


        // while rotation offset greater than/less than && input mouse down
        // y rot = transform.rotation.y
        // change y rot while true
        // transform.rotation = (0, yrot, 0)

        //transform.rotation = transfrom.rotation
    }


    /// <summary>
    /// 
    /// </summary>
    private void RandomRotation()
    {
        float randomRot = Random.Range(0.0f, 360.0f);

        Quaternion randomRotation = Quaternion.Euler(0, randomRot, 0);

        transform.rotation = randomRotation;
    }


    #endregion

    /// <summary>
    /// Checks if artifact has been completed; called when pieces are assembled
    /// </summary>
    /// <returns>True if artifact is complete, false if incomplete</returns>
    private void CheckArtifactComplete()
    {
        bool artifactComplete = true;

        // If any fragment is not placed, artifact is not complete
        foreach (FragmentBehavior fb in fbArray)
        {
            if (!fb.IsPlaced())
                artifactComplete = false;
        }

        if (artifactComplete)
            gm.ArtifactComplete();
    }

    /// <summary>
    /// Disables snap point after a piece has been placed at that point
    /// </summary>
    /// <param name="snapPoint">Snap point to disable</param>
    private void DisableSnapPoint(GameObject snapPoint)
    {
        snapPoint.SetActive(false);
    }

    /// <summary>
    /// Toggles childed snap points on and off
    /// </summary>
    /// <param name="snapPointStatus">Tracks if snap points are active</param>
    private void ToggleSnapPoints(bool snapPointStatus)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "SnapPoint")
                child.gameObject.SetActive(!snapPointStatus);
        }
        snapPointsActive = !snapPointsActive;
    }
}
