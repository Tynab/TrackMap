﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using TrackMap.Common.Dtos.Device;
using TrackMap.Common.Responses;
using YANLib;

namespace TrackMap.Pages;

public sealed partial class UserDetailPage
{
    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationState!;

        if (authenticationState.User.Identity is not null && authenticationState.User.Identity.IsAuthenticated && Id.IsNotWhiteSpaceAndNull())
        {
            User = await UserService.Get(new Guid(Id));

            Devices = User?.Devices?.Select(x => new DeviceResponse
            {
                Id = x.Id,
                DeviceType = x.DeviceType,
                DeviceOs = x.DeviceOs,
                IpAddress = x.IpAddress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                LastLogin = x.LastLogin,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy,
                UpdatedAt = x.UpdatedAt,
                Status = x.Status
            }).ToList();
        }
    }

    private async Task HandleDeviceSearch(EditContext context)
    {
        DeviceSearch.UserId = User?.Id;
        Devices = await DeviceService.Search(DeviceSearch);
        ToastService.ShowInfo("Seach completed");
    }

    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationState { get; set; }

    private UserResponse? User { get; set; }

    private List<DeviceResponse>? Devices { get; set; }

    private DeviceSearchDto DeviceSearch { get; set; } = new DeviceSearchDto();
}