using System;
using System.Collections;
using UnityEngine;

public class LinkedList
{
    public class Node
    {
        public GameObject data; // this is where snake's head and tails will be
        public Node next;       // to create next node

        // CTor, when adding GameObject, set nextnode null
        public Node(GameObject obj)
        {
            data = obj;
            next = null;
        }

        // if next is null, add gameobject into next (and the next one will be null. check out the constructor above)
        public void AddToEnd(GameObject data)
        {
            if (next == null)
            {
                next = new Node(data);
            }
            // if next is not null, call this method again and check the next next, and next next next... so on
            else
            {
                next.AddToEnd(data);
            }
        }
    }

    public Node headNode;   // first one(snake head) will be headNode

    // this property returns the amount of the nodes
    public int Count
    {
        get
        {
            int count = 0;
            Node currentNode = headNode;    // currentNode is just for checking the num of nodes
            while (currentNode != null)     // loops through untill the end of the node
            {
                // move to next node and increase count by 1
                currentNode = currentNode.next; 
                count++;
            }
            return count;
        }
    }

    // when you want to go through like arraylist (t.e. list[0], list[1])
    public GameObject this[int index]
    {
        get
        {
            Node currentNode = headNode;
            for (int i = 0; i <= index && currentNode != null; i++)
            {
                if (index == i)
                {
                    return currentNode.data;
                }
                currentNode = currentNode.next; // thanks to this, we move to the next node when the i is increased
            }
            return null;
        }
    }

    // Ctor
    public LinkedList()
    {
        headNode = null;
    }

    // if headNode is null, add obj to the headNode. otherwise call AddToEnd() method in Node class. go up and check
    public void AddToEnd(GameObject obj)
    {
        if (headNode == null)
        {
            headNode = new Node(obj);
        }
        else
        {
            headNode.AddToEnd(obj);
        }
    }

}
