using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class GunBasic : MonoBehaviour
{
    public float fFireRate;     // 弾の間隔
    public float fSpread;       // 拡散度
    public float fRange;        //射程 未実装
    public float fDamage;       //ダメージ
    public float fBulletSpeed;  // 弾の速度
    public float fReloadTime;   // リロードにかかる時間
    public int clipSize;        // 最大装填数
    public int ammoMax;         // 弾の最大所持数
    public int ammoUsep;        // 一発あたりの消費弾数
    public int shotPerRound;    // 発射弾数
    [SerializeField]
    public int ammo;            // 現在の装填数
    [SerializeField]
    public int ammoHave;        // 現在の弾の所持数
    public bool isShell;        // 薬きょうを出すか
    public bool isHitScan;      // ヒットスキャンにするか
    public bool isShotSE;       // 弾の発射音のフラグ
    public GameObject Bullet;   // 発射するオブジェクト
    public GameObject Shell;    // 薬莢
    public GameObject MuzzleFX; // 発射エフェクト                     
    public Transform tShellOuter;// 薬莢の排出口
    [SerializeField]
    public Transform tMuzzle;   // 発射位置を指定
    public Transform tStartRay; // Rayの開始位置
    public AudioClip ShotSE;    //弾を打ったときの音
    public AudioClip ShotEndSE; //打ち終わったときの音
    public enum ShotType        // 銃の種類
    {
        Full,
        Semi,
    }
    public ShotType eShotType;  // 銃の種類をいれる変数
    private float fTimeS;       // Shotのタイム
    private float fTimeR;       // Reloadのタイム
    private bool isShot;        // 弾を打っているかのフラグ
    private bool isReload;      // リロードをするかのフラグ
    private Quaternion initRotation;// 角度初期化する際にでも
    private int layer;          // layerの距離
    private bool isShellPut = false;// 薬きょうを出すときに建てるフラグ
    private ParticleSystem Psys;// パーティクルシステムの情報を入れる変数
    private bool isSE;          // 弾の発射音のフラグ
    private bool isBulletEnd;   //球切れ


    /**
    *  初期化
    */
    public void Init()
    {
        ammo = clipSize;//現在の装填数に最大装填数を入れる
        ammoHave = ammoMax;//残りの玉に最大玉数を入れる
    }

    /**
    *  どのタイプの銃にするか
    */
    public void ChoiceType(string input)// どのタイプの銃にするか
    {
        if (!isReload)
        {
            switch (eShotType)
            {
                case ShotType.Full:
                    Shot(Input.GetButton(input));
                    break;
                case ShotType.Semi:
                    Shot(Input.GetButtonDown(input));
                    break;

            }
        }
    }

    /**
    *  弾を打つ処理
    */
    public void Shot(bool input)
    {
        if (isShot == true)
        {
            fTimeS += Time.deltaTime;
            if (fTimeS > fFireRate)
            {
                isShot = false;
                fTimeS = 0;
            }
        }
        if (input == true && isShot == false)
        {
            ShotElement();
        }
    }


    /**
    *  弾の打つ間隔
    */
    public void Recast()
    {
        if (isShot == true)
        {
            fTimeS += Time.deltaTime;
            if (fTimeS > fFireRate)
            {
                isShot = false;
                fTimeS = 0;
            }
        }
    }

    /**
    *  リロード
    */
    public void Reload()
    {
      
        if (!(ammo >= clipSize))//最大装填数より少なかったら
        {
            isReload = true;
        }
        
        if (isReload == true)//リロードができるなら
        {
            fTimeR += Time.deltaTime;
            if (fTimeR >= fReloadTime)
            {
                if (ammoHave - (clipSize - ammo) <= 0)
                {
                    ammo += (ammoHave);
                    ammoHave = 0;
                }
                else
                {
                    ammoHave -= (clipSize - ammo);
                    ammo += (clipSize - ammo);
                }
                isReload = false;
                fTimeR = 0f;
            }
        }
    }

    /**
    *  レイキャストでうつ弾の処理
    */
    private void ShotElement()
    {
        isShot = true;
        if (ammo > 0)//装填している玉があれば
        {
            for (int i = 0; i < shotPerRound; i++)
            {
                Vector3 diffusionVector;
                float angle_x = Random.Range(-fSpread, fSpread);
                float angle_y = Random.Range(-fSpread, fSpread);
                diffusionVector = new Vector3(angle_x, angle_y, 0);

                if (isHitScan == true)
                {
                    Ray ray = new Ray(tStartRay.position, tStartRay.forward + diffusionVector);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, fRange, layer))
                    {
                        if (hit.transform.tag == "Enemy")
                        {
                            //ここにダメージの処理を書く
                        }
                    }
                }
                else
                {
                    GameObject b = Instantiate(Bullet, tMuzzle.position, tMuzzle.rotation);
                    b.GetComponent<Rigidbody>().velocity = b.transform.forward * fBulletSpeed;
                    Destroy(b, 10.0f);
                    //b.SendMessage("BulletPower", fDamage);//ダメージ処理
                }

                //生成と同時に弾に移動速度を与える
                if (MuzzleFX != null)
                {
                    var fx = Instantiate(MuzzleFX, tMuzzle.position, tMuzzle.rotation) as GameObject;
                    fx.transform.parent = tMuzzle;
                    Destroy(fx, 1.0f);

                }

                isShellPut = true;
                if (isShellPut == true && isShell == true)
                {
                    Instantiate(Shell, tShellOuter.position, tShellOuter.rotation);
                    isShellPut = false;

                }

                isSE = true;
                //Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.5f, true);
                //transform.rotation = initRotation;
            }
            ammo -= ammoUsep;
        }
        else//装填している玉がないとき
        {
            isBulletEnd = true;
        }

        if (isShotSE == true)//サウンドを出すなら
        {
            Sound();
        }
    }

    /**
    *  残りの残弾
    */
    public int GetAmmo()
    {
        return ammo;
    }

    /**
    *  残り持っている残弾数
    */
    public int GetAmmoHave()
    {
        return ammoHave;
    }

    /**
    *  発射時の音処理
    */
    void Sound()
    {
        if (isSE == true)
        {
            GetComponent<AudioSource>().PlayOneShot(ShotSE);
            isSE = false;
        }
        else if (isBulletEnd == true)
        {
            GetComponent<AudioSource>().PlayOneShot(ShotEndSE);
            isBulletEnd = false;

        }

    }

}