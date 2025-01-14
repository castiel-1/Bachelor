using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Tree_script : MonoBehaviour
{
    [SerializeField]
    int maxDepth = 2;

    [SerializeField]
    int minNumChildren = 0;

    [SerializeField]
    int maxNumChildren = 2;

    void Start()
    {
        /*
        List<object> result = generateRandomTree(0);
        Debug.Log(FormatTree(result, 0));
        */
    }
   
    public List<object> GenerateRandomTree(int currentDepth)
    {

        List<object> tree = new List<object> {"null"};

        // randomizes like so: minNumchildren <= x < maxnNumChildren, so +1 needed
        int numChildren = Random.Range(minNumChildren, maxNumChildren + 1);

        if (currentDepth > maxDepth)
        {
            return tree;
        }

        for (int i = 0; i < numChildren; i++)
        {
            List<object> childTree = GenerateRandomTree(currentDepth + 1);
            tree.Add(childTree); 
        }

        return tree;
    }

    public string FormatTree(List<object> tree, int depth)
    {

        string result = "";
        string indent = new string(' ', depth * 4);  

        for (int i = 0; i < tree.Count; i++)
        {
            if (tree[i] is string node)
            {
                result += node; 
            }
            else if (tree[i] is List<object> childTree)
            {
                result += $"\n{indent}|-- ";  

                result += FormatTree(childTree, depth + 1);  
            }

        }

        return result;
    }
}
