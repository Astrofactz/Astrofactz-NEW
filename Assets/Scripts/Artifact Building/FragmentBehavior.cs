/*******************************************************************************
// File Name :      FragmentBehavior.cs
// Author :         Avery Macke
// Creation Date :  4 March 2022
// 
// Description :    Allows for movement, placement, and combination of artifact
                    pieces.
*******************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class FragmentBehavior : MonoBehaviour
{
    [Tooltip("Fragment ScriptableObject")]
    public Fragment fragment;

    [Tooltip("Correct Snap Point for fragment")]
    public GameObject correctSnapPoint;

    [Tooltip("Sound that plays while dragging the fragment")]
    public AudioClip dragSnd;

    [Tooltip("Particle system to play when fragment placed correctly")]
    public GameObject snapParticle;

    [Tooltip("Z position offset for spawning particle effects")]
    public float zPosOffsetPS;

    #region Level Bounds
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
    private float boundaryOffset = 0.5f;
    #endregion

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
    /// Pedestal object in scene
    /// </summary>
    private GameObject pedestal;

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
    public bool isPlaced { get; set; } = false;

    /// <summary>
    /// Tracks whether the piece is currently being dragged
    /// </summary>
    private bool isDragged = false;

    /// <summary>
    /// Tracks previous mouse position when dragging
    /// </summary>
    private Vector3 prevFragmentPos;

    /// <summary>
    /// Tracks position of fragment when first dragged
    /// </summary>
    private Vector3 startDragPos;

    /// <summary>
    /// Tracks whether piece is in artifact build zone
    /// </summary>
    private bool inBuildZone = false;

    /// <summary>
    /// Tracks whether child snap points are active
    /// </summary>
    private bool snapPointsActive = true;

    /// <summary>
    /// Default and selected outline shaders
    /// </summary>
    private GameObject outlineIdle, outlineSelected;

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
        AddRandomRotation();
        AddRandomForce();
    }

    /// <summary>
    /// Called once per frame; checks fragment velocity
    /// </summary>
    public void Update()
    {
        CheckBounds();

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
        pedestal = GameObject.FindGameObjectWithTag("Pedestal");

        outlineIdle = transform.Find("outlineIdle").gameObject;

        outlineSelected = transform.Find("outlineSelected").gameObject;

        ToggleSnapPoints(snapPointsActive);

        // Audio
        AudioMixer mixer = Resources.Load("Mixer") as AudioMixer;
        sm = FindObjectOfType<SoundManager>();
        srce = GetComponent<AudioSource>();
        srce.volume = 0;
        srce.clip = dragSnd;
        srce.playOnAwake = true;
        srce.loop = true;
        srce.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        srce.Play();
    }

    #region Fragment interaction
    /// <summary>
    /// Called when mouse clicks object; rotates object, enables selected shader
    /// </summary>
    private void OnMouseDown()
    {
        if (!isPlaced)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(MatchTargetRot());

            outlineIdle.SetActive(false);
            outlineSelected.SetActive(true);
        }
    }

    /// <summary>
    /// Called when mouse is over object; enabled selected shader
    /// </summary>
    private void OnMouseEnter()
    {
        if(!gm.isDraggingPiece)
        {
            outlineIdle.SetActive(false);
            outlineSelected.SetActive(true);
        }
    }

    /// <summary>
    /// Called when mouse exits object; disables selected shader
    /// </summary>
    private void OnMouseExit()
    {
        if (!isDragged)
        {
            outlineIdle.SetActive(true);
            outlineSelected.SetActive(false);
        }
    }

    /// <summary>
    /// Called when mouse drags object; if mouse collides with snap point, snaps
    /// piece to snap point, else piece follows mouse
    /// </summary>
    private void OnMouseDrag()
    {
        if (!isPlaced)
            MovePiece();
    }

    /// <summary>
    /// Called when mouse is released; determines fragment behavior when mouse
    /// is released
    /// </summary>
    private void OnMouseUp()
    {
        gm.isDraggingPiece = false;
         outlineIdle.SetActive(true);
         outlineSelected.SetActive(false);

        isDragged = false;
        srce.volume = 0;

        // If has snap target
        if (currentSnapTarget)
        {
            // If correct target, combine pieces
            if (currentSnapTarget == correctSnapPoint)
            {
                CombinePieces();
            }

            // If incorrect target
            else if (currentSnapTarget != correctSnapPoint)
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = RigidbodyConstraints.FreezePositionZ;
                AddIdleForce();
                sm.Play("WrongSnap");

                StartCoroutine(MovePos(startDragPos, 0.2f));
            }
        }
        // If does not have snap target
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;

            // Throw piece, if magnitude under threshold, add random force
            AddIdleForce();

            // if in build zone, target outside of zone; else wherever is released
            Vector3 target;

            if (inBuildZone)
                target = startDragPos;

            else
                target = new Vector3 (transform.position.x, transform.position.y, zPosIdle);

            StartCoroutine(MovePos(target, 0.2f));

            if (rb.velocity.magnitude < 0.01f)                                  // need stop movement check for non-drag pieces too
                Invoke("AddRandomForce", 1.5f);

            Invoke("AddRandomRotation", 0.25f);
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
        gm.isDraggingPiece = true;

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
            t /= dragSpeed * Time.deltaTime;

            srce.volume = 0.5f;
            srce.pitch = Mathf.Lerp(0.5f, 1.25f, t);
        }
    }

    /// <summary>
    /// Moves fragment to specified target on z axis
    /// </summary>
    /// <param name="zPosTarget">target z position</param>
    IEnumerator MovePos(Vector3 targetPos, float moveSpeedMod)
    {
        float dist = Vector3.Distance(transform.position, targetPos);

        while (dist > 0.01f || dist < -0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos,
                                 (moveIdleSpeed * moveSpeedMod) * Time.deltaTime);

            dist = Vector3.Distance(transform.position, targetPos);

            yield return null;
        }
    }

    /// <summary>
    /// Called when fragment colliders with level boundary collider
    /// </summary>
    /// <param name="other">Other collider involved in collision</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bounds")
        {
            print("hit bounds");
            CheckBounds();
        }

        else if (other.gameObject.tag == "Build Zone")
        {
            inBuildZone = true;
            startDragPos = transform.position;
        }
    }

    /// <summary>
    /// Called when fragment leaves collider
    /// </summary>
    /// <param name="other">Other collider involved in collision</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Build Zone")
            inBuildZone = false;
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
                                                              / 100;

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
    /// <param name="collision">Other collider involved in collision</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!isDragged && collision.gameObject.tag == "Fragment")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Applies force in opposite direction of collision
            Vector3 force = transform.position - collision.transform.position;
            force.z = 0;

            force.Normalize();

            rb.AddForce(force * moveIdleSpeed / 100);
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

        bc.enabled = false;

        SpawnPS();
        sm.Play("RightSnap");

        // Enable fragments snap points
        ToggleSnapPoints(snapPointsActive);

        // Disables snap point fragment is connected to
        DisableSnapPoint(correctSnapPoint);

        isPlaced = true;

        transform.position = correctSnapPoint.transform.position;

        gm.CheckProgress();

        CheckArtifactComplete();
    }
    #endregion

    #region Fragment rotation
    /// <summary>
    /// Adds random rotatoinal force on all axes
    /// </summary>
    private void AddRandomRotation()
    {
        float xForce, yForce, zForce;

        xForce = Random.Range(-1.0f, 1.0f);
        yForce = Random.Range(-1.0f, 1.0f);
        zForce = Random.Range(-1.0f, 1.0f);

        Vector3 rotForce = new Vector3(xForce, yForce, zForce);

        rb.AddTorque(rotForce * rotateIdleSpeed / 100);
    }

    /// <summary>
    /// Rotates fragment to match rotation of base artifact on pedestal
    /// </summary>
    IEnumerator MatchTargetRot()
    {
        Quaternion targetRot = pedestal.transform.rotation;

        float rotDiff = (transform.rotation.eulerAngles - targetRot.eulerAngles).magnitude;

        while ((rotDiff > 0.1f || rotDiff < -0.1f) && rotDiff != 360)
        {
            Quaternion target = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed);

            transform.rotation = target;

            rotDiff = (transform.rotation.eulerAngles - targetRot.eulerAngles).magnitude;

            yield return null;
        }

        transform.rotation = targetRot;
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
            if (!fb.isPlaced)
                artifactComplete = false;

            //StartCoroutine(CorrectSnapUI());
            // call snap pop-up
        }

        if (artifactComplete && !gm.gameWon)
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

    ///<summary>
    /// Spawns snap particle effect; called when fragment placed correctly
    /// </summary>
    private void SpawnPS()
    {
        Vector3 offset = new Vector3(0.0f, 0.0f, zPosOffsetPS);

        Instantiate(snapParticle, transform.position - offset,
                    Quaternion.identity);
    }
}
