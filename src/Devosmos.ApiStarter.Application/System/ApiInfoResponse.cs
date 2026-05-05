namespace Devosmos.ApiStarter.Application.System;

public sealed record ApiInfoResponse(
    string Name,
    string Version,
    string Runtime,
    DateTimeOffset ServerTimeUtc);
