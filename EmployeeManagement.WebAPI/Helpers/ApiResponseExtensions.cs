using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Api;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Helpers;

public static class ApiResponseExtensions
{
    public static IActionResult ToStandardLogin(this ControllerBase controller, ApiResult<LoginResponse> result)
    {
        if (!result.Success)
            return controller.Unauthorized(StandardResponseFactory.Failure(result.Error!, "401"));

        return controller.Ok(StandardResponseFactory.Success(result.Data, "Login successful", "200"));
    }

    public static IActionResult ToStandardOk<T>(this ControllerBase controller, ApiResult<T> result, string successMessage, string statusCode = "200")
    {
        if (!result.Success)
            return controller.BadRequest(StandardResponseFactory.Failure(result.Error!, "400"));

        return controller.Ok(StandardResponseFactory.Success(result.Data, successMessage, statusCode));
    }

    public static IActionResult ToStandardNotFound<T>(this ControllerBase controller, ApiResult<T> result, string successMessage)
    {
        if (!result.Success)
            return controller.NotFound(StandardResponseFactory.Failure(result.Error!, "404"));

        return controller.Ok(StandardResponseFactory.Success(result.Data, successMessage));
    }

    public static IActionResult ToStandardCreated<T>(this ControllerBase controller, string actionName, object routeValues, ApiResult<T> result, string successMessage)
    {
        if (!result.Success)
            return controller.BadRequest(StandardResponseFactory.Failure(result.Error!, "400"));

        return controller.CreatedAtAction(
            actionName,
            routeValues,
            StandardResponseFactory.Success(result.Data, successMessage, "201"));
    }

    public static IActionResult ToStandardDelete(this ControllerBase controller, ApiResult<bool> result, string successMessage)
    {
        if (!result.Success)
            return controller.BadRequest(StandardResponseFactory.Failure(result.Error!, "400"));

        return controller.Ok(StandardResponseFactory.Success(null, successMessage, "200"));
    }
}
