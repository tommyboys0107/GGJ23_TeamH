using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBullet : MonoBehaviour
{
    public float hp;
    LineRenderer Line;
    public Transform P;//player
    public float RotaSpeed = 20;
    public float MoveSpeed = 20;
    public Vector3[] v3s = new Vector3[1];
    public float LineUptateTime = 0.5f;
    float t;
    public Vector2 RandomRota;
    public float RandomTime;

    public GameObject LinePrefab;
    float rt;

    Hsinpa.InsectMapBuilder insectMapBuilder;

    void Start()
    {
        Line = gameObject.GetComponent<LineRenderer>();
        v3s[0] = P.position;
        GM.AddLineEvent += AddNewLine;
        rt = Time.time + 1;

        insectMapBuilder = GameObject.FindObjectOfType<Hsinpa.InsectMapBuilder>();
        //P = transform;
    }

    private void Update()
    {
        hp -= Time.deltaTime;
        if(hp <= 0)
        {
            GM.AddLineEvent -= AddNewLine;
            Destroy(this);
            return;
        }

        //Chekc constraint
        Vector3 currentPosition = P.transform.position;

        if (insectMapBuilder != null) {

            //Debug.Log( "Width Right " + (insectMapBuilder.center.x + (insectMapBuilder.width * 0.5f)));
            //Debug.Log("Width Left " + (insectMapBuilder.center.x - (insectMapBuilder.width * 0.5f)));
            //Debug.Log("X " + currentPosition.x);

            if (
                currentPosition.x > insectMapBuilder.center.x + (insectMapBuilder.width * 0.5f) ||
                currentPosition.x < insectMapBuilder.center.x - (insectMapBuilder.width * 0.5f) ||

                currentPosition.y > insectMapBuilder.center.y + (insectMapBuilder.height * 0.5f) ||
                currentPosition.y < insectMapBuilder.center.y - (insectMapBuilder.height * 0.5f)
                ) {

                //Debug.Log("Destory");
                return;
            }
                
        }


        LineMove();
        LineUpdate();
    }
    protected void LineMove()
    {
        if (Time.time > rt)
        {
            rt = Time.time + RandomTime;
            RotaSpeed = Random.Range(RandomRota.x, RandomRota.y);
            
        }
        P.transform.Rotate(Vector3.forward * RotaSpeed * Time.deltaTime);
        P.transform.position += -P.transform.up * Time.deltaTime * MoveSpeed;
    }
    protected void LineUpdate()
    {
        if (Time.time > t)
        {
            t = Time.time + LineUptateTime;
            System.Array.Resize(ref v3s, v3s.Length + 1);
            v3s[v3s.Length - 1] = P.position;
            Line.positionCount = v3s.Length;
            Line.SetPositions(v3s);
        }

        Line.SetPosition(v3s.Length - 1, P.position);


    }
    int n= 1;
    public void AddNewLine()
    {
        GameObject g = Instantiate(GM.MainGM.LinePrefab, P.transform.position, P.transform.rotation);
        float r = Random.Range(30, 80f);
        g.transform.Rotate(Vector3.forward * r * Time.deltaTime);
        
        P.transform.Rotate(Vector3.forward * -r * Time.deltaTime);
        rt = Time.time + 1;
        g.GetComponent<LineBullet>().hp = hp * GM.MainGM.HPscale;
        hp = hp - hp * GM.MainGM.HPscale;
        
    }
}
