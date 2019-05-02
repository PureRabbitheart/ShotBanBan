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
    private GameObject Hero; //Player

    [SerializeField]
    private float fwidth; //横移動の制限   

    [SerializeField]
    private float fvertical; //縦移動の制限


    // Update is called once per frame
    void Update()

    {
         clamp();
         move(vMoveSpeed);
    }


    void clamp()　//移動制限
    {
        // Hero.transform.position = (new Vector3(Mathf.Clamp(Hero.transform.position.x, fMax_x, fMin_x), transform.position.y, Mathf.Clamp(Hero.transform.position.z, fMax_z, fMin_z)));
        Hero.transform.position = (new Vector3(Mathf.Clamp(Hero.transform.position.x, -fwidth, fwidth), transform.position.y, Mathf.Clamp(Hero.transform.position.z, -fvertical, fvertical)));
    }


    void move(Vector3 vMvSpd) //左スティックでの移動
    {
        float flsh = Input.GetAxis(H_MOVE);
        float flsv = Input.GetAxis(V_MOVE);
        if ((flsh != 0) || (flsv != 0))
        {
            Hero.transform.Translate(vMvSpd.x * flsh,0, vMvSpd.y * flsv);
        }
    }




}
