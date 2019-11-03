using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public ParticleSystem boomSystem;
    private ParticleSystem.Particle[] particlesArray;
    float r;
    int merge_count;
    Vector3[] targets;
    Vector3[] target_centers;
    float[] angles;
    float spendTime;
    // Start is called before the first frame update
    void Start()
    {
        var Explotion = GameObject.Find("MyExposion");
        boomSystem = Explotion.GetComponent<ParticleSystem>();
        boomSystem.maxParticles = 0;
        spendTime = 0f;
        r = 0f;
        merge_count = 0;
        int particleNum = 9000;
        particleSystem.maxParticles = particleNum;
        targets = new Vector3[particleNum];
        target_centers = new Vector3[6];
        particlesArray = new ParticleSystem.Particle[particleNum];
        angles = new float[particleNum];
        for (int i = 0; i < particleNum; i++)
        {
            targets[i] = new Vector3(0f, 0f, 0f);
            float ranx = Random.Range(0f, 5f);
            float rany = Random.Range(0f, 5f);
            angles[i] = Random.Range(0, 2 * Mathf.PI);
        }
    }
    

    void Update() //动画播放顺序
    {
        spendTime += Time.deltaTime;
        if (spendTime <= 2) CircleExpand();
        else if (spendTime <= 11) CircleRotate();
        else if (spendTime <= 20) Merge();
        else if (spendTime <= 30) DrawMode();
        else if (spendTime <= 34) CircleClose();
        else if (spendTime <= 35.5) boom();
        else boomSystem.maxParticles = 0;
    }

    void CircleExpand() //圆环扩张
    {
        particleSystem.GetParticles(particlesArray);
        for (int i = 0; i < particlesArray.Length; i++)
        {
            float v_x = Mathf.Cos(angles[i]);
            float v_y = Mathf.Sin(angles[i]);
            particlesArray[i].position += new Vector3(v_x, v_y, 0f) * 0.1f;
        }
        r += 0.1f;
        Debug.Log(r);
        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }

    void CircleRotate() //圆环旋转
    {
        Vector3 axis = new Vector3(0, 0, 1f);
        this.transform.RotateAround(this.transform.position, axis, 10 * Time.deltaTime);
    }

    void Merge() //圆环凝聚成六个点
    {
        for(int i = 0; i < 6; i++)
        {
            float angle = Mathf.PI / 3 * (float)i + Mathf.PI / 6;
            target_centers[i] = new Vector3(r * Mathf.Cos(angle), r * Mathf.Sin(angle), 0);
        }
        particleSystem.GetParticles(particlesArray);
        for (int i = merge_count; i < particlesArray.Length && i < merge_count+50; i++)
        {
            int pos = Random.Range(0, 6);
            float ran_x = Random.Range(-0.1f, 0.1f);
            float ran_y = Random.Range(-0.1f, 0.1f);
            particlesArray[i].position = target_centers[pos] + new Vector3(ran_x,ran_y, 0);
            targets[i] = particlesArray[i].position;
        }
        merge_count += 50;
        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }

    void DrawMode() //粒子绘制阵型
    {
        particleSystem.GetParticles(particlesArray);
        for (int i = 0; i < particlesArray.Length; i++)
        {
            if(targets[i] == particlesArray[i].position)
            {
                int pos = Random.Range(0, 6);
                float ran_x = Random.Range(-0.1f, 0.1f);
                float ran_y = Random.Range(-0.1f, 0.1f);
                targets[i] = target_centers[pos] + new Vector3(ran_x, ran_y, 0);
            }
            else
            {
                float MoveSpeed = Random.Range(0f, 20f);
                particlesArray[i].position = Vector3.MoveTowards(particlesArray[i].position, targets[i], MoveSpeed * Time.deltaTime);
            }
        }
        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }

    void CircleClose() //粒子收缩回圆心
    {
        particleSystem.GetParticles(particlesArray);
        for (int i = 0; i < particlesArray.Length; i++)
        {
            float v_x = Mathf.Cos(angles[i]);
            float v_y = Mathf.Sin(angles[i]);
            particlesArray[i].position *= 0.97f;
        }
        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }

    void boom() //播放爆炸动画
    {
        boomSystem.maxParticles = 4;
    }
}
