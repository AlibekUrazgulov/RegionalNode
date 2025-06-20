﻿using Microsoft.AspNetCore.Routing;

namespace Web.Api.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
