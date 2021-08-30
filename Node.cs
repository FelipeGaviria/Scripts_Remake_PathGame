using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isExplored = false; //Estado de si el nodo ya fue explorado (si o no)
    public Node isExploredFrom; //Varible desde la cual se explora 

    public Vector2Int GetPos() //Vector de como se toma la siguiente posición llamado en el Program
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x / 11), //Divide las posiciones entre 11
                              Mathf.RoundToInt(transform.position.z / 11));
    }
}
