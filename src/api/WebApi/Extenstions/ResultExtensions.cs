using AutoFilterer.Types;
using FluentResults;
using WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApi.Extensions;

public static class ResultExtensions
{
    public static IEnumerable<string> ToErrors<T>(this Result<T> @this) =>
        @this.Errors.Select(e => e.Message);

    public static IEnumerable<string> ToErrors(this Result @this) =>
        @this.Errors.Select(e => e.Message);

    public static ResponseModel<T> ToResponse<T>(this Result<T> @this) =>
        new(@this.ValueOrDefault, @this.ToErrors().ToArray());

    public static ResponseModel ToResponse(this Result @this) =>
        new(@this.ToErrors().ToArray());

    public static PagingResponseModel<T> ToResponse<T>(this Result<T> @this, PaginationFilterBase filter) =>
        new(@this.ValueOrDefault, @this.ToErrors().ToArray(), filter.Page, filter.PerPage);

    public static IActionResult ToNoContent(this Result @this) =>
        @this.IsSuccess
        ? new NoContentResult()
        : new BadRequestObjectResult(@this.ToResponse());

    public static ObjectResult ToObjectResponse<T>(this Result<T> @this) =>
        @this.IsSuccess
        ? new OkObjectResult(@this.ToResponse())
        : new BadRequestObjectResult(@this.ToResponse());

    public static ObjectResult ToObjectResponse<T>(this Result<T> @this, PaginationFilterBase filter) =>
        @this.IsSuccess
        ? new OkObjectResult(@this.ToResponse(filter))
        : new BadRequestObjectResult(@this.ToResponse(filter));
}