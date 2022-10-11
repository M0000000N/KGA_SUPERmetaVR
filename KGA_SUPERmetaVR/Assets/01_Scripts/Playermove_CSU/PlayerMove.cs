using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using OVR; 

public class PlayerMove : MonoBehaviourPun
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float runSpeed;
    private float applySpeed; 

    public bool isRun = false;

    [SerializeField]
    private GameObject peekaboo;

    [SerializeField]
    private Stamina stamina; 

    //플레이어 이동
    private float dirX = 0;
    private float dirZ = 0; 

    private void Start()
    {
        applySpeed = walkSpeed;
        peekaboo.SetActive(false);
       
    }

    private void Update()
    {
        TryRun();
        walk();
        Attack();
       // Attack2();
    }

    // 플레이어 이동
    public void walk()
    {
        dirX = 0; // 좌우
        dirZ = 0; // 상하 
       
        if(OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            Vector2 pos = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            var absX = Mathf.Abs(pos.x);
            var absY = Mathf.Abs(pos.y);

            if(absX > absY)
            {
                //right
                if (pos.x > 0)
                    dirX = +1;
                //left
                else
                    dirX = -1; 
            }
            else
            {
                //up
                if (pos.y > 0)
                    dirZ = +1;
                //down
                else
                    dirZ = -1; 
            }

            // 이동방향 설정 후 이동
            Vector3 moveDir = new Vector3(dirX * applySpeed, 0, dirZ * applySpeed);
            transform.Translate(moveDir * Time.deltaTime); 

        }

    }

    public void TryRun()
    {     
            if (OVRInput.Get(OVRInput.RawButton.B) || Input.GetKey(KeyCode.Space))
            {
                Running();
            }
            if (OVRInput.GetUp(OVRInput.RawButton.B) || !Input.GetKey(KeyCode.Space))
            {
                RunningCancle();
            }
        
    }

    public void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
        stamina.DecreaseProgress(); 
    }

    public void RunningCancle()
    {
        isRun = false;
        applySpeed = walkSpeed;
        stamina.IncreaseProgress();
    }

    public void Attack()
    {
        peekaboo.SetActive(true);
    }

    //public void Attack2()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Debug.Log("공격");
    //        peekaboo.SetActive(true);
    //    }

    //}
}
