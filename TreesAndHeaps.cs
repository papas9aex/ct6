using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Генерация списка случайных чисел
        List<int> randomNumbers = GenerateRandomNumbers(30, 0, 100);

        // Создание и визуализация бинарного дерева поиска
        BST binarySearchTree = new BST();
        foreach (int number in randomNumbers)
        {
            binarySearchTree.Insert(number);
        }

        Console.WriteLine("Binary Search Tree:");
        binarySearchTree.Visualize();
        Console.Write("In-order: ");
        binarySearchTree.InOrderTraversal();
        Console.Write("Pre-order: ");
        binarySearchTree.PreOrderTraversal();

        // Создание и визуализация AVL-дерева
        AVLTree avlTree = new AVLTree();
        foreach (int number in randomNumbers)
        {
            avlTree.Insert(number);
        }

        Console.WriteLine("AVL Tree:");
        avlTree.Visualize();
        Console.Write("In-order Traversal: ");
        avlTree.InOrderTraversal();

        // Создание и визуализация бинарной кучи
        BinaryHeap binaryHeap = new BinaryHeap();
        foreach (int number in randomNumbers)
        {
            binaryHeap.Insert(number);
        }

        Console.WriteLine("Binary Heap:");
        binaryHeap.Print();
    }

    static List<int> GenerateRandomNumbers(int count, int min, int max)
    {
        Random random = new Random();
        List<int> rndNums = new List<int>();

        while (rndNums.Count < count)
        {
            int randomNumber = random.Next(min, max + 1);
            if (!rndNums.Contains(randomNumber))
            {
                rndNums.Add(randomNumber);
            }
        }

        return rndNums;
    }
}

class Node
{
    public int Key;
    public Node Left;
    public Node Right;

    public Node(int key)
    {
        Key = key;
        Left = null;
        Right = null;
    }
}

class BST
{
    public Node root;

    public void Insert(int key)
    {
        root = InsertHelper(root, key);
    }

    public Node InsertHelper(Node currentNode, int key)
    {
        if (currentNode == null)
        {
            return new Node(key);
        }

        if (key < currentNode.Key)
        {
            currentNode.Left = InsertHelper(currentNode.Left, key);
        }
        else if (key > currentNode.Key)
        {
            currentNode.Right = InsertHelper(currentNode.Right, key);
        }

        return currentNode;
    }

    public void Visualize()
    {
        VisualizeHelper(root, " ", true);
    }

    public void VisualizeHelper(Node node, string prefix, bool isLeft)
    {
        if (node == null)
        {
            return;
        }

        string nodeStr = node.Key.ToString();
        string line = prefix + (isLeft ? "├── " : "└── ");
        Console.WriteLine(line + nodeStr);

        string childPrefix = prefix + (isLeft ? "│   " : "    ");
        VisualizeHelper(node.Left, childPrefix, true);
        VisualizeHelper(node.Right, childPrefix, false);
    }

    public void InOrderTraversal()
    {
        InOrderTraversalHelper(root);
        Console.WriteLine();
    }

    public void InOrderTraversalHelper(Node node)
    {
        if (node != null)
        {
            InOrderTraversalHelper(node.Left);
            Console.Write(node.Key + " ");
            InOrderTraversalHelper(node.Right);
        }
    }

    public void PreOrderTraversal()
    {
        PreOrderTraversalHelper(root);
        Console.WriteLine();
    }

    public void PreOrderTraversalHelper(Node node)
    {
        if (node != null)
        {
            Console.Write(node.Key + " ");
            PreOrderTraversalHelper(node.Left);
            PreOrderTraversalHelper(node.Right);
        }
    }
}

class AVLNode
{
    public int key;
    public AVLNode left;
    public AVLNode right;
    public int height;

    public AVLNode(int key)
    {
        this.key = key;
        this.left = null;
        this.right = null;
        this.height = 1;
    }
}

class AVLTree
{
    AVLNode root;

    public AVLTree()
    {
        this.root = null;
    }

    public void Insert(int key)
    {
        root = InsertHelper(root, key);
    }

    private AVLNode InsertHelper(AVLNode root, int key)
    {
        if (root == null)
        {
            return new AVLNode(key);
        }

        if (key < root.key)
        {
            root.left = InsertHelper(root.left, key);
        }
        else if (key > root.key)
        {
            root.right = InsertHelper(root.right, key);
        }

        root.height = 1 + Math.Max(GetHeight(root.left), GetHeight(root.right));

        int balanceFactor = GetBalance(root);

        if (balanceFactor > 1)
        {
            if (key < root.left.key)
            {
                return RightRotate(root);
            }
            else
            {
                root.left = LeftRotate(root.left);
                return RightRotate(root);
            }
        }

        if (balanceFactor < -1)
        {
            if (key > root.right.key)
            {
                return LeftRotate(root);
            }
            else
            {
                root.right = RightRotate(root.right);
                return LeftRotate(root);
            }
        }

        return root;
    }

