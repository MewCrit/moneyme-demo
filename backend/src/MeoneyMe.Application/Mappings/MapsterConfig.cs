

using Mapster;
using MeoneyMe.Application.DTO;
using MoneyMe.Contracts.Request;
using MoneyMe.Domain;

namespace MeoneyMe.Application.Mappings;

public static class MoneyMeMapsterConfig
{

    public static void RegisterMappings()
    {
        TypeAdapterConfig<ClientLoanRequest, MoneyMeyLoan>.NewConfig();
        TypeAdapterConfig<MoneyMeyLoan, ClientLoanDTO>.NewConfig();

    }
}