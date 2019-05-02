/**
*　CreateName　PureRabbitHeart
*　CreateTime　2019/05/02
*　Processing　敵を時間でランダムな場所に生成する処理
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR 
using UnityEditor;
using UnityEditor.Experimental.UIElements;//Editor拡張で使うためのusing
#endif

public class CreateEnemyManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> EnemyList = new List<GameObject>();//出現する敵の情報を保存
    [SerializeField]
    private float fCreateTime;                                  //スポーンする時間
    private float fCurrentTime;                                 //n秒おきに生成するときに経過時間を保存する変数

    /**
    *   初期化 
    */
    void Awake()
    {
        fCurrentTime = 0.0f;
    }

    /**
    * 更新処理
    */
    void Update()
    {
        TimeCreateEnemy(fCreateTime);
    }

    /**
    *   n秒おきに敵を生成する処理
    *   引数…スポーンする時間
    **/
    private void TimeCreateEnemy(float time)
    {
        fCurrentTime += Time.deltaTime;
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2), transform.position.y, transform.position.z);
        if (EnemyList.Count > 0 && fCurrentTime > time)
        {
            GameObject enemy = Instantiate(EnemyList[Random.Range(0, EnemyList.Count)], pos, Quaternion.identity);
            Destroy(enemy, 20.0f);
            fCurrentTime = 0.0f;
        }
    }

    /**
    *   呼び出されたら敵を生成する処理
    *   引数…生成する敵の数
    **/
    public void CallCreateEnemy(int enemyValue)
    {
        for (int i = 0; i < enemyValue; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2), transform.position.y, transform.position.z);
            GameObject enemy = Instantiate(EnemyList[Random.Range(0, EnemyList.Count)], pos, Quaternion.identity);
            Destroy(enemy, 20.0f);
        }
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(CreateEnemyManager))]
    public class GunEditor : Editor
    {
        bool isEnemyList = true;
        public override void OnInspectorGUI()
        {
            CreateEnemyManager p_CreateEnemyManager = target as CreateEnemyManager;

            if (isEnemyList = EditorGUILayout.Foldout(isEnemyList, "敵データ"))
            {
                for (int i = 0; i < p_CreateEnemyManager.EnemyList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    p_CreateEnemyManager.EnemyList[i] = EditorGUILayout.ObjectField(p_CreateEnemyManager.EnemyList[i], typeof(GameObject), true) as GameObject;
                    if (GUILayout.Button("削除", GUILayout.MinWidth(100.0f), GUILayout.MinHeight(10.0f)))
                    {
                        p_CreateEnemyManager.EnemyList.Remove(p_CreateEnemyManager.EnemyList[i]);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                GameObject AddEnemy = EditorGUILayout.ObjectField("追加", null, typeof(GameObject), true) as GameObject;
                if (AddEnemy != null)
                {
                    p_CreateEnemyManager.EnemyList.Add(AddEnemy);
                }
            }

            EditorGUILayout.LabelField("n秒ごとに生成する時間");
            p_CreateEnemyManager.fCreateTime = EditorGUILayout.Slider(p_CreateEnemyManager.fCreateTime, 0.0f, 60.0f);

        }
    }

#endif
}
