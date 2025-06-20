using UnityEngine;

public static class CalculateOpposite
{
    public static Vector2 CalculateOpposite2D(Vector3 _position)
    {
        return CalculateOpposite2D(_position, out float _dis);
    }


    public static Vector2 CalculateOpposite2D(Vector3 _position, out float _distance)
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        mouseWorldPosition.z = 0;

        Vector3 vectorToMouse = mouseWorldPosition - _position;

        Vector3 oppositeDirection = -vectorToMouse;

        if (vectorToMouse.magnitude > 0.01f)
        {
            oppositeDirection.Normalize();
        }
        else
        {
            oppositeDirection = Vector3.zero;
        }

        _distance = vectorToMouse.magnitude;
        return oppositeDirection;
        // this.oppositeDirection = oppositeDirection;

        // float distance = UnityEngine.Mathf.Clamp(vectorToMouse.magnitude / distanceToMaxForec, 0, distanceToMaxForec);

        // this.force = UnityEngine.Mathf.Clamp(maxForce * distance, 0, maxForce);

        // TARGET.position = transform.position + oppositeDirection;
    }

}
