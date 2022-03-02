
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

    /// <summary>
    /// TakeFromMenu is called when an object is removed from the menu.
    /// </summary>
    /// <param name="obj">object to be removed</param>
    public void TakeFromMenu(GameObject obj)
    {
        piecesInMenu.Remove(obj);
        piecesInSpace.Add(obj);
    }

    /// <summary>
    /// BackInMenu is called when an object is put back in the menu.
    /// </summary>
    /// <param name="obj">object to be added</param>
    public void BackInMenu(GameObject obj)
    {
        piecesInSpace.Remove(obj);
        piecesInMenu.Add(obj);
    }
}
