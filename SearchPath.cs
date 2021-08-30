using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPath : MonoBehaviour
{
    [Header("Program y Asignar")] //Un texto ahí
    [SerializeField] private Node startingPoint; //Poner el nodo desde el cual parte el player
    [SerializeField] private Node endingPoint; //Pues el punto donde llega
    [SerializeField] private Color startingPointColor; //Color de partida
    [SerializeField] private Color endingPointColor; //Color de llegada
    [SerializeField] private Color pathColor; //Color del camino
    [SerializeField] private Color exploredNodeColor; //y asi

    private Dictionary<Vector2Int, Node> block = new Dictionary<Vector2Int, Node>(); //El diccionario para todos los nodos
    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };  //Direcciones para buscar en el BFS,   // Directions to search in BFS
    private Queue<Node> queue = new Queue<Node>(); //Cola de nodos 
    private Node searchingPoint; //Nodo Actual de análisis
    private bool isExploring = true; //El estado de si se está analizando, este se detiene más adelante cuando se encuentra el nodo final

    private List<Node> path = new List<Node>(); //Guarda el Path definitivo

    public List<Node> Path //El Path, será una lista de Nodos
    {
        get
        {
            if (path.Count == 0)                           // If we've already found path, no need to check it again
            {
                LoadAllBlocks();  //Es un metodo que buscará todos los obejtos tipo nodo, está más abajo
                BFS(); //Es el metodo que buscará el path entre los nodos
                CreatePath(); //Algo así como el tejido del Pah
            }
            return path; //Lo que devuelve
        }
    }
    
    // For getting all nodes with Node.cs and storing them in the dictionary
    private void LoadAllBlocks()
    {
        Node[] nodes = FindObjectsOfType<Node>(); //Organiza los objetos de tipo nodo en el [] nodes

        foreach (Node node in nodes)
        {
            Vector2Int gridPos = node.GetPos(); //acceso al componente de la posición que tiene el script de nodos

            // For checking if 2 nodes are in same position; i.e overlapping nodes
            if (block.ContainsKey(gridPos))
            {
                Debug.LogWarning("2 Nodos presentes en la misma posición. i.e nodos Sobrepuestos.");
            }
            else
            {
                block.Add(gridPos, node);        // Add the position of each node as key and the Node as the value
            }
        }
    }
    
    
    // BFS; For finding the shortest path
    private void BFS()
    {
        queue.Enqueue(startingPoint); //Pirmero guarda el primer elemento, que es el nodo de Partida
        while (queue.Count > 0 && isExploring)  //El BFS Opera mientras la lista no sea nula, osea siempre que esté asignado el primernodo
            //y que El explorador este funcionando, cosa que se detiene con el motodo On reaching end
        {
            searchingPoint = queue.Dequeue();
            OnReachingEnd(); //Método que frena la Busqueda más abajo una vez se coinciden los nodos buscados y el final.
            ExploreNeighbourNodes(); //Método que explora los ndoos Veecinos (Como lo dice el nombre ._.)
        }
    }

    // To check if we've reached the Ending point
    private void OnReachingEnd()
    {
        if (searchingPoint == endingPoint) //Si coinciden el buscador con el buscado
        {
            isExploring = false; //Se apaga la busqueda
        }
        else
        {
            isExploring = true;
        }
    }

    // Searching the neighbouring nodes
    private void ExploreNeighbourNodes()
    {
        if (!isExploring) { return; } //Si no está explorando, pues no tiene que funcionar

        foreach (Vector2Int direction in directions) //Por cada direccion en el vector de direcciones
        {
            Vector2Int neighbourPos = searchingPoint.GetPos() + direction; //Que se tomen la posición, más las nuevas direcciones

            if (block.ContainsKey(neighbourPos))               // If the explore neighbour is present in the dictionary _block, which contians all the blocks with Node.cs attached
            {                                                   //Traduccion, si el el vecino está en el diccionario de nodos que son los nodos con el Script (todos)...
                Node node = block[neighbourPos];                //Agregar al diccionario ese nodo

                if (!node.isExplored)
                {
                    queue.Enqueue(node);                       // Y Enqueueing the node at this position
                    node.isExplored = true;                     //Aquí solo estamos marcandolos como explorados o no
                    node.GetComponentInChildren<Renderer>().material.color = exploredNodeColor;
                    node.isExploredFrom = searchingPoint;      // Set how we reached the neighbouring node i.e the previous node; for getting the path
                }
            }
        }
    }
    //Create Path es el metodo más heavy probablemente
    // Creating path using the isExploredFrom var of each node to get the previous node from where we got to this node
    public void CreatePath()
    {
        SetPath(endingPoint);  //Set Path es un void mas abajo que añade el nodo a la lista, en este caso el final
        Node previousNode = endingPoint.isExploredFrom; //Estamos decalrando los nodos que iran antes del ultimo

        while (previousNode != startingPoint) //Entonces mientras el nodo anterior al ultimo no sea el primero...
        {
            SetPath(previousNode); //El set path pasará a ser este previo nodo, el cual se archivará
            previousNode = previousNode.isExploredFrom; //Y se seguirá explorando de nuevo, desde este
        }
        SetPath(startingPoint); // Tamvien hay qye archivar el punto de partida en el path
        path.Reverse(); //Todo este Método de busqueda opera de modo que se hace la busqueda y evaluacion de nodos partiendo de
        //El ultimo, el de llegada hacia atras, el reverse, hacer que se de el proceso inverso
        SetPathColor(); // El metodo que lo colorea
    }
    
    // For adding nodes to the path
    private void SetPath(Node node)
    {
        path.Add(node); //recordar que path es la lista de nodos
    }

    // Setting color to nodes
    private void SetPathColor()
    {
        foreach (Node node in path) //Esto ocurre luego de que hay un path hallado
        {
            node.GetComponentInChildren<Renderer>().material.color = pathColor; //Los colores se asignan en Unity
        }
        SetColor(); //Duh
    }
    // Setting color to start and end position
    private void SetColor()
    {
        startingPoint.GetComponentInChildren<Renderer>().material.color = startingPointColor; //Instanciado arriba
        endingPoint.GetComponentInChildren<Renderer>().material.color = endingPointColor; //x2
    }
}
