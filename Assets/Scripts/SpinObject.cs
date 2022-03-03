using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public Mesh solvedArtifact;

    public MeshFilter mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh.mesh = solvedArtifact;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0.1f, 0));
    }
}
