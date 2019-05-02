/**
*　CreateName　PuerRabbitHeart
*　CreateTime　2019/04/29
*　Processing　共同の攻撃処理
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR 
using UnityEditor;
using UnityEditor.Experimental.UIElements;//Editor拡張で使うためのusing
#endif


public class BulletManager : GunBasic
{
    [SerializeField]
    private bool isEnemy;// 敵に使うか自分に使うか
    public bool isEnemyShot;// 敵に使うか自分に使うか


    /**
    *   start前の初期化 
    */
    void Awake()
    {
        Init();
    }

    /**
    *   更新処理 
    */
    void Update()
    {
       Recast();//発射間隔
       Shot(Input.GetKeyDown("joystick button 0"));//Aボタンで発射
       Shot(Input.GetKeyDown(KeyCode.Space));//Spaceで発射
       Reload();
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(BulletManager))]
    public class GunEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GunBasic p_GunBasic = target as GunBasic;
         
            p_GunBasic.fFireRate = EditorGUILayout.FloatField("弾打つ間隔", p_GunBasic.fFireRate);
            EditorGUILayout.LabelField("拡散度");
            p_GunBasic.fSpread = EditorGUILayout.Slider(p_GunBasic.fSpread, 0.0f, 1.0f);
            EditorGUILayout.LabelField("射程");
            p_GunBasic.fRange = EditorGUILayout.Slider(p_GunBasic.fRange, 0.0f, 100.0f);
            EditorGUILayout.LabelField("ダメージ");
            p_GunBasic.fDamage = EditorGUILayout.Slider(p_GunBasic.fDamage, 0.0f, 100.0f);
            p_GunBasic.fBulletSpeed = EditorGUILayout.FloatField("弾の速度", p_GunBasic.fBulletSpeed);
            p_GunBasic.eShotType = (ShotType)EditorGUILayout.EnumPopup("銃の種類", p_GunBasic.eShotType);
            p_GunBasic.fReloadTime = EditorGUILayout.FloatField("リロードにかかる時間", p_GunBasic.fReloadTime);
            p_GunBasic.clipSize = EditorGUILayout.IntField("最大装填数", p_GunBasic.clipSize);
            p_GunBasic.ammoMax = EditorGUILayout.IntField("弾の最大所持数", p_GunBasic.ammoMax);
            p_GunBasic.ammoUsep = EditorGUILayout.IntField("一発あたりの消費弾数", p_GunBasic.ammoUsep);
            p_GunBasic.shotPerRound = EditorGUILayout.IntField("発射弾数", p_GunBasic.shotPerRound);
            p_GunBasic.Bullet = EditorGUILayout.ObjectField("発射する弾", p_GunBasic.Bullet, typeof(GameObject), true) as GameObject;
            p_GunBasic.tMuzzle = EditorGUILayout.ObjectField("発射位置を指定", p_GunBasic.tMuzzle, typeof(Transform), true) as Transform;

            p_GunBasic.isHitScan = EditorGUILayout.Toggle("ヒットスキャンにするか", p_GunBasic.isHitScan);
            if (p_GunBasic.isHitScan == true)
            {
                p_GunBasic.tStartRay = EditorGUILayout.ObjectField("Rayの開始位置", p_GunBasic.tStartRay, typeof(Transform), true) as Transform;
            }

            p_GunBasic.isShell = EditorGUILayout.Toggle("薬莢を出すか", p_GunBasic.isShell);
            if (p_GunBasic.isShell == true)
            {
                p_GunBasic.Shell = EditorGUILayout.ObjectField("薬莢", p_GunBasic.Shell, typeof(GameObject), true) as GameObject;
                p_GunBasic.tShellOuter = EditorGUILayout.ObjectField("薬莢の排出口", p_GunBasic.tShellOuter, typeof(Transform), true) as Transform;

            }

            p_GunBasic.MuzzleFX = EditorGUILayout.ObjectField("発射エフェクト", p_GunBasic.MuzzleFX, typeof(GameObject), true) as GameObject;
            p_GunBasic.isShotSE = EditorGUILayout.Toggle("弾の発射音を鳴らすか", p_GunBasic.isShotSE);
            if (p_GunBasic.isShotSE == true)
            {
                p_GunBasic.ShotSE = EditorGUILayout.ObjectField("発射時の音", p_GunBasic.ShotSE, typeof(AudioClip), true) as AudioClip;
                p_GunBasic.ShotEndSE = EditorGUILayout.ObjectField("弾切れの音", p_GunBasic.ShotEndSE, typeof(AudioClip), true) as AudioClip;

            }
        }
    }

#endif
}