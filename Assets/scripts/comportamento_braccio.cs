using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comportamento_braccio : MonoBehaviour
{
    float cooldownposizionamento = 0;
    public GameObject cestinoProva;
    void Update()
    {
        RaycastHit puntocolpito;
        Vector3 direzioneavanti = transform.TransformDirection(Vector3.forward) * 20;
        if (cooldownposizionamento != 0)
        {
            cooldownposizionamento = cooldownposizionamento - Time.deltaTime;
            if (cooldownposizionamento < 0)
                cooldownposizionamento = 0;
        }
        if (Physics.Raycast(transform.position, direzioneavanti, out puntocolpito, 20f)&& cooldownposizionamento == 0)
        {
            Debug.DrawRay(transform.position, direzioneavanti, Color.green);
            if (Input.GetMouseButton(0))
            {
                cooldownposizionamento = 3;
                Instantiate(cestinoProva,puntocolpito.point,Quaternion.identity);
                /*costruzione.AddComponent<MeshFilter>();
                costruzione.AddComponent<MeshRenderer>();
                costruzione.GetComponent<MeshFilter>().mesh=*/
                //Instantiate(costruzione, punto);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, direzioneavanti, Color.red);
        }
        
    }
}
