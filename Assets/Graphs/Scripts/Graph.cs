using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Node startNode;
    public Node finishNode;
    public List<Node> graph;
    [SerializeField] private GameObject nodePrefab;
    [Range(0,5)]public float delay;
    public Node[,] matrix = new Node[10,10];
    private List<Node> checkedNodes = new List<Node>();
    private List<Node> parents = new List<Node>();

    public bool IsDesiredNode(Node node)
    {
    	if (node.isFinishNode) return true;
    	return false;
    }

    public void GenerateGraph()
    {
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                var position = new Vector3(i, 0, j);
                var node = Instantiate(nodePrefab, position, Quaternion.identity);
                var n = node.GetComponent<Node>();
                if (i == 0 && j == 0)
                {
                    n.isStartNode = true;
                    startNode = n;
                }
                else if (i == 9 && j == 9) {
                    n.isFinishNode = true;
                    finishNode = n;
                }
                node.transform.SetParent(transform);
                matrix[i, j] = n;
            }
        }
        CreateLinks();
    }

    private void CreateLinks()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                try 
                {
                   matrix[i,j].neighbors.Add(matrix[i+1,j]); 
                } 
                catch (IndexOutOfRangeException) 
                {
                    Debug.Log("Fail");
                }
                try 
                {
                   matrix[i,j].neighbors.Add(matrix[i,j+1]); 
                } 
                catch (IndexOutOfRangeException) 
                {
                    Debug.Log("Fail");
                }
                try 
                {
                   matrix[i,j].neighbors.Add(matrix[i-1,j]);
                } 
                catch (IndexOutOfRangeException) 
                {
                    Debug.Log("Fail");
                }
                try 
                {
                   matrix[i,j].neighbors.Add(matrix[i,j-1]); 
                } 
                catch (IndexOutOfRangeException) 
                {
                    Debug.Log("Fail");
                }
                
                
            }
        }
        //StartCoroutine(BreadthFirstSearch());
    }

    public IEnumerator BreadthFirstSearch()
    {
    	//создаём очередь с соседями стартовой точки
    	var searchQueue = new Queue<Node>(startNode.neighbors);
    	//создаём список для уже проверенных узлов
    	var checkedNodes = new List<Node>();
    	//пока очередь не ппуста извлека
    	while (searchQueue.Count > 0)
    	{
    		var node = searchQueue.Dequeue();
    		if (!HasNodeInList(node, checkedNodes))
    		{
    			if (IsDesiredNode(node))
    			{
    				Debug.Log("finish node is" + node.name);
    				
    			}
    			else
    			{
    				Debug.Log("ааааааааааааааааааа");
    				foreach (var n in node.neighbors)
    				{
    					searchQueue.Enqueue(n);
    				}
    				checkedNodes.Add(node);
    				node.isChecked = true;
    			}
    		}
    		yield return new WaitForSeconds(delay);
    	}
    }
/*
    public IEnumerator Dijkstras()
    {
        Node node = startNode;


        while(!node.isFinishNode)
        {
            Debug.Log(node.name);
            float cost = node.label;
            
            List<Node> neighbors = node.neighbors;
            
            for (var i = 0; i < neighbors.Count; i++) 
            {
                //Debug.Log(cost);
                //Debug.Log(node.distanceToNeighbors[i]);
                var newCost = cost + node.distanceToNeighbors[i];
                // новая стоимость для соседних точек
                print(neighbors[i].label > newCost);//newCost+""+neighbors[i].label);
                if (neighbors[i].label > newCost)
                // если новая стоимость меньше старой для соседних точек
                {
                    neighbors[i].isChecked = true;
                    neighbors[i].unknownLabel = false;
                    neighbors[i].label = newCost;
                    // ярлык - новая стоимость
                    if (node != null) neighbors[i].parent = node;
                    // обновление родителя с наименьшей стоимостью
                }
            }
            checkedNodes.Add(node);
            node = FindLowestCostNode();
            //Debug.Log(node.name);
            yield return new WaitForSeconds(4);
        }
    }

    private Node FindLowestCostNode()
    {
        float lowestCost = float.PositiveInfinity;
        Node lowestCostNode = null;
        for (var i = 0; i < graph.Count; i++) 
        {
            if (graph[i].label < lowestCost && !HasNodeInList(graph[i], checkedNodes))
            {
                Debug.Log(graph[i].name + "checked");
                lowestCost = graph[i].label;
                lowestCostNode = graph[i];
            }
        }
        return lowestCostNode;
    }

    private bool HasNodeInList(Node node, List<Node> nodesList)
    {
    	foreach (var n in nodesList)
    	{
    		if (n == node) return true;
    	}
    	return false;
    }
*/
    private bool HasNodeInList(Node node, List<Node> nodesList)
    {
        foreach (var n in nodesList)
        {
            if (n == node) return true;
        }
        return false;
    }
    
    public IEnumerator Dijkstras()
    {
        Node node = startNode;

        while(!node.isFinishNode)
        {
            Debug.Log(node.name);
            float cost = node.label;
            List<Node> neighbors = node.neighbors;
            for (var i = 0; i < neighbors.Count; i++) 
            {
                Debug.Log(cost);
                Debug.Log(node.distanceToNeighbors[i]);
                var newCost = cost + node.distanceToNeighbors[i];
                neighbors[i].isChecked = true; 
                if (neighbors[i].label > newCost)
                {
                    neighbors[i].unknownLabel = false; 
                    neighbors[i].label = newCost; 
                    if (node != null) neighbors[i].parent = node;
                }
            } 
            checkedNodes.Add(node); 
            node = FindLowestCostNode();
            Debug.Log(node.name);
            yield return new WaitForSeconds(4);
        }
    }

    private Node FindLowestCostNode()
    {
        float lowestCost = float.PositiveInfinity;
        Node lowestCostNode = null;
        for (var i = 0; i < graph.Count; i++)
        {
            if (graph[i].label < lowestCost && !HasNodeInList(graph[i], checkedNodes))
            {
                Debug.Log(graph[i].name + " checked");
                lowestCost = graph[i].label;
                lowestCostNode = graph[i];
            }
        }
        return lowestCostNode;
    }



    private void SetGraph()
    {
    	foreach (Transform t in transform)
		{
			graph.Add(t.GetComponent<Node>());
		}
    }

	private void OnDrawGizmos()
	{
		if (graph.Count == 0)
		{
			SetGraph();
			//StartCoroutine(BreadthFirstSearch());
		}
	}

	private void Start()
	{
        StartCoroutine(Dijkstras());
        //GenerateGraph();
		//StartCoroutine(BreadthFirstSearch());
	}
}
