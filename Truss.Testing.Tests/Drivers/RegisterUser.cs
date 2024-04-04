using Truss.Testing.Drivers;
using Truss.Testing.Language;

namespace Truss.Testing.Tests.Drivers;

public class RegisterUser;

public class RegisterUserDriver(RegistrationStore registrationStore) : Driver<RegisterUserDriver>
{
    public override async Task Drive(DslArgs args)
    {
        registrationStore.AddData(args["email"]!);
        await Task.Delay(1);
    }
}