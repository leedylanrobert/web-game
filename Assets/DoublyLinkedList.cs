namespace Utils 
{
    public class DLL {
        public Node head; // head of list
    
        /* Doubly Linked list Node*/
        public class Node {
            public int data;
            public Node prev;
            public Node next;
    
            // Constructor to create a new node
            // next and prev is by default initialized as null
            public Node(int d) { data = d; }
        }
    }
}
