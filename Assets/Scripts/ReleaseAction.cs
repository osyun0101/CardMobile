using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseAction : MonoBehaviour
{
    public static int countNum;
    public void Release()
    {
        var anim = GameObject.Find("ReleaseCountPanel(Clone)").transform.Find("Panel").GetComponent<Animator>();
        anim.SetBool("Release", true);
    }
}
