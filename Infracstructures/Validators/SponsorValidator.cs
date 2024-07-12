﻿using Application.IValidators;
using FluentValidation;
using Infracstructures.SendModels.Sponsor;

namespace Infracstructures.Validators
{
    public class SponsorValidator : ISponsorValidator
    {
        private readonly IValidator<SponsorRequest> _sponsorvalidator;
        private readonly IValidator<SponsorUpdateRequest> _sponsorupdatevalidator;

        public SponsorValidator(IValidator<SponsorRequest> sponsorvalidator, IValidator<SponsorUpdateRequest> sponsorupdatevalidator)
        {
            _sponsorvalidator = sponsorvalidator;
            _sponsorupdatevalidator = sponsorupdatevalidator;
        }

        public IValidator<SponsorRequest> SponsorRequestValidator => _sponsorvalidator;
        public IValidator<SponsorUpdateRequest> SponsorUpdateRequestValidator => _sponsorupdatevalidator;
    }
}