    public AVLNode Delete(AVLNode root, int key)
    {
        if (root == null)
        {
            return root;
        }
        else if (key < root.key)
        {
            root.left = Delete(root.left, key);
        }
        else if (key > root.key)
        {
            root.right = Delete(root.right, key);
        }
        else
        {
            if (root.left == null)
            {
                AVLNode temp = root.right;
                root = null;
                return temp;
            }
            else if (root.right == null)
            {
                AVLNode temp = root.left;
                root = null;
                return temp;
            }

            AVLNode successor = GetSuccessor(root.right);
            root.key = successor.key;
            root.right = Delete(root.right, successor.key);
        }

        if (root == null)
        {
            return root;
        }

        root.height = 1 + Math.Max(GetHeight(root.left), GetHeight(root.right));

        int balanceFactor = GetBalance(root);

        if (balanceFactor > 1)
        {
            if (GetBalance(root.left) >= 0)
            {
                return RightRotate(root);
            }
            else
            {
                root.left = LeftRotate(root.left);
                return RightRotate(root);
            }
        }

        if (balanceFactor < -1)
        {
            if (GetBalance(root.right) <= 0)
            {
                return LeftRotate(root);
            }
            else
            {
                root.right = RightRotate(root.right);
                return LeftRotate(root);
            }
        }

        return root;
    }

    private AVLNode GetSuccessor(AVLNode node)
    {
        while (node.left != null)
        {
            node = node.left;
        }
        return node;
    }

    private int GetHeight(AVLNode node)
    {
        if (node == null)
        {
            return 0;
        }
        return node.height;
    }

    private int GetBalance(AVLNode node)
    {
        if (node == null)
        {
            return 0;
        }
        return GetHeight(node.left) - GetHeight(node.right);
    }

    private AVLNode RightRotate(AVLNode y)
    {
        AVLNode x = y.left;
        AVLNode T2 = x.right;

        x.right = y;
        y.left = T2;

        y.height = 1 + Math.Max(GetHeight(y.left), GetHeight(y.right));
        x.height = 1 + Math.Max(GetHeight(x.left), GetHeight(x.right));

        return x;
    }

    private AVLNode LeftRotate(AVLNode x)
    {
        AVLNode y = x.right;
        AVLNode T2 = y.left;

        y.left = x;
        x.right = T2;

        x.height = 1 + Math.Max(GetHeight(x.left), GetHeight(x.right));
        y.height = 1 + Math.Max(GetHeight(y.left), GetHeight(y.right));

        return y;
    }
    public void Visualize()
    {
        VisualizeHelper(root, "", true);
    }

    private void VisualizeHelper(AVLNode root, string indent, bool last)
    {
        if (root != null)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("└── ");
                indent += "    ";
            }
            else
            {
                Console.Write("├── ");
                indent += "│   ";
            }
            Console.WriteLine(root.key);
            VisualizeHelper(root.left, indent, false);
            VisualizeHelper(root.right, indent, true);
        }
    }

    public void InOrderTraversal()
    {
        InOrderTraversalHelper(root);
        Console.WriteLine();
    }

    private void InOrderTraversalHelper(AVLNode node)
    {
        if (node != null)
        {
            InOrderTraversalHelper(node.left);
            Console.Write($"{node.key} ");
            InOrderTraversalHelper(node.right);
        }
    }
}

public class BinaryHeap
{
    public int[] heap;
    public int size;
    public BinaryHeap()
    {
        heap = new int[31];
        size = 0;
    }
    public void Insert(int item)
    {
        if (size == heap.Length)
        {
            ResizeHeap();
        }

        heap[size] = item;
        size++;
        ShiftUp(size - 1);
    }
    public int Delete()
    {
        if (size == 0)
        {

            throw new Exception("Heap is empty");
        }


        int root = heap[0];
        heap[0] = heap[size - 1];
        size--;
        ShiftDown(0);
        return root;
    }
    private void ShiftUp(int index)
    {
        int parentIndex = (index - 1) / 2;
        while (parentIndex >= 0 && heap[parentIndex] > heap[index])
        {
            Swap(parentIndex, index);
            index = parentIndex;
            parentIndex = (index - 1) / 2;
        }
    }

    private void ShiftDown(int index)
    {
        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int largest = index;

            if (leftChildIndex < size && heap[leftChildIndex] < heap[largest])
            {
                largest = leftChildIndex;
            }

            if (rightChildIndex < size && heap[rightChildIndex] < heap[largest])
            {
                largest = rightChildIndex;
            }

            if (largest != index)
            {
                Swap(largest, index);
                index = largest;
            }
            else
            {
                break;
            }
        }
    }
    private void Swap(int i, int j)
    {
        int temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
    private void ResizeHeap()
    {
        int[] newHeap = new int[heap.Length * 2];
        Array.Copy(heap, newHeap, heap.Length);
        heap = newHeap;
    }
    public void Print()
    {
        if (size == 0)
        {
            Console.WriteLine("PriorityQueue is empty.");
            return;
        }

        PrintNode(0, "");
    }
    public int getElement(int index)
    {
        return heap[index];
    }

    private void PrintNode(int index, string indent)
    {
        if (index < size)
        {
            Console.WriteLine(indent + heap[index]);

            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;

            if (leftChildIndex < size)
            {
                Console.WriteLine(indent + "├── " + "Left Child:");
                PrintNode(leftChildIndex, indent + "│   ");
            }

            if (rightChildIndex < size)
            {
                Console.WriteLine(indent + "└── " + "Right Child:");
                PrintNode(rightChildIndex, indent + "    ");
            }
        }

    }
    public int binarySearch(int target)
    {
        int left = 0;
        int right = size - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;

            if (heap[mid] == target)
            {
                return mid; // Элемент найден
            }
            else if (heap[mid] < target)
            {
                left = mid + 1; // Искомый элемент может находиться в правой половине
            }
            else
            {
                right = mid - 1; // Искомый элемент может находиться в левой половине
            }
        }

        return -1;
        // Элемент не найден
    }
}
