Drop in code for your login & logout commands

```
        public Command LoginCommand => field ??= new(async () =>
        {
            AuthLoginResult loginResult = await _authClient.LoginAsync();
            if (!loginResult.IsError)
            {
                Username = loginResult?.User?.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Failed to get username";
                UserPicture = loginResult?.User?.Claims.FirstOrDefault(c => c.Type == "picture")?.Value ?? "Failed to get profile picture";

                (IsLoginViewVisible, IsHomeViewVisible) = (false, true);
            }
            else
            {
                await _shellClient.DisplayAlertAsync("Error", loginResult?.ErrorDescription, "OK");
            }
        });
        public Command LogoutCommand => field ??= new(async () =>
        {
            await _authClient.LogoutAsync();
            (IsLoginViewVisible, IsHomeViewVisible) = (true, false);
        });
```
