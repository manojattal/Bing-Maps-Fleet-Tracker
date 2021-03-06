// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trackable.Common;
using Trackable.Models;
using Trackable.Services;

namespace Trackable.Web.Controllers
{
    [Route("api/settings")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService settingsService;
        private readonly IInstrumentationService instrumentationService;

        public SettingsController(
            IInstrumentationService instrumentationService,
            ISettingsService settingsService,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.instrumentationService = instrumentationService.ThrowIfNull(nameof(instrumentationService));
            this.settingsService = settingsService.ThrowIfNull(nameof(settingsService));
        }

        [HttpGet("version")]
        [Authorize(UserRoles.Owner)]
        public async Task<UpdateStatus> GetVersionInfo()
        {
            return await this.settingsService.GetUpdateStatus();
        }

        [HttpGet("subscriptionkeys")]
        [Authorize(UserRoles.TrackingDevice)]
        public IEnumerable<SubscriptionKey> GetSubscriptionKeys()
        {
            return this.settingsService.GetSubscriptionKeys();
        }

        [HttpGet("instrumentation")]
        [Authorize(UserRoles.Owner)]
        public async Task<bool?> GetInstrumentationApproval()
        {
            return await this.instrumentationService.GetInstrumentationApproval();
        }

        [HttpPost("instrumentation")]
        [Authorize(UserRoles.Owner)]
        public async Task SetInstrumentationApproval(bool approval)
        {
            await this.instrumentationService.SetInstrumentationApproval(approval);
        }
    }
}
