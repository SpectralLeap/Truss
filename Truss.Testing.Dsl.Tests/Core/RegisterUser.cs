using Truss.Testing.Dsl.Drivers;

namespace Truss.Testing.Dsl.Tests.Core;

public class RegisterUser;

public class RegisterUserDriver(RegistrationStore registrationStore) : Driver<RegisterUser>
{
    public override async Task Drive(DslArgs args)
    {
        registrationStore.AddData(args["email"]!);
        await Task.Delay(10);
    }
}