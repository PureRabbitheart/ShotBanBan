/**
*　CreateName　agenasu
*　CreateTime　2019/5/1
*　Processing　コントローラーでの移動処理
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMove : MonoBehaviour
{
    const string H_MOVE = "L_Stick_H";
    const string V_MOVE = "L_Stick_V";

    
    [SerializeField]
    private Vector3 vMoveSpeed; //動く速さ

    [SerializeField]
    private GameObject Hero;


    // Update is called once per frame
    void Update()
    {     
         move(vMoveSpeed);
    }


    void move(Vector3 vMvSpd) //左スティックでの移動
    {
        float flsh = Input.GetAxis(H_MOVE);
        float flsv = Input.GetAxis(V_MOVE);
        if ((flsh != 0) || (flsv != 0))
        {
            Hero.transform.Translate(vMvSpd.x * flsh, vMvSpd.y * flsv, 0);
        }
    }


}
