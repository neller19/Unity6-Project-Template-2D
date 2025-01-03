using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Utils
{

    public static Vector2 RandomPointOnRectanglePerimeter(Vector2 center, Vector2 size)
    {
        //taking a random number between 0 and half the perimeter,
        //if that number is less than a side length (y axis), we choose a 
        //side to spawn on, otherwise we spawn top/bottom. This should
        //correctly weight the edge chosen based on their lengths
        float maxRandom = size.y + size.x;
        float randomEdgeChoice = Random.Range(0, maxRandom);

        bool useSide = randomEdgeChoice <= size.y;

        //half the time we choose the left or top side, other
        //half we choose the bottom or right
        bool leftOrTop = Random.Range(0, 100) > 50;

        Vector2 halfSize = size * 0.5f;

        Vector2 v1 = center;
        Vector2 v2 = center;

        if (useSide)
        {
            //set the edge to be the y axis side on the left
            //or right of the rectangle
            float horizontal = halfSize.x * (leftOrTop ? -1 : 1);
            v1 += new Vector2(horizontal, -halfSize.y);
            v2 += new Vector2(horizontal, halfSize.y);
        }
        else
        {
            //set the edge to be the x axis side on the top
            //or bottom of the rectangle
            float vertical = halfSize.y * (leftOrTop ? 1 : -1);
            v1 += new Vector2(-halfSize.x, vertical);
            v2 += new Vector2(halfSize.x, vertical);
        }
        //finally grab a random point along the chosen edge
        float interpolator = Random.Range(0, 1f);
        return Vector2.Lerp(v1, v2, interpolator);
    }



}

public static class ShipMath
{
    //returns the distance a vehicle (ship) will cover while coming to a stop from the given speed
    public static float CalcStoppingDistance(float speed, float acceleration)
    {
        return Mathf.Pow(speed, 2) / (2 * acceleration);
    }

    //returns how much deceleration is needed to come to a complete stop while covering the given distance
    public static float CalcStoppingDeceleration(float startSpeed, float distance)
    {
        return (startSpeed * startSpeed) / (2 * distance);
    }

    /// <summary>
    /// Gets the turning circle radius for an object moving with a set speed and turn rate. 
    /// for any turn speed, the amount of time it takes for 1 revolution is 2*pi / turnSpeed (or 360 degrees / turn speed)
    /// in this time, the total distance covered is time * movespeed. This is the circumference of the circle we will have travelled.
    /// From this, the radius can be derived from the formula [circumference = 2 * PI * radius] or [radius = circ / 2pi]
    /// since circumference = time * movespeed, and time = 2pi / turnspeed, we can simplify to:
    /// radius = (time * movespeed) /2pi
    ///         = ((2pi / turnspeed) * movespeed) / 2pi
    ///         = movespeed / turnspeed         - 2 pi cancels
    /// So turn radius is just the movespeed / turn speed
    /// </summary>
    /// <param name="moveSpeed"></param>
    /// <param name="turnSpeedRadians"></param>
    /// <returns></returns>
    public static float GetTurningRadius(float moveSpeed, float turnSpeedRadians)
    {
        return moveSpeed / turnSpeedRadians;
    }

    public static float GetTurningRadiusDegrees(float moveSpeed, float turnSpeedDegrees)
    {
        return GetTurningRadius(moveSpeed, turnSpeedDegrees * Mathf.Deg2Rad);
    }
}