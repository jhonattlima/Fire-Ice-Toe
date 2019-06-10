using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    // Variables
    private GameObject magic = null;

    // Getters and setters
    public GameObject getMagic ()
    {
        return this.magic;
    }

    public void setMagic(GameObject magic)
    {
        magic.transform.parent = this.transform;
        this.magic = magic;
    }
}
