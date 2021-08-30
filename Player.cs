using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float movementSmoothing; //Suavizado de movimiento
    [SerializeField] private float yOffset; 

    private SearchPath searchPath;

    private void Start()
    {
        searchPath = FindObjectOfType<SearchPath>();
        if (searchPath.Path != null)        // Si el jugador ya encontró un camino
        {
            StartCoroutine(Movement(searchPath.Path)); //le diremos que arranque la corutina de abajo, que es la que lo mueve
        }
    }

    IEnumerator Movement(List<Node> paths)
    {
        foreach (Node path in paths) //Para cada nodo
        {
            Vector3 pos = path.transform.position; //Le sacamos la posicion y transformamos la del jugador (La esfera a este)
            transform.position = new Vector3(pos.x, yOffset, pos.z); //En x y z pues porque y no va, no es 3d
            yield return new WaitForSeconds(movementSmoothing); //Que se espere el yOffset que se serializó
        }
    }
}

