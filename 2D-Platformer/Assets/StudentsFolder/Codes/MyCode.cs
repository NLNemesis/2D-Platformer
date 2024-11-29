using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MyCode : MonoBehaviour
{
    #region Variables
    /*
    Basic Variables
    int = (-2, -1, 0, 1, 2)
    float = (-3.2, -1. 0, 0.5, 1.4)
    bool = true/false
    string = "Text"

    Unity's Variables
    GameObject = ObjectName.SetActive(true/false);
    Transform = Position, Rotation, Scale

    private = A variable that can be accessed from this script
    public  = A variable that can be accessed from all the scripts
    */

    //Examples
    public int Number = 10;
    private float Health = 100;
    public bool Check = true;
    private string Text = "Code";
    public GameObject Chest;
    private Transform Player;
    #endregion

    #region Arrays
    /*
    An array is a collection of variables that are the same type
    */

    void TestArray()
    {
        //Instead of having this mess
        string Item1;
        string Item2;
        string Item3;

        //We convert it into an array
        string[] Item;

        //How to assign a private Array
        Item = new string[3];

        //How to add new elements into a private array or with a for loop
        Item[0] = "HP";
        Item[1] = "MP";
        Item[2] = "Apple";

        //We can assign the public arrays from the inspector
    }
    #endregion

    #region Single If
    /*
    Equal (==)
    Higher(>)
    Lower (<)
    Higher or equal (>=)
    Lower or equal  (<=)
    Not Equal (!=)
    */

    //Example
    void Example()
    {
        //Check if the number is positive or negative
        int Number = 7;

        if (Number > 0)
            Debug.Log("The number is positive");
        else if (Number < 0)
            Debug.Log("The number is negative");
        else
            Debug.Log("The number is 0");
        /////////////////////////////////////////////////////////////////////////////////
        //Check if an enemy can attack a player
        bool AttackPlayer = false;
        float Distance = 15;

        if (Distance < 20)
            AttackPlayer = true;
        else
            AttackPlayer = false;
    }
    #endregion

    #region Multiple If
    // (&& And)
    // (|| Or)

    void MultipleIf()
    {
        //Check the player's health and shows his condition
        float Health = 75;
        string Condition;
        bool Dead = false;

        if (Health > 100)
            Condition = "Very well";
        else if (Health > 50 && Health < 75)
            Condition = "Good";
        else if (Health > 25 && Health < 50)
            Condition = "Okay";
        else if (Health < 25)
            Condition = "Bad";

        if (Health <= 0 && Dead == false)
            Dead = true;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (Health < 0 || Dead == true)
            Condition = "Dead";
        else if (Health > 0 || Dead == false)
            Condition = "Alive";
    }
    #endregion

    #region For Loop
    /*
    int i  > How many times i repeat the code
    Length > How many times i need to repeat the code
    i++    > Increaser
    break; > Stops the for
    */

    public void ForLoopExample()
    {
        int Number = 0;
        int LoopTimes = 10;

        for (int i = 0; i < LoopTimes; i++)
        {
            Number += 5;
        }
        //////////////////////////////////////////////////////////////////

        float Health = 0;
        LoopTimes = 35;

        for (int i = 0; i < LoopTimes; i++)
        {
            Health += 2.5f;

            if (Health > 100)
                break;
        }
    }
    #endregion

    #region While Loop
    /*
    The while loop repeats the code until the condition becomes false
    */
    void WhileLoopExample()
    {
        float Health = 0f;
        float Regeneration = 0.25f;

        while (Health < 100) //As long as the condition is true, loop the code
        {
            Health += Regeneration;
        }
    }
    #endregion

    #region Switch
    void SwitchCase()
    {
        int PotionTier = 0;
        float Health = 0;
        
        switch (PotionTier)
        {
            case 1:
            {
                Health += 20;
                Debug.Log("Player heals 20 hp");
                break;
            }
            case 2:
            {
                Health += 30;
                Debug.Log("Player heals 30 hp");
                break;
            }
            default:
            {
                Health += 10;
                Debug.Log("Player heals 10 hp");
                break;
            }
        }
    }
    #endregion

    #region Collision
    /*
    Collisions are seperated into 3 parts
    When the player hits a hitbox (OnTriggerEnter)
    When the player stays inside to the hitbox (OnTriggerStay)
    When the player leaves the hitbox (OnTriggerExit)
    */

    //The Object is the reference to the entity that hits the hitbox
    //If we develop a 3D game we delete the 2D in voids and in Collider

    /*
    I can recognize the Object that collides with the hitbox by tags or names
    */
    void OnTriggerEnter2D(Collider2D Object)
    {
        if (Object.tag == "Tag Name")
            Debug.Log("The tag is" + Object.tag);
    }

    void OnTriggerStay2D(Collider2D Object)
    {
        if (Object.name == "Object Name")
            Debug.Log("The tag is" + Object.name);
    }

    void OnTriggerExit2D(Collider2D Object)
    {
        if (Object.name == "Object Name")
            Debug.Log("The tag is" + Object.name);
    }
    #endregion

    #region Mouse and Keyboard Inputs
    /*
    Keyboard Inputs

    Input.GetKeyDown = When the player press a key
    Input.GetKey     = When the player holds a key
    Input.GetKeyUp   = When the player release a key

    The KeyCode is the reference to the button that we want to Check
    KeyCode.Alpha0-9  -->The numbers above the letters of the keyboard
    KeyCode.Keypad0-9 -->The numpad numbers
    KeyCode.Tab,LeftShift,Escape,LeftCotrol -->The reference to the buttons

    Mouse Inputs

    Input.GetMouseButtonDown = When the player press a mouse button
    Input.GetMouseButton     = When the player holds a mouse button
    Input.GetMouseButtonUp   = When the player release a mouse button

    0 = Left Click
    1 = Right Click
    2 = Middle Click
    The side buttons are 4-5
    */

    void TestButtons()
    {
        Input.GetKeyDown(KeyCode.E);
        Input.GetKey(KeyCode.Alpha1);
        Input.GetKeyUp(KeyCode.Keypad0);

        Input.GetMouseButtonDown(0);
        Input.GetMouseButton(1);
        Input.GetMouseButtonUp(2);
    }
    #endregion

    #region References and Components
    /*
    GetComponent = Grab the reference from the specific object
    GetComponentInParent = Grab a reference from the object's parent
    GetComponentInChildren = Grab a reference from the object's children
    */

    private InventoryController IC;
    private PlayerMovement PM;
    private Animator _animator;
    private Canvas canvas;
    void GrabReference()
    {
        IC = GameObject.Find("/MaxPrefab/Player").GetComponent<InventoryController>();
        PM = GameObject.Find("/MaxPrefab/Player").GetComponent<PlayerMovement>();
        _animator = GameObject.Find("/MaxPrefab/Player").GetComponent<Animator>();
        canvas = GameObject.Find("/MaxPrefab/Canvas").GetComponent<Canvas>();
    }
    #endregion

    #region Animator
    Animator animator; //The name of the animator
    void AnimatorController()
    {
        //How to change the variables of an animator
        animator.SetInteger("Number", 10);
        animator.SetFloat("Number1", 2.6f);
        animator.SetBool("Open", true);
        animator.SetTrigger("Attack");   //Sets the trigger to true
        animator.ResetTrigger("Attack"); //Sets the trigger to false

        //How to get variables from the animator
        int Number = animator.GetInteger("Number");
        float State = animator.GetFloat("State");
        bool Open = animator.GetBool("Open");

        //How to change animator's speed value between 0-Infinite
        animator.speed = 1;

        //How to enable/disable the animator
        animator.enabled = false; //true = enables the animator, false = disable the animator
    }
    #endregion

    #region UI Elements

    #endregion

    #region Access to a different script

    #endregion

    #region Audio Sources
    public AudioSource AS;
    public AudioClip AC;
    
    void TestAudioSource()
    {
        //Change the clip
        AS.clip = AC;

        //Change the volume
        AS.volume = 0.5f;

        //Modes
        AS.Play();
        AS.Stop();
        AS.Pause();
        AS.UnPause();

        //Loop
        AS.loop = true;
    }
    #endregion

    #region Functions

    #endregion

    #region IENumerators

    #endregion

    #region Vectors && Colors

    #endregion

    #region Start and Update
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
