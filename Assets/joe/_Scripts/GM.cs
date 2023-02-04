using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public static GM MainGM;
    public float hp = 100;
    LineRenderer Line;
    public Vector3[] v3s;
    public Transform P;//player
    public float RotaSpeed = 20;
    public float MoveSpeed = 20;
    public float LineUptateTime = 0.5f;
    float t;


    public GameObject LinePrefab;
    public GameObject deadPrefab;

    public List<Transform> points = new List<Transform>();
    public float PointUptateTime = 2f;
    float pt;

    public Material met;
    public bool isAlive = true;

    public GameObject OverUI;

    public bool BottonDownR;
    public bool BottonDownL;
    public delegate void AddLine();
    public static event AddLine AddLineEvent;

    public float HPscale = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        MainGM = this;
        Line = gameObject.GetComponent<LineRenderer>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Hsinpa.Utility.SimpleEventSystem.Send(Hsinpa.GeneralStaticFlag.EventFlag.GameFailEvent, this);

            return;
        }
        hp -= Time.deltaTime;
        if (isAlive)
        {
            PlayerMove();

            LineUpdate();

            //AddObject();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Scene1");
            }
        }
    }
 
    protected void PlayerMove()
    {
        
       
        if (Input.GetKey(KeyCode.RightArrow))
        {
           
            P.transform.Rotate(Vector3.forward * RotaSpeed * Time.deltaTime);
            BottonDownR = true;
        }
        else
        if (Input.GetKey(KeyCode.LeftArrow))
        {
           
            P.transform.Rotate(Vector3.forward * -RotaSpeed * Time.deltaTime);
            BottonDownL = true;
        }
        else
        {
            if (BottonDownR == true)
            {
                GameObject g = Instantiate(LinePrefab, P.transform.position, Quaternion.Euler(0, 0, P.eulerAngles.z + -90));
                g.GetComponent<LineBullet>().hp = hp * HPscale;
                hp = hp - hp * HPscale;

                AddLineEvent?.Invoke();
                BottonDownR = false;
            }

            if (BottonDownL == true)
            {
                GameObject g = Instantiate(LinePrefab, P.transform.position, Quaternion.Euler(0, 0, P.eulerAngles.z + 90));
                g.GetComponent<LineBullet>().hp = hp * HPscale;
                hp = hp - hp * HPscale;

                AddLineEvent?.Invoke();
                BottonDownL = false;
            }
        }
        P.transform.position += -P.transform.up * Time.deltaTime * MoveSpeed;
        Camera.main.transform.position = new Vector3(P.position.x, P.position.y, Camera.main.transform.position.z);
    }

    protected void LineUpdate()
    {
        if (Time.time > t)
        {
            t = Time.time + LineUptateTime;
            Array.Resize(ref v3s, v3s.Length + 1);
            v3s[v3s.Length - 1] = P.position;
            Line.positionCount = v3s.Length;
            Line.SetPositions(v3s);
        }
        
        Line.SetPosition(v3s.Length - 1, P.position);

        
    }

    public void AddObject()
    {
       
        if (Time.time > pt)
        {
            pt = Time.time + PointUptateTime+UnityEngine.Random.Range(-1f,1f);
            int r = UnityEngine.Random.Range(6, 10);
            for (int i = 0; i < r; i++)
            {
                Instantiate(deadPrefab, (P.position + P.up * -15) + P.right * UnityEngine.Random.Range(-15f, 15f), P.rotation);
            }

            r = UnityEngine.Random.Range(1, 100);
            if (r > 30)
            {
                r = 1;
                for (int i = 0; i < r; i++)
                {
                    //Instantiate(pointPrefab, (P.position + P.up * -20) + P.right * UnityEngine.Random.Range(-3f, 3f), P.rotation);
                }
            }
           

        }

    }

    public void EnterPoint(Transform _point)
    {
        points.Add(_point);
    }

    public void EnterDead()
    {
        Debug.Log("d");
        PlayerDead();
        
    }

    public void AddNewLine(Vector3 pos,Vector3 rot)
    {

    }

    public void AddHp(float value)
    {
        hp += value;
    }

    protected void PlayerDead()
    {
        if(points.Count == 0)
        {
            isAlive = false;
            OverUI.SetActive(true);
            return;
        }

        GameObject g = new GameObject("g");
        Line = g.AddComponent<LineRenderer>();
        Line.material = met;
        v3s = new Vector3[1];
        P.position = points[points.Count - 1].position;
        v3s[0] = P.position;
        points.RemoveAt(points.Count - 1);

    }

   
}
