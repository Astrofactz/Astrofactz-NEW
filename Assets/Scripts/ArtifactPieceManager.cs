using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPieceManager : MonoBehaviour
{
    public List<GameObject> piecesInMenu;
    public List<GameObject> piecesInSpace;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TakeFromMenu(GameObject obj)
    {
        piecesInMenu.Remove(obj);
        piecesInSpace.Add(obj);
    }

    public void BackInMenu(GameObject obj)
    {
        piecesInSpace.Remove(obj);
        piecesInMenu.Add(obj);
    }
}
