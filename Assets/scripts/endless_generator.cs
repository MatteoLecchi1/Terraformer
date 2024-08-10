using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endless_generator : MonoBehaviour
{
    [SerializeField]
    public float distanza_vista= 600;
    public float seed;

    [SerializeField]
    private Transform giocatore;
    [SerializeField]
    private Transform platformTransform;
    [SerializeField]
    private GameObject water;

    public Material materiale;
    public static Vector2 posizione_giocatore;
    int chunksize;
    int chunk_visibili;
    public Color col;
    Dictionary<Vector2, terrainchunk> terrainchunkdictionary = new Dictionary<Vector2, terrainchunk>();
    List<terrainchunk> terreinchunksvisiblelastupdate = new List<terrainchunk>();

    [SerializeField]
    public int lunghezza = 241;

    [SerializeField]
    private float amplitude = 30f;
    [SerializeField]
    private float frequency = 1f;
    [SerializeField]
    private float amplitudeModifier = .5f;
    [SerializeField]
    private float frequencyModifier = 1.5f;
    [SerializeField]
    private float maxMeshHeight = 1f;
    [SerializeField]
    private bool Blocky = false;
    [SerializeField]
    private Vector2 startingPosition;

    [SerializeField]
    private int Variazione = 4;
    [SerializeField]
    private AnimationCurve heightCurve;
    private void Start()
    {
        chunksize = lunghezza;
        chunk_visibili =Mathf.RoundToInt( distanza_vista / chunksize);
        seed = UnityEngine.Random.Range(-10000.00f, 10000.00f);
        giocatore.position = new Vector3(startingPosition.x, Mathf.Max(FindHightAtPoint(startingPosition.x, startingPosition.y), water.transform.position.y) + 3, startingPosition.y);
        platformTransform.position = new Vector3(startingPosition.x,Mathf.Max(FindHightAtPoint(startingPosition.x, startingPosition.y), water.transform.position.y) + 1, startingPosition.y);
    }
    private void Update()
    {
        posizione_giocatore = new Vector2(giocatore.position.x, giocatore.position.z);
        aggiornachunkvisibili();
    }
    void aggiornachunkvisibili()
    {
        for (int i = 0; i < terreinchunksvisiblelastupdate.Count; i++)
        {
            terreinchunksvisiblelastupdate[i].setvisible(false);
        }
        terreinchunksvisiblelastupdate.Clear();
        int currentchunkx = Mathf.RoundToInt(posizione_giocatore.x / chunksize);
        int currentchunky = Mathf.RoundToInt(posizione_giocatore.y / chunksize);
        for (int yoffset = -chunk_visibili;yoffset<= chunk_visibili; yoffset++)
        {
            for (int xoffset = -chunk_visibili; xoffset <= chunk_visibili; xoffset++)
            {
                Vector2 viewdchunkcoord = new Vector2(currentchunkx + xoffset, currentchunky + yoffset);
                if (terrainchunkdictionary.ContainsKey(viewdchunkcoord))
                {
                    terrainchunkdictionary[viewdchunkcoord].updatechunk(distanza_vista);

                    if (terrainchunkdictionary[viewdchunkcoord].isvisble())
                    {
                        terreinchunksvisiblelastupdate.Add(terrainchunkdictionary[viewdchunkcoord]);
                    }
                }
                else
                {
                    terrainchunkdictionary.Add(viewdchunkcoord,new terrainchunk(viewdchunkcoord,chunksize,transform,materiale,col,seed,amplitude,frequency,amplitudeModifier,frequencyModifier,Variazione,lunghezza,Blocky,heightCurve,maxMeshHeight));
                }
            }
        }
    }
    float FindHightAtPoint(float x, float y)
    {
        float noisesum = 0;
        float zCord;
        float xCord;
        float amp = amplitude;
        float fre = frequency;
        for (int o = 0; o < Variazione; o++)
        {
            xCord = x / lunghezza;
            zCord = y / lunghezza;
            noisesum += (Mathf.PerlinNoise((xCord + seed) * fre, (zCord - seed) * fre) * 2 - 1) * amp;
            amp *= amplitudeModifier;
            fre *= frequencyModifier;
        }
        noisesum = heightCurve.Evaluate(noisesum);
        return (noisesum * maxMeshHeight);
    }
    public class terrainchunk
    {
        GameObject meshobj;
        Vector2 position;
        Bounds bounds;
        MeshCollider collider;
        [Range(0, 5)]
        public int levelOfDetail;
        public terrainchunk(Vector2 coord,int size,Transform parent, Material material,Color col,float seed, float amp, float fre, float ampmod,float fremod,int Variazione, int lunghezza,bool Blocky,AnimationCurve heightCurve,float maxMeshHeight)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 position3d = new Vector3(position.x, 0, position.y);
            mesh_generator mesh_gen = new mesh_generator();
            GameObject m = new GameObject();
            m.AddComponent(typeof(MeshRenderer));
            m.AddComponent(typeof(MeshFilter));
            m.GetComponent<MeshRenderer>().material = material;
            m.transform.position = position3d;
            m.transform.localScale =new Vector3(1,1,1);
            m.transform.parent = parent;
            meshobj = mesh_gen.crea_triangoli(m,position,col, levelOfDetail,seed, amp,fre,ampmod,fremod,Variazione,lunghezza,Blocky,heightCurve, maxMeshHeight);
            collider = meshobj.AddComponent<MeshCollider>();
            collider.sharedMesh= meshobj.GetComponent<MeshFilter>().mesh;
            setvisible(false);

        }
        public void updatechunk(float distanza_vista)
        {
            float viewerdistfromnearestedge=Mathf.Sqrt(bounds.SqrDistance(posizione_giocatore));
            bool visible = viewerdistfromnearestedge <= distanza_vista;
            setvisible(visible);
        }
        public void setvisible(bool visible)
        {
            meshobj.SetActive(visible);
        }
        public bool isvisble()
        {
            return meshobj.activeSelf;
        }
    }
}
