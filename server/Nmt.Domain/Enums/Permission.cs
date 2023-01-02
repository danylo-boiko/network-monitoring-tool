namespace Nmt.Domain.Enums;

public enum Permission
{
    UsersRead = 1,
    UsersCreate = 2,
    UsersUpdate = 3,
    UsersDelete = 4,

    PacketsRead = 5,
    PacketsCreate = 6,
    PacketsUpdate = 7,
    PacketsDelete = 8,

    BlockedIpsRead = 9,
    BlockedIpsCreate = 10,
    BlockedIpsUpdate = 11,
    BlockedIpsDelete = 12
}