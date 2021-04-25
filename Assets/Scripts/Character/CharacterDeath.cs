using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterDeath : MonoBehaviour
{
    public event Action Died;
    
    // Start is called before the first frame update
    private Transform mainCanvas;

    void Start()
    {
        mainCanvas = GameObject.Find("HUD").transform;
    }
    public void Trigger()
    {
        GetComponent<CharacterMovement>().enabled = false;
        GetComponent<CharacterRotation>().enabled = false;
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        Died?.Invoke();
        yield break;
    }
}
