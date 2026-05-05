using FluentValidation;

namespace Devosmos.ApiStarter.Application.System;

internal sealed class GetApiInfoRequestValidator : AbstractValidator<GetApiInfoRequest>
{
    public GetApiInfoRequestValidator()
    {
    }
}
