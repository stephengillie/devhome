﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHome.Common.Environments.Helpers;
using DevHome.Common.Environments.Models;
using DevHome.Common.Environments.Services;
using DevHome.SetupFlow.Common.Helpers;
using DevHome.SetupFlow.ViewModels.Environments;

namespace DevHome.Common.Services;

/// <summary>
/// Factory class for creating <see cref="ComputeSystemCardViewModel"/> instances asynchronously.
/// </summary>
public class ComputeSystemViewModelFactory
{
    public async Task<ComputeSystemCardViewModel> CreateCardViewModelAsync(IComputeSystemManager manager, ComputeSystem computeSystem, ComputeSystemProvider provider, string packageFullName)
    {
        var cardViewModel = new ComputeSystemCardViewModel(computeSystem, manager);

        try
        {
            cardViewModel.CardState = await cardViewModel.GetCardStateAsync();
            cardViewModel.ComputeSystemImage = await ComputeSystemHelpers.GetBitmapImageAsync(computeSystem);
            cardViewModel.ComputeSystemProviderName = provider.DisplayName;
            cardViewModel.ComputeSystemProviderImage = CardProperty.ConvertMsResourceToIcon(provider.Icon, packageFullName);
            cardViewModel.ComputeSystemProperties = await ComputeSystemHelpers.GetComputeSystemPropertiesAsync(computeSystem, packageFullName);
        }
        catch (Exception ex)
        {
            Log.Logger.ReportError(Log.Component.ComputeSystemViewModelFactory, $"Failed to get initial properties for compute system {computeSystem}. Error: {ex.Message}");
        }

        return cardViewModel;
    }
}
