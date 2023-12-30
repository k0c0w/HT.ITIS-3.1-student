using Dotnet.Homeworks.DataAccess.Specs.Infrastructure;
using Dotnet.Homeworks.Domain.Entities;

namespace Dotnet.Homeworks.DataAccess.Specs;

public class UsersSpecs : IUsersSpecs
{
    #region Specs defenitions
    private static readonly Specification<User> _userHasMoreThan11LenghtNameSpec = new (u => u.Name.Length > 11);
    private static readonly Specification<User> _userNameContainsWhiteSpaceSpec = new (u => u.Name.Contains(' '));
    private static readonly Specification<User> _userNameContainsHyphenSpec = new (u => u.Name.Contains('-'));


    private static readonly Specification<User> _userHasGoogleEmailSpec = new (user => user.Email != null && user.Email.ToLower().EndsWith("@gmail.com"));
    private static readonly Specification<User> _userHasMailRuEmailSpec = new (user => user.Email != null && user.Email.ToLower().EndsWith("@mail.ru"));
    private static readonly Specification<User> _userHasYandexEmailSpec = new (user => user.Email != null
                                                                                                && (user.Email.ToLower().EndsWith("@yandex.ru")
                                                                                                   || user.Email.ToLower().EndsWith("@yandex.com")
                                                                                                   || user.Email.ToLower().EndsWith("@yandex.ua")
                                                                                                   || user.Email.ToLower().EndsWith("@yandex.kz")
                                                                                                   || user.Email.ToLower().EndsWith("@yandex.by")
                                                                                                   || user.Email.ToLower().EndsWith("@ya.ru")
                                                                                                   || user.Email.ToLower().EndsWith("@narod.ru")));
    #endregion

    public Specification<User> HasGoogleEmail() => _userHasGoogleEmailSpec;

    public Specification<User> HasYandexEmail() => _userHasYandexEmailSpec;

    public Specification<User> HasMailEmail() => _userHasMailRuEmailSpec;

    public Specification<User> HasPopularEmailVendor() => HasGoogleEmail() | HasYandexEmail() | HasMailEmail();

    public Specification<User> HasLongName() => _userHasMoreThan11LenghtNameSpec;

    public Specification<User> HasCompositeNameWithWhitespace() => _userNameContainsWhiteSpaceSpec;

    public Specification<User> HasCompositeNameWithHyphen() => _userNameContainsHyphenSpec;

    public Specification<User> HasCompositeName() => HasCompositeNameWithHyphen() | HasCompositeNameWithWhitespace();

    public Specification<User> HasComplexName() => HasLongName() & HasCompositeName();
}