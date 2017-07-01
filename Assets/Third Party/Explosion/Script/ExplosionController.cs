using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ExplosionController : MonoBehaviour
{
    private Animator myAnimator;
    private bool exploded;
    public bool explode;
   
	// Use this for initialization
	private void Start ()
    {
        myAnimator = GetComponent<Animator>();
	}

    private void Update()
    {
        if (!exploded && explode)
        {
            Explode();
        }

        explode = false;
    }

    public void Explode()
    {
        myAnimator.SetTrigger("explode");
        exploded = true;
    }
}
