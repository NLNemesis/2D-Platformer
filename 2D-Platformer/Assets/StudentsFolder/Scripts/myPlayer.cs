using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class myPlayer : MonoBehaviour
{
    #region Variables
    [Header("Controller")]
public bool Freezed;
[HideInInspector] public bool InLadder;
[HideInInspector] public bool IsClimbing;

[Header("Movement")]
public float speed = 8f;
[HideInInspector] public float originalSpeed;
public float jumpingPower = 16f;
private float horizontal;
private bool isFacingRight = true;
public float vertical;
public float ClimbingSpeed;

[Header("Stats")]
public float Health;
public float Damage;

[Header("References")]
public Rigidbody2D rb;
public Transform groundCheck;
[SerializeField] private LayerMask groundLayer;

[Header("Unity Event")]
public UnityEvent IsGroundedEvent;
public UnityEvent NotIsGroundedEvent;
public UnityEvent DeathEvent;
#endregion

void Awake()
{
    originalSpeed = speed;

}

}