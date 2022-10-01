using UnityEngine;

public class BumblebeeController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;

    [Header("Vertical force")] 
    [SerializeField] private bool useForceCurve;
    [SerializeField] private float forceFactor;
    [SerializeField] private AnimationCurve forceFactorCurve;
    [SerializeField] private float maxVerticalVelocity;


    void Update()
    {
        var verticalValue = Input.GetAxis("Vertical");

        var verticalForce = 0f;

        if (useForceCurve)
        {
            verticalForce = forceFactorCurve.Evaluate(verticalValue);
        }
        else
        {
            verticalForce = verticalValue * forceFactor;
        }

        var force = Vector2.up * verticalForce;
        rigidbody.AddForce(force, ForceMode2D.Impulse);
        
        rigidbody.velocity = Vector2.up * Mathf.Clamp(rigidbody.velocity.y, -maxVerticalVelocity, maxVerticalVelocity);
    }
}