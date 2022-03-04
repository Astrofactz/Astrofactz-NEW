using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
// File Name :         ToolbarController.cs
// Author :            Alexander M. Griffith
// Creation Date :     March 3, 2022
//
// Brief Description : This script is used to spawn pieces when their buttons
                       are pressed. Each piece has a function (ex. SpawnAtom())
                       that instantiates a GameObject when the bool is false.
                       It then turns the bool true which prevents a second
                       piece from spawning.
*****************************************************************************/
public class ToolbarController : MonoBehaviour
{
    public GameObject atom;
    public GameObject blenderGlass;
    public GameObject nutAndBase;
    public GameObject stands;

    private bool atomBool;
    private bool blenderGlassBool;
    private bool nutAndBaseBool;
    private bool standsBool;

    /// <summary>
    /// Spawns the Atom GameObject when the Atom button is clicked.
    /// Then sets the bool to true to prevent thet object from spawning again.
    /// </summary>
    public void SpawnAtom()
    {
        if(atomBool == false)
        {
            Instantiate(atom, new Vector3(10, 0, 0), 
                atom.transform.rotation);
            atomBool = true;
        }

        if(atomBool == true)
        {
            Debug.Log("Atom already spawned.");
        }
    }

    /// <summary>
    /// Spawns the BlenderGlass GameObject when the Atom button is clicked.
    /// Then sets the bool to true to prevent thet object from spawning again.
    /// </summary>
    public void SpawnBlenderGlass()
    {
        if (blenderGlassBool == false)
        {
            Instantiate(blenderGlass, new Vector3(10, 0, 0), 
                blenderGlass.transform.rotation);
            blenderGlassBool = true;
        }

        if (blenderGlassBool == true)
        {
            Debug.Log("Blender Glass already spawned.");
        }
    }

    /// <summary>
    /// Spawns the NutAndBase GameObject when the Atom button is clicked.
    /// Then sets the bool to true to prevent thet object from spawning again.
    /// </summary>
    public void SpawnNutAndBase()
    {
        if (nutAndBaseBool == false)
        {
            Instantiate(nutAndBase, new Vector3(-5, 0, 0), 
                nutAndBase.transform.rotation);
            nutAndBaseBool = true;
        }

        if (nutAndBaseBool == true)
        {
            Debug.Log("Nut and Base already spawned.");
        }
    }

    /// <summary>
    /// Spawns the Stands GameObject when the Atom button is clicked.
    /// Then sets the bool to true to prevent thet object from spawning again.
    /// </summary>
    public void SpawnStands()
    {
        if (standsBool == false)
        {
            Instantiate(stands, new Vector3(-10, 0, 0), 
                stands.transform.rotation);
            standsBool = true;
        }

        if (standsBool == true)
        {
            Debug.Log("The Stands already spawned.");
        }
    }
}
