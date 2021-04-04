using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Node startNode;
    public Node finisgNode;
    public List<Node> graph;
    [Range(0,5)]public float delay;

    public bool IsDesiredNode(Node node)
    {
    	if (node.isFinishNode) return true;
    	return false;
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

    private bool HasNodeInList(Node node, List<Node> nodesList)
    {
    	foreach (var n in nodesList)
    	{
    		if (n == node) return true;
    	}
    	return false;
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
			StartCoroutine(BreadthFirstSearch());
		}
	}

	private void Start()
	{
		StartCoroutine(BreadthFirstSearch());
	}
}
