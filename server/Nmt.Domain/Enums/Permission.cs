namespace Nmt.Domain.Enums;

public enum Permission : byte
{
    UsersRead = 1,
    UsersCreate = 2,
    UsersUpdate = 3,
    UsersDelete = 4,

    PacketsRead = 5,
    PacketsCreate = 6,
    PacketsUpdate = 7,
    PacketsDelete = 8,

    IpFiltersRead = 9,
    IpFiltersCreate = 10,
    IpFiltersUpdate = 11,
    IpFiltersDelete = 12
}