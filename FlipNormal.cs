using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipNormal : MonoBehaviour
{
    public GameObject InvertObject;
    //选择一个要反转的物体

    void Awake()
    {
        InvertSphere();//瞎取一个函数名
    }

    void InvertSphere()
    {
        //==============================================
        Vector3[] normals = InvertObject.GetComponent<MeshFilter>().sharedMesh.normals;
        //这一段把模型的每一个法线反转了
        for(int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        InvertObject.GetComponent<MeshFilter>().sharedMesh.normals = normals;
        //更新反转后的法线
        //==============================================

        //仅仅翻转法线还不够，必须根据三角形遍历顺序把1-2-3改变成3-2-1，这样渲染管线应用阶段才会读翻转后的法线（在obj里表现为同时反转v和vn）；
        //三角形换序的同时别忘了submesh也要继承，不然submesh对应的子材质就会变成仅有一个(Cuz Every material has a separate tri list)
        Material[] materials = InvertObject.GetComponent<Renderer>().materials;
        int SubmeshNum =  materials.Length;//统计子材质数量
        for (int j = 0; j < SubmeshNum; j++)
        {
            int[] triangles = InvertObject.GetComponent<MeshFilter>().sharedMesh.GetTriangles(j);//按每个submesh遍历三角形
            for (int index = 0; index < triangles.Length; index += 3)//submesh的每个三角形绕序
            {
                int intermediate = triangles[index];
                triangles[index] = triangles[index + 2];
                triangles[index + 2] = intermediate;
            }
            InvertObject.GetComponent<MeshFilter>().sharedMesh.SetTriangles(triangles, j);//更新反转后的submesh三角形
        }
        //==============================================

    }
}
