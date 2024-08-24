﻿using FirebaseAdmin.Auth;

namespace Labyrinth.API.Utilities;

public class TokenGenerator
{
    public async Task<string> GenerateCustomTokenAsync(string userId)
    {
        var token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(userId);
        return token;
    }
}