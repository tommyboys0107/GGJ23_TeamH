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

    float maxPH = 100f;
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

    Hsinpa.InsectMapBuilder insectMapBuilder;


    public float HPscale = 0.25f;
    private bool deadFlag = false;


    // Start is called before the first frame update
    void Start()
    {
        maxPH= hp;
        MainGM = this;
        Line = gameObject.GetComponent<LineRenderer>();
        insectMapBuilder = GameObject.FindObjectOfType<Hsinpa.InsectMapBuilder>(includeInactive: true);
        StartColor = Line.startColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (deadFlag) return;

        if (hp <= 0)
        {
            Hsinpa.Utility.SimpleEventSystem.Send(Hsinpa.GeneralStaticFlag.EventFlag.GameFailEvent, this);
            deadFlag = true;
            isAlive = false;
            return;
        }

        if (insectMapBuilder != null && !insectMapBuilder.InsectBodyCollider.OverlapPoint(P.transform.position)) {
            Hsinpa.Utility.SimpleEventSystem.Send(Hsinpa.GeneralStaticFlag.EventFlag.GameFailEvent, this);
            deadFlag = true;
            return;
        }

        hp -= Time.deltaTime;
        if (isAlive)
        {
            PlayerMove();

            LineUpdate();
            SetColor();
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
    public GameObject pp;
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
                //Instantiate(pp);
                g.GetComponent<LineBullet>().hp = hp * HPscale;
                hp = hp - hp * HPscale;

                AddLineEvent?.Invoke();
                BottonDownR = false;
            }

            if (BottonDownL == true)
            {
                GameObject g = Instantiate(LinePrefab, P.transform.position, Quaternion.Euler(0, 0, P.eulerAngles.z + 90));
                //Instantiate(pp);
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
    public Color LineOverColor;
    Color StartColor;
    Color nowColor;
    protected void SetColor()
    {
        Debug.Log("sc1");
        if (hp < maxPH * 0.7f)
        {

            Debug.Log("sc");

            nowColor = Color.Lerp(StartColor, LineOverColor, 1 - hp / maxPH);

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(nowColor, 0.0f), new GradientColorKey(nowColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
            );
            Line.colorGradient = gradient;
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
