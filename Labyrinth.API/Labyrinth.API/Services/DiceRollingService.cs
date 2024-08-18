using FluentResults;
using System;

namespace Labyrinth.API.Services;

public class DiceRollingService
{
    private readonly Random _random;

    public DiceRollingService()
    {
        _random = new Random();
    }

    public Result<int> RollDice(int numberOfSides, int numberOfDice)
    {
        // Validate input
        if (numberOfSides <= 0)
        {
            return Result.Fail<int>("Number of sides must be greater than zero.");
        }

        if (numberOfDice <= 0)
        {
            return Result.Fail<int>("Number of dice must be greater than zero.");
        }

        int total = 0;

        // Roll each die and sum the results
        for (int i = 0; i < numberOfDice; i++)
        {
            int roll = _random.Next(1, numberOfSides + 1);
            total += roll;
        }

        return Result.Ok(total);
    }
}
