using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

[RequireComponent(typeof(MeshFilter))]
public class mesh_generator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertici;
    int[] triangoli;
    Vector2[] uvs;
    public int Temp = 15;
    public GameObject crea_triangoli(GameObject obj,Vector2 pos,Color col,int levelOfDetail,float seed, float amp, float fre, float ampmod, float fremod,int Variazione, int lunghezza, bool blocky, AnimationCurve heightCurve,float maxMeshHeight)
    {
        int simplificationIncrement = (levelOfDetail + 1) * 2;
        mesh = new Mesh();
        float y=0;
        Vector2[] offset = new Vector2[Variazione];
        uvs = new Vector2[(lunghezza + 1) * (lunghezza + 1)];
        for (int i = 0; i < Variazione; i++)
        {
            float offsetx = pos.x;
            float offsetz = pos.y;
            offset[i] = new Vector2(offsetx, offsetz);
        }

        float[,] mappa = new float[(lunghezza + 1) , (lunghezza + 1)];
        vertici = new Vector3[(lunghezza + 1) * (lunghezza + 1)]; 
        for (int z = 0; z <= lunghezza; z++)
        {
            for (int x = 0; x <= lunghezza; x++)
            {
                float value = genera_rumore(x, z, offset, seed, amp, fre, ampmod, fremod, lunghezza, Variazione, heightCurve, maxMeshHeight);
                if(blocky)
                    value = Mathf.RoundToInt(value);
                mappa[x, z] = value;

            }
        }
        for (int z = 0,i = 0; z <= lunghezza; z++)
        {
            for (int x = 0; x <= lunghezza; x++)
            {
                y = mappa[x,z];
                vertici[i]= new Vector3(x,y, z);
                uvs[i] = new Vector2((float)x / lunghezza, (float)z/lunghezza);
                i++;
            }
        }

        triangoli = new int[(lunghezza) * (lunghezza) * 6];
        int vertice_attuale = 0;
        int triangolo_attuale = 0;
        for (int z = 0; z < lunghezza; z++)
        {
            for (int x = 0; x < lunghezza; x++)
            {
                triangoli[triangolo_attuale + 0] = vertice_attuale;
                triangoli[triangolo_attuale + 1] = vertice_attuale + lunghezza + 1;
                triangoli[triangolo_attuale + 2] = vertice_attuale + 1;
                triangoli[triangolo_attuale + 3] = vertice_attuale + 1;
                triangoli[triangolo_attuale + 4] = vertice_attuale + lunghezza + 1;
                triangoli[triangolo_attuale + 5] = vertice_attuale + lunghezza + 2;

                vertice_attuale++;
                triangolo_attuale += 6;
            }
            vertice_attuale++;
        }
        Mesh m = aggiorna_mesh();
        obj.GetComponent<MeshFilter>().mesh = m;
        return obj;
    }
    public float genera_rumore(float x, float z,Vector2[] offset,float seed,float amplitude, float frequency, float amplitudeModifier, float frequencyModifier,int lunghezza,int Variazione,AnimationCurve heightCurve,float maxMeshHeight)
    {
        float noisesum = 0;
        float zCord;
        float xCord;
        for (int o = 0; o < Variazione; o++)
        {
            xCord = (x + offset[o].x) / lunghezza;
            zCord = (z + offset[o].y) / lunghezza;
            noisesum += (Mathf.PerlinNoise((xCord + seed) * frequency, (zCord - seed) * frequency) * 2 - 1) * amplitude;
            amplitude *= amplitudeModifier;
            frequency *= frequencyModifier;
        }
        noisesum = heightCurve.Evaluate(noisesum);
        return (noisesum*maxMeshHeight);
    }
    Mesh aggiorna_mesh()
    {
        mesh.Clear();
        mesh.vertices = vertici;
        mesh.triangles = triangoli;
        mesh.RecalculateNormals();
        mesh.uv = uvs;
        return mesh;
    }
}
